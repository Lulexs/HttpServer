using WebServer.Http.Objects;

namespace WebServer.Http;

public class HttpRequest
{
    public required RequestLine RequestLine { get; init; }
    public required RequestHeader Header { get; init; }
    public Body? Body { get; set; }
}