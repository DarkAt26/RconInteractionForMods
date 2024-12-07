using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RconInteractionForMods
{
    public static class Config
    {
        public static string configUrl = "rifm_config.json";
        public static Cfg cfg = new Cfg();

        public static void Load()
        {
            //Check if config file exists and if not create a new one with default params and stop
            if (!File.Exists(configUrl))
            {
                //create default config string
                string jsonString = JsonConvert.SerializeObject(cfg, Formatting.Indented);
                
                File.WriteAllText(configUrl, jsonString);
                
                return;
            }

            //read configString from configUrl
            string configString = System.IO.File.ReadAllText(configUrl);

            //stop if config string is empty
            if (string.IsNullOrEmpty(configString))
            {
                return;
            }

            //try to Deserialize the config string into the config var
            try
            {
                cfg = System.Text.Json.JsonSerializer.Deserialize<Cfg>(configString)!;
            }
            catch
            {

            }
        }

        public static void Print()
        {
            Console.WriteLine("Config:");
            Console.WriteLine("HttpRequest_Ip: " + cfg.HttpRequest_Ip);
            Console.WriteLine("HttpRequest_Port: " + cfg.HttpRequest_Port);
            Console.WriteLine("HttpRequest_AuthKey: " + cfg.HttpRequest_AuthKey);
            Console.WriteLine("HttpRequest_ViewKey: " + cfg.HttpRequest_ViewKey);
            Console.WriteLine("HttpRequest_AcceptNonLocalRequests: " + cfg.HttpRequest_AcceptNonLocalRequests);
            Console.WriteLine();
            Console.WriteLine("Rcon_Ip: " + cfg.Rcon_Ip);
            Console.WriteLine("Rcon_Port: " + cfg.Rcon_Port);
            Console.WriteLine("Rcon_Password: " + cfg.Rcon_Password);
            Console.WriteLine();
        }


        public class Cfg //DataHolder
        {
            public string HttpRequest_Ip { get; set; } = "";
            public int HttpRequest_Port { get; set; } = 8000;
            public string HttpRequest_AuthKey { get; set; } = "";
            public string HttpRequest_ViewKey { get; set; } = "";
            public bool HttpRequest_AcceptNonLocalRequests { get; set; } = false;

            public string Rcon_Ip { get; set; } = "";
            public int Rcon_Port { get; set; } = 8000;
            public string Rcon_Password { get; set; } = "";
        }
    }
}
