using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using Components;

namespace Server
{
    public class ChatRoom
    {
        private readonly object lockOperations = new object();
        private List<User> users = new List<User>();

        public void Join(User user)
        {
            lock (lockOperations)
                users.Add(user);

            new Thread(() => AddUserToConversation(user)).Start();
        }

        private void AddUserToConversation(User user)
        {
            try
            {
                string firstRead = user.Receive().ToString();
                user.Name = firstRead.Substring(0, firstRead.IndexOf('\0'));
                HandleMessage(new Message($"{user.Name} has connected.\0"));
                while (true)
                {
                    Message m = user.Receive();
                    HandleMessage(m);
                }
            }
            catch (Exception)
            {
                Remove(user);
                return;
            }
        }

        private void HandleMessage(Message m)
        {
            Console.WriteLine(m);
            SendMessageToAllClients(m);
        }


        public void Remove(User user)
        {
            lock (lockOperations)
                users.Remove(user);

            user.Close();
            Console.WriteLine($"{user.Name} has disconnected...");
            SendMessageToAllClients(new Message($"{user.Name} has disconnected..."));
        }

        public void SendMessageToAllClients(Message message)
        {
            lock(lockOperations)
            foreach (var client in users)
            {
                client.Send(message);
            }
        }
    }
}
