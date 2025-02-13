using WebServer.Http;

namespace WebServer.RequestHandlers;

public class ControllerRequestHandler : RequestHandler
{
    public ControllerRequestHandler(HttpRequest request) : base(request)
    {
    }

    public override void GetResponse()
    {
        throw new NotImplementedException();
    }
}