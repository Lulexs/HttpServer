using WebServer.Http;

namespace WebServer.RequestHandlers;

public abstract class RequestHandler
{
    private readonly HttpRequest _request;
    public RequestHandler(HttpRequest request)
    {
        _request = request;
    }

    public abstract void GetResponse();
}