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

namespace HttpRequestRcon
{
    class WebSocketServer
    {
        public void Start()
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);

            server.Start();
            Console.WriteLine("Server has started on 127.0.0.1:80.{0}Waiting for a connection…", Environment.NewLine);

            TcpClient client = server.AcceptTcpClient();

            Console.WriteLine("A client connected.");
        }
    }

    public class RconInteractionForMods
    {
        public Config? config;
        public StreamWriter? commandWriter;




        public async void Start()
        {
            Console.WriteLine("Start?");

            config = JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText("rifm_config.json"));

            Console.WriteLine(config.IP);
            Console.WriteLine(config.Port);
            Console.WriteLine(config.Password);

            Console.WriteLine("Starting");

            // Handle requests
            Task listenTask = ConnectToRcon();
            listenTask.GetAwaiter().GetResult();


            //Console.WriteLine("Starting");
            //await ConnectToRcon();
            Console.WriteLine("Stopped");
        }

        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public async Task ConnectToRcon()
        {
            await socket.ConnectAsync(config.IP, config.Port);
            await using NetworkStream stream = new(socket);
            using StreamReader reader = new(stream);
            await using StreamWriter writer = new(stream);
            await writer.WriteAsync(Convert.ToHexString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(config.Password))).ToLower());
            await writer.FlushAsync();
            commandWriter = writer;

            Console.WriteLine("Connected");

            while (true)
            {
                Thread.Sleep(2000);
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

        public class Config
        {
            public string IP { get; set; } = "";
            public int Port { get; set; } = 0;
            public string Password { get; set; } = "";
        }
    }
}
