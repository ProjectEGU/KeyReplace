using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeyReplace
{
    //http://blog.csdn.net/cloudhsu/article/details/5773043
    //it didnt work so i changed up the code.
    public class AccurateTimer
    {
        static System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        public static void AccurateSleep(int ms)
        {
            sw.Restart();
            while(sw.ElapsedMilliseconds < ms)
            {
                Thread.SpinWait(100);
               
            }
        }
    }
}
