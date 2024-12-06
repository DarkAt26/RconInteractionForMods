using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequestRcon
{
    class HttpServer
    {
        public HttpListener listener;
        public string ip = "localhost"; 
        public string port = "8000";
        public int requestCount = 0;


        public async Task HandleIncomingConnections()
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                Console.WriteLine("Request #{0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                // POST Requests
                if (req.HttpMethod == "POST" && req.Url.AbsolutePath.StartsWith("/httpRcon"))
                {
                    Console.WriteLine(req.Url.AbsolutePath);
                    Console.WriteLine(req.Url.AbsolutePath.Remove(0, 9));

                    switch (req.Url.AbsolutePath.Remove(0, 9))
                    {
                        case "/shutdown":
                            Console.WriteLine("Shutdown requested");
                            runServer = false;
                            break;
                        case "/restart":
                            Console.WriteLine("Restart requested");
                            break;
                        default:
                            Console.WriteLine("Unknown request");
                            break;
                    }
                }

                // GET Requests
                else if (req.HttpMethod == "GET")
                {

                }

                // Other Requests
                else
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

        public string GetURL()
        {
            return "http://" + ip + ":" + port + "/";
        }

        public async void Start()
        {
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(GetURL());
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", GetURL());

            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();

            //win?
        }
    }
}
