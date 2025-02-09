using WebServer.Http.Objects;

namespace WebServer.Http;

public class HttpResponse
{
    public required StatusLine StatusLine { get; set; }
    public ResponseHeader? Header { get; set; }
    public Body? Body { get; set; }

    public override string ToString()
    {
        var resp = $"{StatusLine}{HttpConstants.NewLine}";

        if (Header is not null)
        {
            resp += $"{Header}{HttpConstants.NewLine}";
        }

        resp += HttpConstants.NewLine;

        return resp;
    }
}