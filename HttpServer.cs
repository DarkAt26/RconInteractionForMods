using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static HttpRequestRcon.HttpServer;


namespace HttpRequestRcon
{
    class HttpServer
    {
        public class Config
        {
            public string Ip { get; set; } = "";
            public int Port { get; set; } = 8000;
            public string AuthKey { get; set; } = "";
            public string ViewKey { get; set; } = "";
            public bool AcceptNonLocalRequests { get; set; } = false;
        }
        public class RconCommand
        {
            public string? Command { get; set; }
            public string[] Arguments { get; set; } = { };
        }
        public class Logs
        {
            public int RequestCount { get; set; } = 0;
            public Request[] UnauthorizedRequests { get; set; } = { };
        }
        public class Request
        {
            public string? Method { get; set; }
            public string? UserAgent { get; set; }
            public string? Url { get; set; }
            public bool? IsLocal { get; set; }
            public string? Headers { get; set; }
            public string? ContentBody { get; set; }
        }

        public HttpListener? listener;
        public Config? config;
        public Logs? logs;

        public void PrintRequestDetails(HttpListenerRequest req)
        {
            if (req.RawUrl == "/favicon.ico") { return; }
            Console.WriteLine("Request #{0} {1}", ++logs!.RequestCount, req.HttpMethod);
            Request request = new Request();
            request.Method = req.HttpMethod;
            request.Url = req.Url!.ToString().Replace("?authkey=" + config!.ViewKey, "");
            request.UserAgent = req.UserAgent;
            request.IsLocal = req.IsLocal;
            request.Headers = req.Headers.ToString();
            
            
            
            Request[] requestPre = { request };
            logs.UnauthorizedRequests = requestPre.Concat(logs.UnauthorizedRequests).ToArray();
        }

        public RconCommand GetRconCommand(HttpListenerRequest req)
        {
            if (!req.HasEntityBody) { return new RconCommand(); }

            string requestBody = new StreamReader(req.InputStream, req.ContentEncoding).ReadToEnd();
            Console.WriteLine(requestBody);
            try
            {
                return JsonSerializer.Deserialize<RconCommand>(requestBody)!;
            }
            catch
            {
                Console.WriteLine("RconCommand is Invalid");
                return new RconCommand();
            }
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

                string responseContent = "This message shouldnt be seen at any point. Something went wrong.";



                

                //Skip Requests which dont target httpRcon
                if (req.RawUrl!.StartsWith("/httpRcon") == false)
                {
                    resp.Close();
                    continue;
                }

                PrintRequestDetails(req);

                //Skip Unauthorized Requests
                if (!IsAuthorized(req))
                {
                    Console.WriteLine("Skipped, Unauthorized");
                    RespondToRequest(resp, ToJsonArray("Unauthorized"));
                    continue;
                }



                RconCommand rconCommand = GetRconCommand(req);

                

                //Skip NonLocal Requests if not allowed
                if ((config!.AcceptNonLocalRequests == false && req.IsLocal == false) && req.HttpMethod != "GET")
                {
                    Console.WriteLine("Skipped, Unauthorized-NL");
                    RespondToRequest(resp, ToJsonArray("Unauthorized-NL"));
                    continue;
                }

                //POST Requests
                if (req.HttpMethod == "POST")
                {
                    switch (rconCommand.Command)
                    {
                        case "SwitchMap":
                            responseContent = Rcon.SwitchMap(rconCommand.Arguments[0], rconCommand.Arguments[1]);
                            break;

                        default:
                            Console.WriteLine("Unknown request");
                            responseContent = ToJsonArray("UnknownRequest");
                            break;
                    }
                }

                //GET Requests
                else if (req.HttpMethod == "GET")
                {
                    responseContent = JsonSerializer.Serialize(logs);
                }




                RespondToRequest(resp, responseContent);
            }
        }
        public string ToJsonArray(string str, bool isJsonString = false)
        {
            return isJsonString ? "[" + str + "]" : "[\"" + str + "\"]";
        }
        public bool IsAuthorized(HttpListenerRequest req)
        {
            //POST Auth
            if ( (req.HttpMethod == "POST") && (config!.AuthKey != req.Headers.Get("Authorization")) )
            {
                return false;
            }

            //GET Auth
            else if ( (req.HttpMethod == "GET") && ( (config!.AuthKey != req.Headers.Get("Authorization")) && !(req.RawUrl.EndsWith("?authkey=" + config.ViewKey)) ) )
            {
                return false;
            }

            return true;
        }

        public async void RespondToRequest(HttpListenerResponse httpResponse, string content)
        {
            // Write the response info
            byte[] data = Encoding.UTF8.GetBytes(content);
            httpResponse.ContentType = "application/json";
            httpResponse.ContentEncoding = Encoding.UTF8;
            httpResponse.ContentLength64 = data.LongLength;

            // Write out to the response stream (asynchronously), then close it
            await httpResponse.OutputStream.WriteAsync(data, 0, data.Length);
            httpResponse.Close();
        }

        public bool LoadConfig()
        {
            try
            {
                //Try to load config string from file
                config = JsonSerializer.Deserialize<Config>(File.ReadAllText("config.json"));
                Console.WriteLine("Config Loaded");

                //Throw error if config data contains incorrect data
                if (config!.Ip == null || config!.Ip == "" || config.Port == null || config.AuthKey == null || config.AuthKey == "")
                {
                    Console.WriteLine("Error: Invalid Config Data Found");
                    return false;
                }

                Console.WriteLine("IP: " + config.Ip);
                Console.WriteLine("PORT: " + config.Port);
                Console.WriteLine("KEY: " + config.AuthKey);

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
            logs = new Logs();

            //Load config & dont start if config not valid
            if (!LoadConfig()) { Console.WriteLine("Error: Config Not Valid"); return; };

            


            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add("http://" + config!.Ip + ":" + config.Port + "/");
            listener.Start();
            Console.WriteLine("Listening for requests on {0}", "http://" + config.Ip + ":" + config.Port + "/");

            // Handle requests
            Task listenTask = HandleIncomingRequests();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();

        }
    }
}
