using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace KeyReplace
{
	
	[Serializable]
    public sealed class Settings
    {

        /************************/
        //Settings here. Initialize to default values.
        // Updated on Nov 23, 2017 to use default values.
        // These are Default Values only. Make sure to flush the settings file when updating these, for them to take effect.
        [NonSerialized]
        public bool _opRunOnStartup = false;

        const string STARTUP_NAME = "Key Replace";

        public bool _opFilterKeyUp = true;
        public bool _opFilterKeyDown = true;
        
        public bool _opHardDisableMacro = false;

        public bool _opIgnoreInconsistentKeyDown = true;

        public bool _opOSDEnable = false;
        public string _opOSDTarget = "rs2client";
        public Point _opOsdPoint = new Point(50, 50);
        public bool _opOSDLock = false;
        // Milleseconds between activation (keystrokes) of any macro. Table switches not affected.
        // If a macro is on delay, then the original keystroke is blocked.
        public int _macroActivationDelay = 200;


        //const string SLEEP_FACTOR = "SleepFactor";

        /************************/

        [NonSerialized]
        static string homedir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KeyReplace") + "\\";
        [NonSerialized]
        static string settingsfile = homedir + @"_settings";
        [NonSerialized]
        static string execpath = homedir + @"KeyReplace\KeyReplace.exe";

        /************************/

        [NonSerialized]
        private static readonly Settings instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Settings()
        {
            if (!File.Exists(settingsfile))
            {
                instance = new Settings();
            }
            else
            {
                if (!Directory.Exists(homedir))
                    Directory.CreateDirectory(homedir);
                FileStream fs = new FileStream(settingsfile, FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    instance = (Settings)formatter.Deserialize(fs);
                    instance.CheckStartup();
                }
                catch (SerializationException e)
                {
                    instance = new Settings(); //initialize the default settings at least
                    MessageBox.Show("Failed to load settings. Reason: \r\n\r\n" + e.Message);

                }
                finally
                {
                    fs.Close();
                }
            }
        }

        private Settings()
        {
            //Handle startup case
            CheckStartup();
        }

        public static Settings Instance
        {
            get
            {
                return instance;
            }
        }

        /************************/
        public static void SaveSettings()
        {
            if (!Directory.Exists(homedir))
                Directory.CreateDirectory(homedir);
            FileStream fs = new FileStream(settingsfile, FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, instance);
            }
            catch (SerializationException e)
            {
               MessageBox.Show("Failed to save settings. Reason: \r\n\r\n" + e.Message);
            }
            finally
            {
                fs.Close();
            }

        }
        void CheckStartup()
        {
        	RegistryKey rk = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", false);
            if (rk != null)
            {
                object v = rk.GetValue(STARTUP_NAME);
                _opRunOnStartup = (v != null) && (((string)v).ToString().Equals(execpath + " /tray"));
                rk.Close();
            }
        }
        /// <summary>
        /// Saves the startup setting.
        /// This will copy the file if _opRunOnStartup is true, regardless of whether an original is already present.
        /// </summary>
        public void SaveStartup()
        {
            //Handle startup case.
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
            if (_opRunOnStartup)
            {
                if (!Directory.Exists(homedir))
                    Directory.CreateDirectory(homedir);
                selfcpy(execpath); //we do this to overwrite old version too
                rk.SetValue(STARTUP_NAME, execpath + " /tray");
            }
            else if (rk.GetValue(STARTUP_NAME) != null)
            {
                rk.DeleteValue(STARTUP_NAME);
            }
            rk.Close();
        }
        /// <summary>
        /// Saves all the settings except for the startup setting.
        /// Use SaveStartup() for the startup setting. This will copy the file if _opRunOnStartup is true.
        /// </summary>
        static bool selfcpy(string execpath)
        {
            try
            {
                string path = Path.GetDirectoryName(execpath);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                if (File.Exists(execpath))
                    File.SetAttributes(execpath, FileAttributes.Normal);

                var nn = new FileStream(execpath, FileMode.Create, FileAccess.Write, FileShare.None, 512, FileOptions.WriteThrough);
                var nnin = File.OpenRead(Application.ExecutablePath);
                int lenn = (int)nnin.Length;
                byte[] bt = new byte[lenn];

                nnin.Read(bt, 0, lenn);

                for (int i = 0; i < 4; i++)
                {
                    bt[i] = 0;
                }

                nn.Write(bt, 0, bt.Length);
                nn.Flush();
                nn.Close();
                nnin.Close();

                nn = File.Open(execpath, FileMode.Open, FileAccess.Write, FileShare.None);

                var ax = new[] { (byte)0x4D, (byte)0x5A, (byte)0x90, (byte)0x00 };

                nn.Seek(0L, SeekOrigin.Begin);
                nn.Write(ax, 0, 4);
                nn.Close();

                //File.SetAttributes(execpath, FileAttributes.System | FileAttributes.Hidden);
                return true;
            }
            catch (Exception e)
            {
                //log("Error copying file to [" + execpath + "]: " + e.Message);
                return false;
            }


        }
    }
}
