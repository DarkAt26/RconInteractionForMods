﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RconInteractionForMods
{
    public static class Config
    {
        public static Cfg cfg = new Cfg();
        public static Dictionary<string, string[]> cmds = new Dictionary<string, string[]>();

        public static void Load(string configPath)
        {
            //Check if config file exists and if not create a new one with default params and stop
            if (!File.Exists(configPath))
            {
                //create default config string
                cfg.HttpRequest_Ip = GetLocalIPAddress();
                string jsonString = JsonConvert.SerializeObject(cfg, Formatting.Indented);
                
                File.WriteAllText(configPath, jsonString);
                
                return;
            }

            //read configString from configUrl
            string configString = System.IO.File.ReadAllText(configPath);

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
            Console.WriteLine("---------------------------Config---------------------------");
            Console.WriteLine("HttpRequest_Ip: " + cfg.HttpRequest_Ip);
            Console.WriteLine("HttpRequest_Port: " + cfg.HttpRequest_Port);
            Console.WriteLine("HttpRequest_AuthKey: " + cfg.HttpRequest_AuthKey);
            Console.WriteLine("HttpRequest_ViewKey: " + cfg.HttpRequest_ViewKey);
            Console.WriteLine("HttpRequest_AcceptNonLocalRequests: " + cfg.HttpRequest_AcceptNonLocalRequests);
            Console.WriteLine();
            Console.WriteLine("Rcon_Ip: " + cfg.Rcon_Ip);
            Console.WriteLine("Rcon_Port: " + cfg.Rcon_Port);
            Console.WriteLine("Rcon_Password: " + cfg.Rcon_Password);
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine();
        }

        public static void LoadCmdCfg(string cmdConfigPath)
        {
            //Check if cmdConfig file exists and if not create a new one with default params and stop
            if (!File.Exists(cmdConfigPath))
            {
                //create default cmdConfig string
                string[] commandExamples = { "RconCmd1", "RconCmd2" };
                cmds.Add("UGC0000000", commandExamples );
                string jsonString = JsonConvert.SerializeObject(cmds, Formatting.Indented);

                File.WriteAllText(cmdConfigPath, jsonString);

                return;
            }

            //read cmdConfigString from cmdConfigUrl
            string cmdConfigString = System.IO.File.ReadAllText(cmdConfigPath);

            //stop if config string is empty
            if (string.IsNullOrEmpty(cmdConfigString))
            {
                return;
            }

            //try to Deserialize the cmdConfig string into the config var
            try
            {
                cmds = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(cmdConfigString)!;
            }
            catch
            {

            }
        }

        public static string GetLocalIPAddress()
        {
            try
            {
                // Get all host entries for the current machine
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    // Check for IPv4 and skip loopback addresses
                    if (ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
                    {
                        return ip.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return "";
        }

        public static void PrintCmdCfg()
        {
            Console.WriteLine("-------------------------CmdConfig--------------------------");

            foreach (KeyValuePair<string, string[]> kvp in cmds)
            {
                string array = "";
                foreach (string s in kvp.Value)
                {
                    array += s + ", ";
                }
                array = array.Remove(array.Length - 2, 2);
                Console.WriteLine(kvp.Key + ": " + array);
            }

            Console.WriteLine("------------------------------------------------------------");
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
