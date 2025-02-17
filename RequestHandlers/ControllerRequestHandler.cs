using System.Reflection;
using WebServer.Http;
using WebServer.Http.Objects;

namespace WebServer.RequestHandlers;

public class ControllerRequestHandler : RequestHandler
{
    private readonly MethodInfo _method;

    public ControllerRequestHandler(HttpRequest request, MethodInfo method) : base(request)
    {
        _method = method;
    }

    public override HttpResponse GetResponse()
    {
        Type type = _method.DeclaringType!;
        object? instance = Activator.CreateInstance(type);
        if (instance != null)
        {
            _method.Invoke(instance, null);
        }
        Console.WriteLine(_request.RequestLine.RequestTarget);
        return new HttpResponse() { StatusLine = new StatusLine() { HttpVersion = HttpConstants.Http11, StatusCode = (int)StatusCodes.StatusCodes200Ok } };

    }
}