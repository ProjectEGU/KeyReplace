using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyReplace
{
    public partial class frmMain
    {

        #region GenHist

        BinaryWriter pressDelay;
        BinaryWriter nextKeyDelay;

        void GenHist()
        {
            //todo - generate csv of some sorta histogram of pressDelay and nextKeyDelay

            int binsize = 5; //ie if binsize (ms) is 5, then bins will be 0-4, 5-9, 10-14, etc etc.
            int bins = 100;

            int[] counters = new int[bins];//automatically fully init to 0s. very lovely.

            BinaryReader pressDelay = new BinaryReader(File.Open("pressDelay", FileMode.Open, FileAccess.Read, FileShare.None));
            BinaryReader nextKeyDelay = new BinaryReader(File.Open("nextKeyDelay", FileMode.Open, FileAccess.Read, FileShare.None));

            StreamWriter csv = new StreamWriter("output" + DateTime.Now.ToFileTime() + ".csv", false, Encoding.ASCII);

            AppendDataToOutputCSV(binsize, bins, counters, pressDelay, csv);
            AppendDataToOutputCSV(binsize, bins, counters, nextKeyDelay, csv);

            pressDelay.Close();
            nextKeyDelay.Close();
            csv.Close();
        }

        private static void AppendDataToOutputCSV(int binsize, int bins, int[] counters, BinaryReader inputfile, StreamWriter output)
        {
            int excess = 0;
            //how to eof - courtesy of https://stackoverflow.com/questions/10942848/c-sharp-checking-for-binary-reader-end-of-file
            while (inputfile.BaseStream.Position != inputfile.BaseStream.Length)
            {
                int bin = inputfile.ReadInt32() / binsize;
                if (bin >= bins)
                {
                    excess++; continue;
                }

                counters[bin] += 1;
            }
            for (int i = 0; i < bins; i++) //counters.Length = bins
            {
                output.WriteLine((i * binsize) + " - " + (i * binsize - 1) + "," +
                              counters[i]);
            }
            output.WriteLine(">= " + (bins * binsize) + "," + excess);
            //
        }
        #endregion
        #region Methods


        int keysDown = 0;


        #region KeyDownLogger - Logging Functionality

        DateTime?[] keytable = new DateTime?[255];
        DateTime? lastPressed = null;
        void _KeyDownLogger(int vk)
        {
            DateTime now = DateTime.Now;

            keysDown += 1;
            label1.Text = "[" + keysDown + "] " + vk + " "
                + Convert.ToString(vk, 2);

            if (_logKPData && vk >= 0 && vk <= 255)
            {

                if (lastPressed.HasValue)
                {
                    nextKeyDelay.Write((int)(now - lastPressed.Value).TotalMilliseconds);
                }
                lastPressed = now;

                if (keytable[vk].HasValue)
                {
                    //technically it shouldnt, but hey.
                    int timeDiff = (int)(now - keytable[vk].Value).TotalMilliseconds;
                    pressDelay.Write((int)(timeDiff));
                }
                keytable[vk] = now;
            }
        }
        void _KeyUpLogger(int vk)
        {
            DateTime now = DateTime.Now;

            keysDown -= 1;
            label1.Text = "[" + keysDown + "] " + parsekeydown(vk) + " "
                + Convert.ToString(vk, 2);

            if (_logKPData && vk >= 0 && vk <= 255)
            {
                if (keytable[vk].HasValue)
                {
                    pressDelay.Write((int)(now - keytable[vk].Value).TotalMilliseconds);
                }
                keytable[vk] = null;
            }
        }
        #endregion

        ////////////// Gross code ends here.

        static Process ListRSWin()
        {
            //bugfix to work with NXT - 2017-01-29
            Process[] tmp = Process.GetProcessesByName("rs2client");
            Process[] tmp2 = Process.GetProcessesByName("JagexLauncher");
            if (tmp.Length + tmp2.Length == 1)
                return tmp.Length == 1 ? tmp[0] : tmp2[0];
            else
                return null;
        }


        #endregion

        #region Keys Table
        //////////////////////////////////////////
        #region The Key Dictionary

        static Dictionary<int, string> downKeys = new Dictionary<int, string>();
        static Dictionary<int, string> upKeys = new Dictionary<int, string>();
        static Dictionary<int, string> shiftKeys = new Dictionary<int, string>();

        static void initKeyDict()
        {
            #region downkeys

            downKeys.Add(8, "[bs]");
            downKeys.Add(9, "[Tab]");

            downKeys.Add(13, "[Enter]");
            downKeys.Add(16, "[Shft]");
            downKeys.Add(17, "[Ctrl]");
            downKeys.Add(19, "[Pause]");

            downKeys.Add(27, "[Esc]");
            downKeys.Add(32, " ");
            downKeys.Add(33, "[PgUp]");
            downKeys.Add(34, "[PgDn]");
            downKeys.Add(35, "[End]");
            downKeys.Add(36, "[Home]");
            downKeys.Add(37, "[Left]");
            downKeys.Add(38, "[Up]");
            downKeys.Add(39, "[Right]");
            downKeys.Add(40, "[Down]");

            downKeys.Add(41, "[Select]");
            downKeys.Add(42, "[Print]");
            downKeys.Add(43, "[Execute]");
            downKeys.Add(44, "[PrtScrn]");

            downKeys.Add(45, "[Insert]");
            downKeys.Add(46, "[Del]");
            downKeys.Add(47, "[Help]");

            downKeys.Add(106, "*");//numberpad keys
            downKeys.Add(107, "+");
            downKeys.Add(109, "-");
            downKeys.Add(110, ".");
            downKeys.Add(111, "/");

            downKeys.Add(144, "[NumLock]");
            downKeys.Add(145, "[Scrl]");
#if SHOW_SHIFT
			downKeys.Add(160, "[LShift]");
			downKeys.Add(161, "[RShift]");
			downKeys.Add(20, "[CapsLock]");
#else
            downKeys.Add(160, "");
            downKeys.Add(161, "");
            downKeys.Add(20, "");
#endif
            downKeys.Add(162, "[LCtrl]");
            downKeys.Add(163, "[LCtrl]");

            downKeys.Add(192, "`");
            downKeys.Add(189, "-");
            downKeys.Add(187, "=");
            downKeys.Add(219, "[");
            downKeys.Add(221, "]");
            downKeys.Add(220, "\\");
            downKeys.Add(186, ";");
            downKeys.Add(222, "'");
            downKeys.Add(191, "/");
            downKeys.Add(190, ".");
            downKeys.Add(188, ",");
            #endregion
            //
            #region shiftkeys
            shiftKeys.Add(192, "~");
            shiftKeys.Add(49, "!");
            shiftKeys.Add(50, "@");
            shiftKeys.Add(51, "#");
            shiftKeys.Add(52, "$");
            shiftKeys.Add(53, "%");
            shiftKeys.Add(54, "^");
            shiftKeys.Add(55, "&");
            shiftKeys.Add(56, "*");
            shiftKeys.Add(57, "(");
            shiftKeys.Add(48, ")");
            shiftKeys.Add(189, "_");
            shiftKeys.Add(187, "+");
            shiftKeys.Add(219, "{");
            shiftKeys.Add(221, "}");
            shiftKeys.Add(220, "|");
            shiftKeys.Add(186, ":");
            shiftKeys.Add(222, "\"");
            shiftKeys.Add(191, "?");
            shiftKeys.Add(190, ">");
            shiftKeys.Add(188, "<");
            #endregion
            //
            #region upkeys
#if SHOW_SHIFT
			upKeys.Add(160, "[LShift-Up]");
			upKeys.Add(161, "[RShift-Up]");
#endif
            #endregion
        }
        #endregion
        /// <returns>
        ///     A string value if the "up key" is worth showing, ie shift, otherwise.. an empty string
        /// </returns>
        static string parsekeyup(int vk)
        {

            return new string((char)vk, 1);
        }

        /// <summary>
        /// renders a keycode into a human readable string.
        /// </summary>
        public static string parsekeydown(int vk)
        {
            bool caps = (((ushort)Hooker.GetKeyState(0x14)) & 0xffff) != 0;
            bool shift = Hooker.IsKeyPushedDown(Keys.ShiftKey);
            //33-47, 58-64, 91-96, 123-126
            if (vk >= 65 && vk <= 90)
            {
                //uppercase only if one or the other, but not both.
                return ((shift ^ caps) ?
                        new string(new char[] { (char)vk }) :
                        new string(new char[] { (char)vk }).ToLower());
            }
            if (vk > 95 && vk < 106) return (vk - 96).ToString(); //numberpad keys
            if (vk >= 112 && vk <= 135) return ("[F" + (vk - 111).ToString() + "]");//function keys


            if (shift && shiftKeys.ContainsKey(vk))
                return shiftKeys[vk];
            if (downKeys.ContainsKey(vk))
                return downKeys[vk];
            if (vk >= 48 && vk <= 57)
                return new string((char)vk, 1); //top number keys
            else return "[vk" + Convert.ToString(vk) + "]";


        }

        #endregion

    }
}
