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
            throw new InvalidFormatException();
        }

        string[] requestLineParams = _request[.._pointer].Split(HttpConstants.Space);
        if (requestLineParams.Length != 3)
        {
            throw new InvalidFormatException();
        }

        return new RequestLine(requestLineParams[0], requestLineParams[1], requestLineParams[2]);
    }

    public Header GetHeader()
    {
        _pointer += HttpConstants.NewLine.Length;
        int lineEnd = _request[_pointer..].AsSpan().IndexOf(HttpConstants.NewLine);

        Header header = new();

        while (lineEnd != 0 && lineEnd != -1)
        {
            string field = _request[_pointer..(lineEnd + _pointer)];
            int splitPoint = field.AsSpan().IndexOf(":");

            string fieldName = field[..splitPoint];
            string fieldVal = field[(splitPoint + 1)..];

            if (fieldVal[0] == ' ')
            {
                fieldVal = fieldVal[1..];
            }
            if (fieldVal[^1] == ' ')
            {
                fieldVal = fieldVal[..^1];
            }

            try
            {
                header[fieldName.Trim()] = fieldVal;
            }
            catch (ArgumentException)
            {

            }

            _pointer += lineEnd + HttpConstants.NewLine.Length;
            lineEnd = _request[_pointer..].AsSpan().IndexOf(HttpConstants.NewLine);
        }

        return header;
    }
}