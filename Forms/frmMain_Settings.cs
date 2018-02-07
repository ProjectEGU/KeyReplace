using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace KeyReplace
{
    /// <summary>
    /// Notes on saving.
    /// 
    /// HKCU\Software\KeyReplace is a good foldat.
    /// 
    /// Startup only works for the current user.
    /// 
    /// [ ] comprehensive savingtoreg that actually takes account the type
    /// 
    /// [ ] nullables while loading
    /// 
    /// ----------------------
    /// 
    /// Macro Table file format.
    /// 
    /// Macro Name, # of triggers, # of inputs.
    /// 
    /// 
    /// 
    /// </summary>
    public partial class frmMain
    {
    	Settings settings = null;
    	
        static string macrofile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KeyReplace") + "\\_lastmacro";

        bool _opSuspendControlsWhileLoading = false;
        void LoadAllSettings()
        {

            _opSuspendControlsWhileLoading = true;
            
            settings = Settings.Instance;

            label2.Visible = chkHardDisable.Checked = suspendAllMacrosToolStripMenuItem.Checked = settings._opHardDisableMacro;
            runOnStartupToolStripMenuItem.Checked = chkStartup.Checked = settings._opRunOnStartup;
            
           
            chkOSD.Checked = settings._opOSDEnable;
            if (settings._opOSDLock)
            {
                checkBox2.Checked = true;
            }

            _opSuspendControlsWhileLoading = false;
        }
        void SaveAllSettings()
        {
            Settings.SaveSettings();
        }
        void LoadMacros()
        {
            LoadMacros(macrofile);
        }
        void SaveMacros()
        {
            
            SaveMacros(macrofile);
        }

        void LoadMacros(string macrofile)
        {
            if (!File.Exists(macrofile))
            {
                return;
            }
                

            FileStream fs = new FileStream(macrofile, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                _macroPages = (List<MacroPage>)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                MessageBox.Show("Failed to deserialize. Reason: \r\n\r\n" + e.Message);
            }
            finally
            {
                fs.Close();
            }

        }

        void SaveMacros(string macrofile)
        {
            FileStream fs = new FileStream(macrofile, FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, _macroPages);
            }
            catch (SerializationException e)
            {
               MessageBox.Show("Failed to serialize. Reason: \r\n\r\n" + e.Message);
            }
            finally
            {
                fs.Close();
            }

        }
    }
}
