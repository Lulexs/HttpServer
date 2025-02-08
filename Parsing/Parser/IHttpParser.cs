using WebServer.Http;

namespace WebServer.Parsing.Parser;

public interface IHttpParser
{
    public HttpRequest GetRequest();
}