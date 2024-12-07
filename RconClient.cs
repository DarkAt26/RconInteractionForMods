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
        public void Start()
        {
            Log("Starting");

            //Create Rcon Connection
            _ = RconConnection();
        }

        private Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public async Task RconConnection()
        {
            //Create connection
            await client.ConnectAsync(Config.cfg.Rcon_Ip, Config.cfg.Rcon_Port);

            //Check if response is "Password: "
            if (await Receive() != "Password: ")
            {
                Log("ERROR: Unexpected Process");
                return;
            }

            //respond with Password
            Send(Convert.ToHexString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Config.cfg.Rcon_Password))).ToLower());

            //check if response is Authenticated
            if (await Receive() != "Authenticated=1\r\n")
            {
                Log("ERROR: Unauthorized");
                return;
            }

            Log("Connected.");

            //InfinityLoop to keep the connection up
            //RemoteHost closes connection after 3 to 5min???
            while (true)
            {
                await Task.Delay(150000);
                Send("KeepAlive");
            }
        }

        public async Task<string> ExecuteCommandAsync(string command)
        {
            Log("Execute Command: " + command);
            
            Send(command);
            
            return await Receive();
        }

        public async void Send(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(messageBytes, SocketFlags.None);
            Console.WriteLine(message);
        }

        public async Task<string> Receive()
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
