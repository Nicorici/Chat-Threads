using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Server
    {
        private ChatRoom chatRoom = new ChatRoom();
        private User user;
        private TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5500));

        public void Start()
        {
            listener.Start();
        }

        public void AcceptClient()
        {
            while (true)
            {
                user = new User(listener.AcceptTcpClient());
                chatRoom.Join(user);
            }
        }
    }
}
