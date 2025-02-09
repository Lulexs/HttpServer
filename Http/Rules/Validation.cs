using WebServer.Exceptions;
using WebServer.Http.Objects;

namespace WebServer.Http.Rules;

public static class HttpValidation
{
    public static bool IsValidHttpRequest(HttpRequest req)
    {
        return IsValidRequestLine(req.RequestLine) && IsValidRequestHeader(req.Header);
    }

    public static bool IsValidRequestHeader(RequestHeader RequestHeader)
    {
        return IsValidConnection(RequestHeader.Connection);
    }

    public static bool IsValidConnection(string? connection)
    {
        return connection == null || HttpConstants.ConnectionOptions.Contains(connection);
    }

    public static bool IsValidRequestLine(RequestLine rl)
    {
        return IsValidHttpMethod(rl.HttpMethod) && IsValidHttpVersion(rl.HttpVersion);
    }

    public static bool IsValidHttpMethod(string method)
    {
        if (!HttpConstants.Methods.Contains(method))
        {
            throw new UnrecognizedMethodException($"{method} is not valid Http method");
        }
        return true;
    }

    public static bool IsValidHttpVersion(string version)
    {
        if (!HttpConstants.Versions.Contains(version))
        {
            throw new VersionNotSupportedException($"{version} is not supported");
        }
        return true;
    }
}