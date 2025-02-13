using WebServer.Http;
using WebServer.RequestHandlers;

namespace WebServer.Routing;

public static class Router
{
    public static void GetRequestHandler(HttpRequest request)
    {
        string route = request.RequestLine.RequestTarget;

        string[] path = route.Split("/");

        if (path[0] == "static")
        {
            // return new StaticRequestHandler(request);
        }

        // return GetControllerHandler(request);
    }

    // private static RequestHandler GetControllerHandler(HttpRequest request)
    // {

    // }
}