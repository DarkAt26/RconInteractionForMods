using RconInteractionForMods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RconInteractionForMods
{
    class Core
    {
        static void Main()
        {
            //Load & Print Config
            Config.Load();
            Config.Print();

            //Create and start servers
            HttpServer httpServer = new HttpServer();
            RconServer rconServer = new RconServer();
            
            httpServer.Start();
            rconServer.Start();




            Console.WriteLine("FUCK");


            AsyncTest asyncTest = new AsyncTest();
            AsyncTest2 asyncTest2 = new AsyncTest2();
            //asyncTest.Start();
            //asyncTest2.Start();


            //AsyncTest asyncTest = new AsyncTest();
            //asyncTest.MyMethodAsync(5);
            //asyncTest.MyMethodAsync(2);
            //asyncTest.MyMethodAsync(20);

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
}
