using WebServer.Exceptions;

namespace WebServer.Http.Rules;

public static class HttpValidation
{
    public static bool IsValidConnection(string? connection)
    {
        return connection == null || HttpConstants.ConnectionOptions.Contains(connection);
    }

    public static bool IsValidHttpMethod(string method)
    {
        if (!HttpConstants.Methods.Contains(method))
        {
            return false;
        }
        return true;
    }

    public static bool IsValidHttpVersion(string version)
    {
        if (!HttpConstants.Versions.Contains(version))
        {
            return false;
        }
        return true;
    }
}