using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Text;
using WebServer.Exceptions;
using WebServer.Http;
using WebServer.Http.Objects;
using WebServer.Http.Rules;
using WebServer.Parsing.Parser;

namespace WebServer;

public class ConnectionHandler
{
    private readonly Socket? _connection;
    private const int WorkBufferSize = 4096;
    private const int ReadBufferSize = 1024;

    public ConnectionHandler(Socket? socket)
    {
        _connection = socket;
    }

    static int GetSubarrayIndexOf(byte[] array, byte[] subarray)
    {
        if (subarray.Length == 0 || array.Length < subarray.Length)
            return -1;

        for (int i = 0; i <= array.Length - subarray.Length; i++)
        {
            if (array.AsSpan(i, subarray.Length).SequenceEqual(subarray))
                return i;
        }

        return -1;
    }

    public async Task Handle()
    {
        var buffer = new byte[WorkBufferSize];
        var recvBuffer = new byte[ReadBufferSize];

        try
        {
            int totalRecieved = 0;
            int remainder = 0;
            int offsetInReadBuff = 0;

            while (true)
            {
                if (_connection is null)
                {
                    break;
                }

                if (remainder != 0)
                {
                    recvBuffer[offsetInReadBuff..(offsetInReadBuff + remainder)].CopyTo(buffer, totalRecieved);
                    totalRecieved += remainder;
                    remainder = 0;
                }

                var recieved = await _connection.ReceiveAsync(recvBuffer, SocketFlags.None);

                if (recieved == 0)
                {
                    break;
                }

                int eohLine = GetSubarrayIndexOf(recvBuffer, Encoding.UTF8.GetBytes("\r\n\r\n"));

                while (eohLine == -1)
                {

                    if (totalRecieved + recieved > ReadBufferSize)
                    {
                        // WRITE RESPONSE REQUEST HEADER TOO LARGE
                        // CLOSE CONNECTION
                    }

                    recvBuffer.CopyTo(buffer, totalRecieved);
                    totalRecieved += recieved;

                    recieved = await _connection.ReceiveAsync(recvBuffer, SocketFlags.None);
                    eohLine = GetSubarrayIndexOf(recvBuffer, Encoding.UTF8.GetBytes("\r\n\r\n"));
                }

                if (totalRecieved + eohLine + 4 > WorkBufferSize)
                {
                    // WIRTE RESPONSE REQUEST HEADER TOO LARGE
                    // CLOSE CONNECTION
                }
                else
                {
                    recvBuffer[..(eohLine + 4)].CopyTo(buffer, totalRecieved);
                    totalRecieved += eohLine + 4;
                    remainder = recieved - eohLine - 4;
                }

                HttpParser parser = new(buffer[..totalRecieved]);
                totalRecieved = 0;
                HttpRequest? request = null;
                try
                {
                    request = parser.GetRequest();
                    recvBuffer[(eohLine + 4)..(eohLine + 4 + remainder)].CopyTo(buffer, 0);
                    totalRecieved = remainder;

                    if (request.Header.ContentLength is not null && request.Header.ContentLength > WorkBufferSize)
                    {
                        // DONT WANT TO PROCESS BODY LARGER THAN few KB
                    }
                    else if (request.Header.ContentLength is not null)
                    {
                        if (request.Header.ContentLength > (ulong)remainder)
                        {
                            recieved = await _connection.ReceiveAsync(buffer);

                            buffer[..recieved].CopyTo(buffer, remainder);
                            recvBuffer[(eohLine + 4)..(eohLine + 4 + remainder)].CopyTo(buffer, 0);

                            request.Body = new() { Value = new byte[(int)request.Header.ContentLength] };
                            buffer[..(int)request.Header.ContentLength].CopyTo(request.Body.Value, 0);
                            remainder = 0;
                            offsetInReadBuff = 0;
                            totalRecieved = 0;
                        }
                        else
                        {
                            request.Body = new() { Value = new byte[(int)request.Header.ContentLength] };
                            recvBuffer[(eohLine + 4)..(eohLine + 4 + (int)request.Header.ContentLength)].CopyTo(request.Body.Value, 0);
                            remainder -= (int)request.Header.ContentLength;
                            offsetInReadBuff = eohLine + 4 + (int)request.Header.ContentLength;
                            totalRecieved = 0;
                        }
                    }
                }
                catch (TerminalInvalidFormatException)
                {
                    await _connection.DisconnectAsync(false);
                    return;
                }

                if (request is null)
                {
                    await _connection.DisconnectAsync(false);
                    return;
                }

                Http.Rules.ValidationResult res = ValidateRequest.Validate(request);
                if (res.ErrorCode is not null)
                {

                    HeaderOptions opts = new() { Connection = request.Header.Connection };
                    FormResponse fr = new(res.ErrorCode.Value, opts);
                    HttpResponse invalidRequestResponse = fr.GetResponse();
                    await _connection.SendAsync(Encoding.UTF8.GetBytes(invalidRequestResponse.ToString()), 0);
                }
                else
                {
                    // MAPPING ROUTES
                }

                if (request.Header.Connection == "close" || !HttpValidation.IsValidConnection(request.Header.Connection))
                {
                    await _connection.DisconnectAsync(false);
                }
            }
        }
        catch (Exception ec)
        {
            Console.WriteLine(ec);
        }
        finally
        {
            _connection?.Dispose();
        }
    }

}