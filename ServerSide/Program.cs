using ServerSide;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;


var listenerEP = new IPEndPoint(ip, port);

TcpListener listener = new TcpListener(listenerEP);

listener.Start();
Console.WriteLine($@"{listener.Server.LocalEndPoint} => Listener Connected ....");


while (true)
{
    var client = listener.AcceptTcpClient();

    _ = Task.Run(async () =>
    {
        var clientData = client.GetStream();
        var clientReader = new BinaryReader(clientData);
        var clientWriter = new BinaryWriter(clientData);

        bool isCheck = true;
        var username = "";
        while (true)
        {
            if (isCheck)
            {
                //clientWriter.Write("Username: ");
                //await Task.Delay(500);

                username = clientReader.ReadString();
                DataBase.Users.Add(new User()
                {
                    UserName = username.ToLower(),
                    TcpClient = client
                });

                Console.Write($@"{client.Client.RemoteEndPoint} => ");
                Console.WriteLine($"{username} is connected ....");


                isCheck = false;
            }

            if (!isCheck)
            {
                var message = clientReader.ReadString();

                Sender? msg = JsonSerializer.Deserialize<Sender>(message);
                Sender? sendMsg = new Sender()
                {
                    Name = username,
                    RecepientName = msg.Name,
                    Message = msg.Message
                };
                Console.WriteLine(sendMsg);

                if (msg is not null)
                {
                    var user = DataBase.Users.FirstOrDefault(x => x.UserName.ToLower() == msg.Name.ToLower());
                    if (user is not null)
                    {
                        BinaryWriter? userWriter = new BinaryWriter(user.TcpClient.GetStream());


                        var a = JsonSerializer.Serialize<Sender>(sendMsg);
                        userWriter.Write(a);
                    }
                }
            }
        }
    });
}



static class DataBase
{
    public static List<User> Users = new List<User>();
}