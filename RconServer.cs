using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;

namespace RconInteractionForMods
{
    public class RconServer
    {
        public StreamWriter? commandWriter;

        public async void Start()
        {
            Console.WriteLine("Starting RconServer...");
            // Handle requests
            Task listenTask = ConnectToRcon();
            //listenTask.GetAwaiter().GetResult();


            //Console.WriteLine("Starting");
            //await ConnectToRcon();
            Console.WriteLine("Stopped");
        }

        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public async Task ConnectToRcon()
        {
            await socket.ConnectAsync(Config.cfg.Rcon_Ip, Config.cfg.Rcon_Port);
            await using NetworkStream stream = new(socket);
            using StreamReader reader = new(stream);
            await using StreamWriter writer = new(stream);
            await writer.WriteAsync(Convert.ToHexString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Config.cfg.Rcon_Password))).ToLower());
            await writer.FlushAsync();
            commandWriter = writer;

            Console.WriteLine("Connected");

            while (true)
            {
                Thread.Sleep(2000);
                //Task.Delay(2000);
                Console.WriteLine("Waited");
                await ExecuteCommandAsync("UpdateServerName DarkAt26-RandomDynamicName-" + new Random().Next(0,10000000));
            }
        }

        public async Task ExecuteCommandAsync(string command)
        {
            await commandWriter.WriteAsync(command);
            await commandWriter.FlushAsync();
            Console.WriteLine(command);
        }
    }
}
