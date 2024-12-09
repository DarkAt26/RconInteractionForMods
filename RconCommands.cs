using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RconInteractionForMods
{
    public partial class HttpServer
    {
        public async Task<string> RunRconCommand(RconCommand rconCommand)
        {

            switch (rconCommand.Command)
            {
                case "SwitchMap":
                    return await Core.rconClient.ExecuteCommandAsync("SwitchMap " + rconCommand.Arguments[0] + " " + rconCommand.Arguments[1]);

                case "UpdateServerName":
                    return await Core.rconClient.ExecuteCommandAsync("UpdateServerName " + rconCommand.Arguments[0]);

                default:
                    Console.WriteLine("Unknown request");
                    return ToJsonArray("UnknownRequest");
            }
        }
    }
}
