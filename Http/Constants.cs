namespace WebServer.Http;

public static class HttpConstants
{
    public const string Space = " ";
    public const string NewLine = "\r\n";
    public const string Get = "GET";
    public const string Post = "POST";
    public const string Put = "PUT";
    public const string Delete = "DELETE";
    public static readonly string[] Methods = [Get, Post, Put, Delete];
    public const string Http1 = "HTTP/1.0";
    public const string Http11 = "HTTP/1.1";
    public static readonly string[] Versions = [Http11];
    public const string HeaderFieldRegex = @"^([A-Za-z-]+):[ \t]*([^\r\n]*?)[ \t]*\r\n";
    public static readonly HashSet<string> ConnectionOptions = ["close", "keep-alive"];
    public static string GetContentType(string? ext)
    {
        if (ext == "jpg" || ext == "png")
        {
            return $"image/{ext}";
        }
        else if (ext == "txt" || ext == "html" || ext == "css" || ext == "js")
        {
            return $"text/{ext}";
        }
        else if (ext == "json" || ext == "xml")
        {
            return $"application/{ext}";
        }
        return "application/octet-stream";
    }

}