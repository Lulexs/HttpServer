using System.Net;
using System.Net.Sockets;
using WebServer;


using var listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
listener.Bind(new IPEndPoint(IPAddress.Loopback, 8080));
listener.Listen(100);

while (true)
{
    var connection = await listener.AcceptAsync();

    _ = Task.Run(async () =>
    {
        ConnectionHandler connHandler = new(connection);
        await connHandler.Handle();
    });
}
