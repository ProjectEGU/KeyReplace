using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KeyReplace
{
    public static class Log
    {
        public static void LogMessage(string message)
        {
            File.AppendAllLines("debug.log", new string[] { message });
        }
        
    }
}
