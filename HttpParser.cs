using System.Text;
using WebServer.Exceptions;
using WebServer.Http;
using WebServer.Http.Objects;

namespace WebServer;

public class HttpParser(byte[] request) : IHttpParser
{

    private readonly string _request = Encoding.UTF8.GetString(request);
    private int _pointer = 0;

    public RequestLine GetRequestLine()
    {
        _pointer = _request.AsSpan().IndexOf(HttpConstants.NewLine);
        if (_pointer == 0)
        {
            throw new InvalidFormat();
        }

        string[] requestLineParams = _request[.._pointer].Split(HttpConstants.Space);
        if (requestLineParams.Length != 3)
        {
            throw new InvalidFormat();
        }

        ++_pointer;
        return new RequestLine(requestLineParams[0], requestLineParams[1], requestLineParams[2]);
    }
}