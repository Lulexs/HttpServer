using WebServer.Http.Objects;

namespace WebServer;

public interface IHttpParser
{
    public RequestLine GetRequestLine();
}