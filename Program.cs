using System.Net;
using System.Net.Sockets;
using WebServer;
using WebServer.Routing;


var routes = RouteTable.GetRoutes();


using var listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
listener.Bind(new IPEndPoint(IPAddress.Loopback, 8080));
listener.Listen(100);

while (true)
{
    var connection = await listener.AcceptAsync();

    _ = Task.Run(async () =>
    {
        ConnectionHandler connHandler = new(connection, new Router(routes));
        await connHandler.Handle();
    });
}




// using System.Text;
// using WebServer.Parsing.Parser;

// string req = @"DELETE /static/123 HTTP/1.1
// content-length: 15
// accept-encoding: gzip, deflate, br
// Accept: */*
// User-Agent: Thunder Client (https://www.thunderclient.com)
// Content-Type: application/json
// Host: localhost:8080
// Connection: close";

// HttpParser p = new HttpParser(Encoding.UTF8.GetBytes(req));
// p.GetRequestLine();
// Console.WriteLine(p.GetHeader());