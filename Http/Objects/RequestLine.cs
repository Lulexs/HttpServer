namespace WebServer.Http.Objects;

public readonly struct RequestLine
{
    public readonly string HttpMethod;
    public readonly string RequestTarget;
    public readonly string HttpVersion;

    public RequestLine(string httpMethod, string requestTarget, string httpVersion) : this()
    {
        HttpMethod = httpMethod;
        RequestTarget = requestTarget;
        HttpVersion = httpVersion;
    }

    public override string ToString()
    {
        return $"{HttpMethod} {RequestTarget} {HttpVersion}";
    }
}