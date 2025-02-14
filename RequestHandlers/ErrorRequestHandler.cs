using WebServer.Http;
using WebServer.Http.Objects;

namespace WebServer.RequestHandlers;

public class ErrorRequestHandler : RequestHandler
{
    public ErrorRequestHandler(HttpRequest request) : base(request)
    {
    }

    public override HttpResponse GetResponse()
    {
        Console.WriteLine("ErrorHandler");
        return new HttpResponse() { StatusLine = new StatusLine() { HttpVersion = HttpConstants.Http11, StatusCode = (int)StatusCodes.StatusCodes200Ok } };

    }
}