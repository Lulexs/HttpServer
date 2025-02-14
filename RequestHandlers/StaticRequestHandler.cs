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
        if (_request.Header.ContentType is not null)
        {
            resourcePath += "." + _request.Header.ContentType.Split("/")[1];
        }

        if (!File.Exists(resourcePath))
        {
            // RETURN FILE DOESNT EXIST
        }

        return new HttpResponse() { StatusLine = new StatusLine() { HttpVersion = HttpConstants.Http11, StatusCode = (int)StatusCodes.StatusCodes200Ok } };
    }
}