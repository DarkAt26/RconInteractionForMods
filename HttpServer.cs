using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace HttpRequestRcon
{
    class HttpServer
    {
        public class Config
        {
            public string? ip { get; set; }
            public int? port { get; set; }
            public string? authkey { get; set; }
            public bool? acceptNonLocalRequests { get; set; }
        }

        public class RconCommand
        {
            public string? command { get; set; }
            public string?[] arguments { get; set; } = { };
        }

        public HttpListener? listener;
        public Config? config;
        public int requestCount = 0;

        public void PrintRequestDetails(HttpListenerRequest req)
        {
            Console.WriteLine("Request #{0} {1}", ++requestCount, req.HttpMethod);
            return;
            //Console.WriteLine(req.Headers.ToString());
            Console.WriteLine(req.UserAgent);
            Console.WriteLine(req.UserHostAddress);
            Console.WriteLine(req.UserHostName);
            Console.WriteLine(req.UserLanguages);

            return;
            if (req.RawUrl == "/favicon.ico") { return; }
            // Print out some info about the request
            Console.WriteLine("Request #{0}", ++requestCount);
            Console.WriteLine(req.HttpMethod + " request to " + req.Url);
            Console.WriteLine(req.HttpMethod);
            Console.WriteLine(req.UserAgent);
            Console.WriteLine(req.IsLocal == true);
            Console.WriteLine(req.RawUrl);
            Console.WriteLine();
        }

        public string? GetRequestBody(HttpListenerRequest req)
        {
            if (!req.HasEntityBody) { return null; }

            return new StreamReader(req.InputStream, req.ContentEncoding).ReadToEnd();
        }

        public async Task HandleIncomingRequests()
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                Console.WriteLine();
                // Will wait here until we hear from a connection
                HttpListenerContext listnerContext = await listener!.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = listnerContext.Request;
                HttpListenerResponse resp = listnerContext.Response;

                PrintRequestDetails(req);

                

                //Skip Requests which dont target httpRcon
                if (req.RawUrl!.StartsWith("/httpRcon") == false)
                {
                    Console.WriteLine("Skipped, not targeting httpRcon");
                    continue; 
                }

                //Skip NonLocal Requests if not allowed
                if (config!.acceptNonLocalRequests == false && req.IsLocal == false)
                {
                    Console.WriteLine("Skipped, NonLocal request, which is disabled");
                    continue;
                }

                //Skip Unauthorized Requests
                if (config!.authkey != req.Headers.Get("Authorization"))
                {
                    Console.WriteLine("Skipped, Unauthorized");
                    continue;
                }

                //extract RconCommand from RequestBody
                RconCommand rconCommand = null!;
                try
                {
                    rconCommand = JsonSerializer.Deserialize<RconCommand>(GetRequestBody(req)!)!;
                }
                catch
                {
                    Console.WriteLine("RconCommand is Invalid");
                    continue;
                }

                //POST Requests
                if (req.HttpMethod == "POST")
                {
                    switch (rconCommand.command)
                    {
                        case "SwitchMap":
                            Rcon.SwitchMap(rconCommand.arguments[0], rconCommand.arguments[1]);
                            break;

                        default:
                            Console.WriteLine("Unknown request");
                            break;
                    }


                }

                //GET Requests
                else if (req.HttpMethod == "GET")
                {

                }

                
                

                

                // Write the response info
                string disableSubmit = !runServer ? "{\"disabled\": true}" : "{\"disabled\": false}";
                byte[] data = Encoding.UTF8.GetBytes(disableSubmit);
                resp.ContentType = "application/json";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }

        public bool LoadConfig()
        {
            try
            {
                //Try to load config string from file
                config = JsonSerializer.Deserialize<Config>(File.ReadAllText("config.json"));
                Console.WriteLine("Config Loaded");

                //Throw error if config data contains incorrect data
                if (config!.ip == null || config!.ip == "" || config.port == null || config.authkey == null || config.authkey == "")
                {
                    Console.WriteLine("Error: Invalid Config Data Found");
                    return false;
                }

                Console.WriteLine("IP: " + config.ip);
                Console.WriteLine("PORT: " + config.port);
                Console.WriteLine("KEY: " + config.authkey);

                return true;
            }
            catch
            {
                Console.WriteLine("Error: Couldnt Read Config");
                return false;
            }
        }


        public async void Start()
        {
            //Load config & dont start if config not valid
            if (!LoadConfig()) { Console.WriteLine("Error: Config Not Valid"); return; };




            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add("http://" + config.ip + ":" + config.port + "/");
            listener.Start();
            Console.WriteLine("Listening for requests on {0}", "http://" + config.ip + ":" + config.port + "/");

            // Handle requests
            Task listenTask = HandleIncomingRequests();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();

        }
    }
}
