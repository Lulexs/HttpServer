using System.Net.Sockets;
using System.Text;
using WebServer.Http;
using WebServer.Parsing.Parser;

namespace WebServer;

public class ConnectionHandler
{
    private readonly Socket? _connection;

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
        var buffer = new byte[512];
        var recvBuffer = new byte[64];

        try
        {
            int totalRecieved = 0;
            int remainder = 0;

            while (true)
            {
                if (_connection is null)
                {
                    break;
                }

                var recieved = await _connection.ReceiveAsync(recvBuffer, SocketFlags.None);

                if (recieved == 0)
                {
                    break;
                }

                int eohLine = GetSubarrayIndexOf(recvBuffer, Encoding.UTF8.GetBytes("\r\n\r\n"));

                while (eohLine == -1)
                {

                    if (totalRecieved + recieved > 512)
                    {
                        // WRITE RESPONSE REQUEST HEADER TOO LARGE
                        // CLOSE CONNECTION
                    }

                    recvBuffer.CopyTo(buffer, totalRecieved);
                    totalRecieved += recieved;

                    recieved = await _connection.ReceiveAsync(recvBuffer, SocketFlags.None);
                    eohLine = GetSubarrayIndexOf(recvBuffer, Encoding.UTF8.GetBytes("\r\n\r\n"));
                }

                if (totalRecieved + eohLine + 4 > 512)
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
                Console.WriteLine(Encoding.UTF8.GetString(buffer[..totalRecieved]));
                Console.WriteLine(Encoding.UTF8.GetString(buffer[..totalRecieved]).Length);
                Console.WriteLine(remainder);
                HttpParser parser = new(buffer[..totalRecieved]);

                HttpRequest request = parser.GetRequest();

                var response = Encoding.UTF8.GetString(buffer, 0, recieved);
                await _connection.SendAsync(Encoding.UTF8.GetBytes(response), 0);
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