using WebServer.Http.Rules;

namespace WebServer.Http.Objects;

public readonly struct RequestLine
{
    public readonly string HttpMethod;
    public readonly string RequestTarget;
    public readonly string HttpVersion;

    public RequestLine(string httpMethod, string requestTarget, string httpVersion) : this()
    {
        HttpMethod = HttpValidation.IsValidHttpMethod(httpMethod);
        RequestTarget = requestTarget;
        HttpVersion = HttpValidation.IsValidHttpVersion(httpVersion);
    }

    public override string ToString()
    {
        return $"{HttpMethod} {RequestTarget} {HttpVersion}";
    }
}