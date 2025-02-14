using WebServer.Http;
using WebServer.Http.Objects;

namespace WebServer.RequestHandlers;

public class ControllerRequestHandler : RequestHandler
{
    public ControllerRequestHandler(HttpRequest request) : base(request)
    {
    }

    public override HttpResponse GetResponse()
    {
        Console.WriteLine("Controller handler");
        return new HttpResponse() { StatusLine = new StatusLine() { HttpVersion = HttpConstants.Http11, StatusCode = (int)StatusCodes.StatusCodes200Ok } };

    }
}