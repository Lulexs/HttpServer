namespace WebServer.Http.Objects;

public class StatusLine
{
    public required string HttpVersion { get; set; }
    public int StatusCode { get; set; }
    public string? ReasonPhrase { get; set; }

    public override string ToString()
    {
        return $"{HttpVersion} {StatusCode}" + (ReasonPhrase is not null ? $" {ReasonPhrase}" : "");
    }
}