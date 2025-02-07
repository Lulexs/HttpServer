// using System.Net;
// using System.Net.Sockets;
// using System.Text;
// using WebServer;


// using var listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
// listener.Bind(new IPEndPoint(IPAddress.Loopback, 8080));
// listener.Listen(100);

// while (true)
// {
//     var connection = await listener.AcceptAsync();

//     _ = Task.Run(async () =>
//     {
//         var buffer = new byte[1024];
//         var recieved = await connection.ReceiveAsync(buffer, SocketFlags.None);

//         var response = Encoding.UTF8.GetString(buffer, 0, recieved);
//         await connection.SendAsync(Encoding.UTF8.GetBytes(response), 0);
//     });
// }

using WebServer.Http.Objects;

RequestLine rl = new("PATCH", "/static/123", "HTTP1.1");
Console.WriteLine(rl);