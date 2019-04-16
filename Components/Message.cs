using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
    public class Message
    {
        private string message = "";
        public int Length { get; private set; }
        public bool IsEmpty { get => string.IsNullOrWhiteSpace(message); }

        public Message(string message)
        {
            this.message = message;
        }

        public Message(byte[] message)
         : this(Encoding.UTF8.GetString(message))
        {
        }

        public static Message operator +(Message text1,string text2)
        {
            return new Message(text1.ToString()+text2);
        }

        public int IndexOf(string pattern) => message.IndexOf(pattern);

        public int IndexOf(char character) => message.IndexOf(character);

        public Message Concatenate(string message) => new Message(this.message + message);

        public Message Concatenate(Message message) => new Message(this.message + message.ToString());

        public Message Concatenate(byte[] message) => new Message(this.message + Encoding.UTF8.GetString(message));

        public byte[] ToByteArray() => Encoding.UTF8.GetBytes(message);

        public override string ToString() => message;
    }
}
