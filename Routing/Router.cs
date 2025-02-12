using WebServer.Http;
using WebServer.RequestHandlers;

namespace WebServer.Routing;

public static class Router
{
    public static RequestHandler GetRequestHandler(HttpRequest request)
    {
        string route = request.RequestLine.RequestTarget;

        string[] path = route.Split("/");


    }
}