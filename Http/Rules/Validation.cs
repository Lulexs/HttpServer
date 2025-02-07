using WebServer.Exceptions;

namespace WebServer.Http.Rules;

public static class HttpValidation
{
    public static string IsValidHttpMethod(string method)
    {
        if (!HttpConstants.Methods.Contains(method))
        {
            throw new UnrecognizedMethodException($"{method} is not valid Http method");
        }
        return method;
    }

    public static string IsValidHttpVersion(string version)
    {
        if (!HttpConstants.Versions.Contains(version))
        {
            throw new VersionNotSupportedException($"{version} is not supported");
        }
        return version;
    }
}