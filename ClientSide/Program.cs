using ClientSide;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;

var serverEP = new IPEndPoint(ip, port);

var client = new TcpClient();

client.Connect(serverEP);

NetworkStream? clientData = client.GetStream();

BinaryReader? reader = new BinaryReader(clientData);
BinaryWriter? writer = new BinaryWriter(clientData);

_ = Task.Run(() =>
{
    while (true)
    {
        var message = reader.ReadString();

        var msg = JsonSerializer.Deserialize<Sender>(message);
        Console.WriteLine(msg);

    }
});


bool isCheck = true;
var recepientName="";
while (true)
{
    if (isCheck)
    {
        Console.Write("Enter Your Name: ");
        recepientName = Console.ReadLine();
        writer.Write(recepientName);
        isCheck = false;
    }
    else
    {
        Console.Write("Enter Recepient Name: ");
        var name = Console.ReadLine();
        Console.Write("Enter your Message: ");
        var message = Console.ReadLine();


        var msg = new Sender()
        {
            Name = name,
            RecepientName = recepientName,
            Message = message
        };

        var msgJson = JsonSerializer.Serialize(msg);
        writer.Write(msgJson);
    }

}
