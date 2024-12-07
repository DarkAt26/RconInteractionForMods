using RconInteractionForMods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace RconInteractionForMods
{
     public static class Core
     {
        public static HttpServer httpServer = new HttpServer();
        public static RconServer rconServer = new RconServer();
        public static void Main()
        {
            //Load & Print Config
            Config.Load();
            Config.Print();

            //Create and start servers
            //HttpServer? httpServer = new HttpServer();
            //RconServer? rconServer = new RconServer();
            
            //httpServer.Start();
            //rconServer.Start();


            Testing testing = new Testing();
            testing.Testing2();






            //AsyncTest asyncTest = new AsyncTest();
            //AsyncTest2 asyncTest2 = new AsyncTest2();
            //asyncTest.Start();
            //asyncTest2.Start();


            //AsyncTest asyncTest = new AsyncTest();
            //asyncTest.MyMethodAsync(5);
            //asyncTest.MyMethodAsync(2);
            //asyncTest.MyMethodAsync(20);

            Console.WriteLine("InfinityLoop");
            while (true) { }

            //HttpClient client = new HttpClient();
            //var json = client.GetStringAsync("https://raw.githubusercontent.com/DarkAt26/PavlovVR-RandomBlueprintCollection/refs/heads/main/ModifyItems");
            //Console.WriteLine(json.Result);
        }

    }
    class AsyncTest
    {
        public async void Start()
        {
            Console.WriteLine("Starting1");
            Task listenTask = Loopy();
            //listenTask.GetAwaiter().GetResult();
        }

        public async Task Loopy()
        {
            while (true)
            {
                await Task.Delay(5000);
                Console.WriteLine("Loopy1");
            }
        }

        public async Task MyMethodAsync(int i)
        {
            Task<int> longRunningTask = LongRunningOperationAsync(i);
            // independent work which doesn't need the result of LongRunningOperationAsync can be done here

            //and now we call await on the task 
            int result = await longRunningTask;
            //use the result 
            Console.WriteLine(result);
        }

        public async Task<int> LongRunningOperationAsync(int i) // assume we return an int from this long running operation 
        {
            await Task.Delay(i * 1000); // 1 second delay
            return 1;
        }
    }

    class AsyncTest2
    {
        public async void Start()
        {
            Console.WriteLine("Starting2");
        }

        public async Task MyMethodAsync(int i)
        {
            Task<int> longRunningTask = LongRunningOperationAsync(i);
            // independent work which doesn't need the result of LongRunningOperationAsync can be done here

            //and now we call await on the task 
            int result = await longRunningTask;
            //use the result 
            Console.WriteLine(result);
        }

        public async Task<int> LongRunningOperationAsync(int i) // assume we return an int from this long running operation 
        {
            await Task.Delay(i * 1000); // 1 second delay
            return 1;

        }
    }

    public class Testing
    {
        public async void Testing2()
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            await client.ConnectAsync("45.77.65.193", 9100);

            if (await Receive(client) != "Password: ")
            {
                Console.WriteLine("ERROR ERROR ERROR");
                return;
            }

            Send(client, Convert.ToHexString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Config.cfg.Rcon_Password))).ToLower());

            if (await Receive(client) != "Authenticated=1\r\n")
            {
                Console.WriteLine("ERROR ERROR ERROR");
                return;
            }

            Send(client, "UpdateServerName DarkAt26-RandomDynamicNameLLLLL-" + new Random().Next(0, 10000000));

            await Receive(client);
            await Receive(client);

            while (true)
            {
                await Task.Delay(2000);
                //await using NetworkStream stream = new(client);
                //using StreamReader reader = new(stream);
                //await using StreamWriter writer = new(stream);
                //await writer.WriteAsync(Convert.ToHexString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Config.cfg.Rcon_Password))).ToLower());
                //await writer.FlushAsync();




                //var message = "Hi friends 👋!<|EOM|>";
                //var messageBytes = Encoding.UTF8.GetBytes(message);
                //await client.SendAsync(messageBytes, SocketFlags.None);
                //Console.WriteLine($"Socket client sent message: \"{message}\"");

                // Receive ack.
                //var buffer = new byte[1_024];
                //var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                //var response = Encoding.UTF8.GetString(buffer, 0, received);
                //Console.WriteLine(response);
            }
        }

        public async void Send(Socket client, string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(messageBytes, SocketFlags.None);
            Console.WriteLine(message);
        }

        public async Task<string> Receive(Socket client)
        {
            var buffer = new byte[1_024];
            var received = await client.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);
            Console.WriteLine(response);
            return response;
        }
    }
}
