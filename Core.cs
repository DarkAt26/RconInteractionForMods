using RconInteractionForMods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HttpRequestRcon
{
    class Core
    {
        static void Main()
        {
            //File.Create("config.txt");

            HttpServer server = new HttpServer();
            WebSocketServer ws = new WebSocketServer();
            RconInteractionForModsA rconInteractionForMods = new RconInteractionForModsA();
            //rconInteractionForMods.Start();
            //ws.Start();
            //server.Start();

            RconInteractionForMods.Config.Load();

            RconInteractionForMods.Config.Print();
            
            Console.WriteLine("FUCK");

            AsyncTest asyncTest = new AsyncTest();
            asyncTest.MyMethodAsync(5);
            asyncTest.MyMethodAsync(2);
            asyncTest.MyMethodAsync(20);

            while (true) { }

            //HttpClient client = new HttpClient();
            //var json = client.GetStringAsync("https://raw.githubusercontent.com/DarkAt26/PavlovVR-RandomBlueprintCollection/refs/heads/main/ModifyItems");
            //Console.WriteLine(json.Result);
        }

    }
    class AsyncTest
    {
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
            await Task.Delay(i*1000); // 1 second delay
            return 1;
        }
    }
}
