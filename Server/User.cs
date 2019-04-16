using System;
using System.Net.Sockets;
using System.Text;
using Components;

namespace Server
{
    public class User
    {
        private TcpClient client;
        public ChatStream stream;
        public string Name { get; internal set; }

        public User(TcpClient client)
        {
            this.client = client;
            this.stream = new ChatStream(client.GetStream());
        }

        public void Send(Message message)
        {
            stream.Write(message);
        }

        public Message Receive()
        {
            return stream.Read();
        }

        public void Close()
        {
            stream.Close();
            client.Dispose();
            client.Close();
        }
    }
}
