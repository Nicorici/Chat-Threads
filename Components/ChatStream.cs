using System;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace Components
{
    public class ChatStream
    {
        private NetworkStream stream;
        int bufferSize = 0;
        private byte[] buffer = new byte[255];
        private int pendingBytes = 0;
        public bool StreamIsOn { get => stream.CanRead && stream.CanWrite; }

        public ChatStream(NetworkStream stream)
        {
            this.stream = stream;
            this.bufferSize = buffer.Length;
        }

        public Message Read()
        {
            int bytesRead = stream.Read(buffer, pendingBytes, buffer.Length - pendingBytes);

            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            int indexEnd = message.LastIndexOf('\0');
            if (indexEnd == -1)
            {
                Array.Resize(ref buffer, bytesRead + bufferSize);
                pendingBytes = bytesRead;
                Read();
            }

            buffer = new byte[bufferSize];
            string pending = message.Substring(indexEnd + 1);
            Array.Copy(Encoding.UTF8.GetBytes(pending), 0, buffer, 0, pending.Length);
            pendingBytes = pending.Length;
            return new Message(message);
        }

        public void Write(Message message)
        {
            stream.Write(message.ToByteArray());
        }

        public void Close()
        {
            stream.Dispose();
            stream.Close();
        }
    }
}
