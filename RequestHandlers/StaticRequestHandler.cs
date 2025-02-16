using WebServer.Http;
using WebServer.Http.Objects;

namespace WebServer.RequestHandlers;

public class StaticRequestHandler : RequestHandler
{
    public StaticRequestHandler(HttpRequest request) : base(request)
    {
    }


    public override HttpResponse GetResponse()
    {
        if (_request.RequestLine.HttpMethod != HttpConstants.Get)
        {
            // RETURN METHOD UNSUPPORTED
        }

        var curDir = Directory.GetParent(
            Directory.GetParent(
                Directory.GetParent(
                    Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.FullName)!.FullName)!.FullName)!.FullName;
        var staticAssetsPath = Path.Combine(curDir, "wwwroot");

        if (!Directory.Exists(staticAssetsPath))
        {
            // RETURN SERVER ERROR RESPONSE
        }

        string target = _request.RequestLine.RequestTarget;
        string resource = target[8..];

        var resourcePath = Path.Combine(staticAssetsPath, resource);
        string? extension = null;
        if (_request.Header.ContentType is not null)
        {
            extension = _request.Header.ContentType.Split("/")[1];
        }
        else
        {
            string[] matchingFiles = Directory.GetFiles(staticAssetsPath, resource + ".*");

            if (matchingFiles.Length != 0)
            {
                FileInfo fi = new(matchingFiles.First());
                extension = fi.Extension;
            }
            else
            {
                // RETURN FIEL DOSNT EXIST
            }
        }

        resourcePath += extension;

        byte[] bytes = File.ReadAllBytes(resourcePath);
        HeaderOptions opts = new()
        {
            ContentType = HttpConstants.GetContentType(extension![1..]),
            Connection = "close"
        };
        FormResponse fr = new(StatusCodes.StatusCodes200Ok, opts, bytes);
        return fr.GetResponse();
    }
}