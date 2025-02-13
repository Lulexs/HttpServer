using System.Reflection;
using WebServer.Http;
using WebServer.RequestHandlers;

namespace WebServer.Routing;

public class Router
{

    private readonly Dictionary<string, MethodInfo> _routes;

    public Router(Dictionary<string, MethodInfo> routes)
    {
        _routes = routes;
    }

    public RequestHandler GetRequestHandler(HttpRequest request)
    {
        string route = request.RequestLine.RequestTarget;

        string[] path = route.Split("/");

        if (path[0] == "static")
        {
            return new StaticRequestHandler(request);
        }

        return GetControllerHandler(request);
    }

    private RequestHandler GetControllerHandler(HttpRequest request)
    {
        string method = request.RequestLine.HttpMethod;
        string target = request.RequestLine.RequestTarget;
        string route = method + "/" + target;

        if (!_routes.ContainsKey(route))
        {
            return new ErrorRequestHandler(request);
        }
        return new ControllerRequestHandler(request);
    }
}