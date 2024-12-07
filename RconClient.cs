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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RconInteractionForMods
{
    public class RconClient
    {
        public StreamWriter? commandWriter;

        public List<string> rconCommandStack = new List<string>() { "UpdateServerName DarkAt26-RandomDynamicName-" + new Random().Next(0, 10000000) };

        public async void Start()
        {
            Log("Starting");

            //Create Rcon Connection
            ConnectToRcon();
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

            Log("Connected.");

            while (true)
            {

                if (rconCommandStack.Count != 0)
                {
                    await ExecuteCommandAsync(rconCommandStack[0]);
                    rconCommandStack.RemoveAt(0);
                    
                    //var a = reader.ReadToEndAsync();
                    //Console.WriteLine(a);
                    
                    await commandWriter.FlushAsync();
                    await Task.Delay(200);
                }
            }
        }

        public async Task ExecuteCommandAsync(string command)
        {
            await commandWriter.WriteAsync(command);
            //await commandWriter.FlushAsync();
            Log("Executed Command: " + command);
        }

        public async void Send(Socket client, string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(messageBytes, SocketFlags.None);
            Console.WriteLine(message);
        }

        public async Task<string> Receive(Socket client)
        {
            byte[] buffer = new byte[1_024];
            int received = await client.ReceiveAsync(buffer, SocketFlags.None);
            string response = Encoding.UTF8.GetString(buffer, 0, received);
            Console.WriteLine(response);
            return response;
        }


        public void Log(string data)
        {
            Console.WriteLine("RconServer: " + data);
        }
    }
}
