﻿using RconInteractionForMods;
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
        public static RconClient rconClient = new RconClient();
        public static void Main()
        {
            //Load & Print Config
            Config.Load("rifm_config.json");
            Config.Print();
            
            //start HttpServer and RconClient
            httpServer.Start();
            rconClient.Start();

            //InfinityLoop to keep running
            while (true) { }
        }
    }
}
