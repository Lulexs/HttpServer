using WebServer.Http;
using WebServer.Http.Objects;

namespace WebServer.RequestHandlers;

public class StaticRequestHandler : RequestHandler
{
    public StaticRequestHandler(HttpRequest request) : base(request)
    {
    }

    public override void GetResponse()
    {
        Console.WriteLine("Static handler");
    }
}