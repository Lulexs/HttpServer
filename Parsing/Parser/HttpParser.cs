using System.Text;
using System.Text.RegularExpressions;
using WebServer.Exceptions;
using WebServer.Http;
using WebServer.Http.Objects;

namespace WebServer.Parsing.Parser;

public class HttpParser(byte[] request) : IHttpParser
{

    private readonly string _request = Encoding.UTF8.GetString(request);
    private int _pointer = 0;

    public HttpRequest GetRequest()
    {
        RequestLine rl = GetRequestLine();
        RequestHeader header = GetHeader();
        return new HttpRequest() { RequestLine = rl, Header = header };
    }

    protected RequestLine GetRequestLine()
    {
        _pointer = _request.AsSpan().IndexOf(HttpConstants.NewLine);
        if (_pointer == 0)
        {
            throw new TerminalInvalidFormatException();
        }

        string[] requestLineParams = _request[.._pointer].Split(HttpConstants.Space);
        if (requestLineParams.Length != 3)
        {
            throw new TerminalInvalidFormatException();
        }

        return new RequestLine(requestLineParams[0], requestLineParams[1], requestLineParams[2]);
    }

    protected RequestHeader GetHeader()
    {
        _pointer += HttpConstants.NewLine.Length;
        Regex regex = new(HttpConstants.HeaderFieldRegex, RegexOptions.Compiled | RegexOptions.Multiline);

        var matches = regex.Matches(_request[_pointer..]);

        RequestHeader header = new();
        for (int i = 0; i < matches.Count; ++i)
        {
            if (i > 0 && matches[i - 1].Index + matches[i - 1].Length != matches[i].Index)
            {
                throw new TerminalInvalidFormatException();
            }

            string field = matches[i].Value;
            int splitPoint = field.AsSpan().IndexOf(":");
            string fieldName = field[..splitPoint];
            string fieldVal = field[(splitPoint + 1)..];

            try
            {
                header[fieldName.Trim()] = fieldVal.Trim();
            }
            catch (ArgumentException)
            {

            }
        }

        return header;
    }


    // public Header GetHeader()
    // {
    //     _pointer += HttpConstants.NewLine.Length;
    //     int lineEnd = _request[_pointer..].AsSpan().IndexOf(HttpConstants.NewLine);

    //     Header header = new();

    //     while (lineEnd != 0 && lineEnd != -1)
    //     {
    //         string field = _request[_pointer..(lineEnd + _pointer)];
    //         int splitPoint = field.AsSpan().IndexOf(":");

    //         string fieldName = field[..splitPoint];
    //         string fieldVal = field[(splitPoint + 1)..];

    //         if (fieldVal[0] == ' ')
    //         {
    //             fieldVal = fieldVal[1..];
    //         }
    //         if (fieldVal[^1] == ' ')
    //         {
    //             fieldVal = fieldVal[..^1];
    //         }

    //         try
    //         {
    //             header[fieldName.Trim()] = fieldVal;
    //         }
    //         catch (ArgumentException)
    //         {

    //         }

    //         _pointer += lineEnd + HttpConstants.NewLine.Length;
    //         lineEnd = _request[_pointer..].AsSpan().IndexOf(HttpConstants.NewLine);
    //     }

    //     return header;
    // }
}