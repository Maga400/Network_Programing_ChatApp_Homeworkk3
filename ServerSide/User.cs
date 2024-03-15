

using System.Net.Sockets;

namespace ServerSide;

public class User
{
    public string UserName { get; set; }
    public TcpClient TcpClient  { get; set; }
}
