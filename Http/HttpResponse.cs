using System.Text;
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

        if (Body is not null && Body.Value is not null && Header is not null && Header.ContentType is not null && !HttpConstants.IsBinary(Header.ContentType.Split("/")[1]))
        {
            resp += Encoding.UTF8.GetString(Body.Value);
        }

        return resp;
    }
}