﻿using Newtonsoft.Json;
using RconInteractionForMods;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using static RconInteractionForMods.HttpServer;
using static System.Net.Mime.MediaTypeNames;


namespace RconInteractionForMods
{
    public partial class HttpServer
    {
        public class RconCommand
        {
            public string UGC { get; set; } = "";
            public string Command { get; set; } = "";
            public string[] Arguments { get; set; } = { };
        }

        public HttpListener? listener;
        public int RequestCount = 0;

        public void PrintRequestDetails(HttpListenerRequest req)
        {
            Log("Request #" + ++RequestCount + " " + req.HttpMethod);
        }

        public RconCommand GetRconCommand(HttpListenerRequest req)
        {
            if (!req.HasEntityBody) { return new RconCommand(); }

            string requestBody = new StreamReader(req.InputStream, req.ContentEncoding).ReadToEnd();
            
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<RconCommand>(requestBody)!;
            }
            catch
            {
                Log("RconCommand is Invalid");
                return new RconCommand();
            }
        }

        public async Task HandleIncomingRequests()
        {
            while (true)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext listnerContext = await listener!.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = listnerContext.Request;
                HttpListenerResponse resp = listnerContext.Response;

                string responseContent = "This message shouldnt be seen at any point. Something went wrong.";

                //Skip Requests which dont target httpRcon
                if (req.RawUrl!.StartsWith("/rifm") == false)
                {
                    resp.Close();
                    continue;
                }

                PrintRequestDetails(req);



                //Skip Unauthorized Requests
                if (!IsAuthorized(req))
                {
                    Log("Skipped, Unauthorized");
                    RespondToRequest(resp, ToJsonArray("Unauthorized"));
                    continue;
                }

                //Skip NonLocal Requests if not allowed
                if ((Config.cfg.HttpRequest_AcceptNonLocalRequests == false && req.IsLocal == false) && req.HttpMethod != "GET")
                {
                    Log("Skipped, Unauthorized-NonLocal");
                    RespondToRequest(resp, ToJsonArray("Unauthorized-NonLocal"));
                    continue;
                }



                RconCommand rconCommand = GetRconCommand(req);

                //Skip commands that are not in the AllRconCommands.cs list
                if ((req.HttpMethod != "GET") && (!rconCommands.Contains(rconCommand.Command)))
                {
                    Log("Skipped, Command not in list.");
                    RespondToRequest(resp, ToJsonArray("Command not in list."));
                    continue;
                }



                //Skip if UGC isnt allowed to execute Command
                if ( (req.HttpMethod != "GET") && (!(Config.cmds["UGC" + rconCommand.UGC].Contains(rconCommand.Command)) || rconCommand.UGC == "" || rconCommand.UGC == null) )
                {
                    Log("Skipped, Unauthorized-UGC");
                    RespondToRequest(resp, ToJsonArray("Unauthorized-UGC"));
                    continue;
                }



                //POST Requests
                if (req.HttpMethod == "POST")
                {
                    Log("UGC" + rconCommand.UGC);
                    responseContent = await RunRconCommand(rconCommand);
                }

                //GET Requests
                else if (req.HttpMethod == "GET")
                {
                    if (req.RawUrl!.StartsWith("/rifm/disconnect"))
                    {
                        //disconnect rcon client
                        Core.rconClient.client.Disconnect(true);
                        responseContent = ToJsonArray("Disconnected RconClient Connection.");
                        _ = Core.rconClient.Send("Check Connection");
                    }
                    else
                    {
                        List<string> log = new List<string>();
                        for (int i = Core.rconClient.log.Count-1; i >= 0; i--)
                        {
                            log.Add(Core.rconClient.log[i]);
                        }

                        responseContent = "{\"commandLogs\":" + JsonConvert.SerializeObject(log) + "}";
                    }
                }

                RespondToRequest(resp, responseContent);
            }
        }

        public async Task<string> RunRconCommand(RconCommand rconCommand)
        {
            string args = "";

            foreach (string arg in rconCommand.Arguments)
            {
                args += arg + " ";
            }
            args = args.Trim();

            if (rconCommand.Command == "Custom")
            {
                foreach (string cmd in rconCommands)
                {
                    if (args.ToLower().StartsWith(cmd.ToLower()))
                    {
                        return "Fuck You!";
                    }
                }

                return await Core.rconClient.ExecuteCommandAsync(args, rconCommand.UGC);
            }

            return await Core.rconClient.ExecuteCommandAsync(rconCommand.Command + " " + args, rconCommand.UGC);
        }

        public string ToJsonArray(string str, bool isJsonString = false)
        {
            return isJsonString ? "[" + str + "]" : "[\"" + str + "\"]";
        }

        public bool IsAuthorized(HttpListenerRequest req)
        {
            //POST Auth
            if ( (req.HttpMethod == "POST") && (Config.cfg.HttpRequest_AuthKey != req.Headers.Get("Authorization")) )
            {
                return false;
            }

            //GET Auth
            else if ( (req.HttpMethod == "GET") &&
                ( (Config.cfg.HttpRequest_AuthKey != req.Headers.Get("Authorization"))
                    && !(req.RawUrl!.EndsWith("?viewkey=" + Config.cfg.HttpRequest_ViewKey))
                    && (Config.cfg.HttpRequest_ViewKey != "")
                ) )
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



        public void Start()
        {
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();

            try
            {
                listener.Prefixes.Add("http://" + Config.cfg.HttpRequest_Ip + ":" + Config.cfg.HttpRequest_Port + "/");
            }
            catch
            {
                Log("Invalid Data: Check Config.");
                Environment.Exit(0);
                return;
            }
                
            listener.Start();
            Log("Started.");

            // Handle requests
            _ =HandleIncomingRequests();
        }

        public void Log(string data)
        {
            Console.WriteLine("HttpServer: " + data.Trim());
        }
    }
}
