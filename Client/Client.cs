using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Components;

namespace Client
{
    public class Client
    {
        private TcpClient tcpClient;
        private ChatStream stream;
        private string name = "";
        public string Name { get => name; }

        public Client()
        {
            tcpClient = new TcpClient();
            SetName();
        }

        public void Connect(IPEndPoint endpoint)
        {
            tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5500));
            stream = new ChatStream(tcpClient.GetStream());
            BeginConversation();
        }

        private void BeginConversation()
        {
            new Thread(ReceiveMessages).Start();
            try
            {
                SendMessages(new Message(name + '\0'));
            }
            catch (Exception)
            {
                Console.WriteLine("The connection to the server has been lost or the server is not responding...");
                Close();
            }
        }

        public void ReceiveMessages()
        {
            try
            {
                while (true)
                {
                    Message m = stream.Read();
                    Console.WriteLine(m);
                }
            }
            catch
            {
                Console.WriteLine("The connection to the server has been lost or the server is not responding...");
                Close();
                return;
            }
        }

        public void Close()
        {
            stream.Close();
            tcpClient.Dispose();
            tcpClient.Close();
        }

        public void SendMessages(Message message)
        {
            stream.Write(message);
            string input = "";
            while (string.IsNullOrWhiteSpace(input))
            {
                input = Console.ReadLine();
                DeleteInputLine();
            }
            SendMessages(new Message($"{name} : {input}\0"));
        }

        private void SetName()
        {
            Console.Write("Please input your name (1-20 characters,no \":\" characters) : ");
            bool isInvalid = true;
            while (isInvalid)
            {
                name = Console.ReadLine();
                if (name.Length < 1 || string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("The name does not contain any characteres...Please input your name again : ");
                    continue;
                }
                if (name.Length > 20)
                {
                    Console.WriteLine("The name is too long...Please input your name again : ");
                    continue;
                }
                if (name.Contains(':'))
                {
                    Console.WriteLine("The name cannot contain a the \":\" character...Please input again :");
                    continue;
                }
                isInvalid = false;
            }
        }

        private static void DeleteInputLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }
}
