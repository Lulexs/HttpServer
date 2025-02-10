using System.Reflection;
using WebServer.Http;
using WebServer.Http.Objects;

namespace WebServer;

public class HeaderOptions
{
    public string? ContentType { get; set; }
    public string? Connection { get; set; }
}

public class FormResponse(StatusCodes statusCode, HeaderOptions? headerOptions = null, byte[]? body = null)
{
    private readonly StatusCodes _statusCode = statusCode;
    private readonly HeaderOptions? _headerOptions = headerOptions;
    private readonly byte[]? _body = body;

    public HttpResponse GetResponse()
    {
        StatusLine sl = new()
        {
            HttpVersion = HttpConstants.Http11,
            StatusCode = (int)_statusCode,
            ReasonPhrase = _statusCode.ToString()[14..]
        };

        ResponseHeader stdHeader = new()
        {
            Connection = "keep-alive",
            ContentType = "text/plain",
            Server = "Custom Http Server"
        };

        if (_headerOptions is not null)
        {
            var properties = _headerOptions.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(_headerOptions, null);
                if (value != null)
                {
                    var fieldName = prop.Name;

                    stdHeader[fieldName] = value;
                }
            }
        }

        HttpResponse resp;
        if (_body is not null)
        {
            stdHeader["ContentLength"] = _body.Length;
            resp = new()
            {
                StatusLine = sl,
                Header = stdHeader,
                Body = new Body() { Value = _body }
            };
        }
        else
        {
            resp = new()
            {
                StatusLine = sl,
                Header = stdHeader,
            };
        }

        return resp;
    }

}