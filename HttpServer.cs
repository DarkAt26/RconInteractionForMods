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
            public string? key { get; set; }
            public bool? acceptNonLocalRequests { get; set; }
        }

        public class HttpRequest
        {
            public string? url { get; set; }
            public string? method { get; set; }
            public string? userAgent { get; set; }
            public string? local { get; set; }
        }

        public HttpListener? listener;
        public Config? config;
        public int requestCount = 0;

        public void PrintRequestDetails(HttpListenerRequest req)
        {
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

        public string? GetArgument(string argument, string[] arguments)
        {
            //Loop through all Arguments
            for (int i = 0; i < arguments.Length; i++)
            {
                //Check if argument from loop starts with argument
                if (arguments[i].StartsWith(argument))
                {
                    //return argument value
                    return arguments[i].Remove(0, 1 + argument.Length);
                }
            }
            return null;
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

                if (config.acceptNonLocalRequests !=)

                //Skip Requests which dont target httpRcon & are not GET
                if (!req.RawUrl!.StartsWith("/httpRcon") && req.HttpMethod == "GET") { continue; }

                //extract command and arguments from RawUrl
                string relativeUrl = req.RawUrl.Remove(0, 9).ToLower();
                string command;
                string[] arguments = { };

                //assign command and arguments to their variables
                if (relativeUrl.Contains('?'))
                {
                    command = relativeUrl.Split('?')[0].Remove(0, 1);
                    arguments = relativeUrl.Split('?')[1].Split('&');
                }
                else
                {
                    command = relativeUrl.Remove(0, 1).ToLower();
                }

                switch (command)
                {
                    case "switchmap":
                        Rcon.SwitchMap(GetArgument("mapid", arguments), GetArgument("gamemode", arguments));
                        break;

                    case "shutdown":
                        Console.WriteLine("Shutdown requested");
                        runServer = false;
                        break;

                    default:
                        Console.WriteLine("Unknown request");
                        break;
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
                if (config!.ip == null || config!.ip == "" || config.port == null || config.key == null || config.key == "")
                {
                    Console.WriteLine("Error: Invalid Config Data Found");
                    return false;
                }

                Console.WriteLine("IP: " + config.ip);
                Console.WriteLine("PORT: " + config.port);
                Console.WriteLine("KEY: " + config.key);

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
