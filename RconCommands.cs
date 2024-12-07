using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RconInteractionForMods
{
    public class RconCommands
    {
        public async Task<string> SwitchMap(string? map, string? gameMode)
        {
            //cancel if arguments are null
            if (map == null || map == ""|| gameMode == null || gameMode == "")
            {
                return "Invalid Command Arguments";
            }

            return await Core.rconClient.ExecuteCommandAsync("SwitchMap " + map + " " + gameMode);
        }

        public async Task<string> RandomShit(string? map, string? gameMode)
        {
            //cancel if arguments are null
            if (map == null || map == "" || gameMode == null || gameMode == "")
            {
                return "Invalid Command Arguments";
            }

            //Console.WriteLine("SwitchMap " + map + " " + gameMode);

            await Core.rconClient.ExecuteCommandAsync("UpdateServerName DarkAt26-RandomDynamicNameLLLLL-" + new Random().Next(0, 10000000));

            return "executed";
        }
    }
}
