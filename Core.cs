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
            HttpServer server = new HttpServer();
            server.Start();

            Console.WriteLine("FUCK");

            //HttpClient client = new HttpClient();
            //var json = client.GetStringAsync("https://raw.githubusercontent.com/DarkAt26/PavlovVR-RandomBlueprintCollection/refs/heads/main/ModifyItems");
            //Console.WriteLine(json.Result);
        }
    }
}
