using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequestRcon
{
    public static class Rcon
    {
        public static string SwitchMap(string? map, string? gameMode)
        {
            //cancel if arguments are null
            if (map == null || map == ""|| gameMode == null || gameMode == "")
            {
                return "null";
            }

            Console.WriteLine("SwitchMap " + map + " " + gameMode);

            return "null";
        }
    }
}
