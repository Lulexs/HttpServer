using WebServer.Http;

namespace WebServer.RequestHandlers;

public class ErrorRequestHandler : RequestHandler
{
    public ErrorRequestHandler(HttpRequest request) : base(request)
    {
    }

    public override void GetResponse()
    {
        throw new NotImplementedException();
    }
}