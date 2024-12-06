using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequestRcon
{
    public static class Rcon
    {
        public static string SwitchMap(string? mapId, string? gameMode)
        {
            //cancel if arguments are null
            if (mapId == null || gameMode == null) { return "null"; }

            Console.WriteLine("SwitchMap " + mapId + " " + gameMode);

            return "null";
        }
    }
}
