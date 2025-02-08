using System.Net.Sockets;
using System.Text;
using WebServer.Http;
using WebServer.Http.Objects;
using WebServer.Parsing.Parser;

namespace WebServer;

public class ConnectionHandler
{
    private readonly Socket? _connection;

    public ConnectionHandler(Socket? socket)
    {
        _connection = socket;
    }

    public async Task Handle()
    {
        var buffer = new byte[1024];

        try
        {
            while (true)
            {
                if (_connection is null)
                {
                    break;
                }

                var recieved = await _connection.ReceiveAsync(buffer, SocketFlags.None);

                if (recieved == 0)
                {
                    break;
                }

                Console.WriteLine(Encoding.UTF8.GetString(buffer));
                HttpParser parser = new(buffer);
                HttpRequest request = parser.GetRequest();
                Console.WriteLine(request.Header);

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