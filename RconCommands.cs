﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RconInteractionForMods
{
    public static class RconCommands
    {
        public static string SwitchMap(string? map, string? gameMode)
        {
            //cancel if arguments are null
            if (map == null || map == ""|| gameMode == null || gameMode == "")
            {
                return "Invalid Command Arguments";
            }

            //Console.WriteLine("SwitchMap " + map + " " + gameMode);

            Core.rconServer.rconCommandStack.Add("UpdateServerName DarkAt26-RandomDynamicName-HttpRcon-" + new Random().Next(0, 10000000));

            return "executed";
        }
    }
}
