﻿using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();
            server.Start();
            Console.WriteLine("The server has started.");
            server.AcceptClient();
        }
    }
}
