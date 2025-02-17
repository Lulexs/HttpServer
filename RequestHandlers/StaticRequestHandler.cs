using System.Text;
using WebServer.Http;

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

        if (!File.Exists(resourcePath))
        {

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

            resourcePath += "." + extension;
        }
        else
        {
            FileInfo fi = new(resourcePath);
            extension = fi.Extension;
        }
        HeaderOptions opts;
        FormResponse fr;
        if (HttpConstants.IsBinary(extension![1..]))
        {
            byte[] bytes = File.ReadAllBytes(resourcePath);
            opts = new()
            {
                ContentType = HttpConstants.GetContentType(extension![1..]),
                Connection = _request.Header.Connection ?? "close"
            };
            fr = new(StatusCodes.StatusCodes200Ok, opts, bytes);
            return fr.GetResponse();
        }

        string text = File.ReadAllText(resourcePath);
        opts = new()
        {
            ContentType = HttpConstants.GetContentType(extension![1..]),
            Connection = _request.Header.Connection ?? "close"
        };
        fr = new(StatusCodes.StatusCodes200Ok, opts, Encoding.UTF8.GetBytes(text));
        return fr.GetResponse();
    }
}