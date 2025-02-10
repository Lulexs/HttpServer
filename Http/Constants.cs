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
}