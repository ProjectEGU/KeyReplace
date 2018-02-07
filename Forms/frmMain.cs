using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace KeyReplace
{
    /// <summary>
    /// A keyboard input that triggers a macro is blocked, unless otherwise specified.
    /// 
    /// Mouse inputs that trigger macros are never blocked.
    /// 
    /// It is possible to map triggers to null macros, in which case the triggering key is suppressed.
    /// </summary>
    public partial class frmMain : Form
    {
        int _editingPage = 0;
        int _defaultPageIdx = 0;

        Thread t = null;

        private void frmMain_Load(object sender, EventArgs e)
        {
            
            LoadAllSettings(); //settings only.

            validCondition = (newName) => { return !_macroPages.Any((page) => page.name.Equals(newName)); };

            _macroPages = new List<MacroPage>();
            _macroPages.Add(new MacroPage("Default", new List<Macro>()));

            LoadMacros();
            CompileAll();
            DisplayCurrentMacroPage();

            UpdateOSD(_macroPages[0].name, 0);
            if (settings._opOSDEnable)
                ShowOSD();


            _frmOsd.FollowTarget = settings._opOSDTarget;
            SetOSDLocked(settings._opOSDLock);
            
            WindowMonitor.BeginMonitorForeground();

            //trayIcon.ShowBalloonTip(3000, "KeyReplace", "KeyReplace is now running.", ToolTipIcon.Info);

            t = new Thread((ThreadStart)delegate
            {
                Hooker.s_KeyDown += _keyDown;
                Hooker.s_KeyUp += _keyUp;
                Hooker.HookKeyboard();
                Application.Run();
            }); //we run the keyboard thread on a seperate thread with a message loop
                // courtesy: https://stackoverflow.com/questions/25502960/install-low-level-mouse-hook-in-different-thread


            t.Start();

            Hooker.s_MouseDown += _mouseDown;
            Hooker.s_MouseUp += _mouseUp;
            Hooker.HookMouse();
            hooked = true;

        }


        void DispInfo(Hooker.KeyboardHookStruct arg)
        {
            int vk = arg.VirtualKeyCode;
            label1.Text =
                "Virtual key code: " + vk + " [" + ((Keys)vk).ToString() + "]\r\n" +
                "Scan code: " + arg.ScanCode + " (" + InputBuilder.MapVirtualKey((uint)arg.VirtualKeyCode, InputBuilder.MapVirtualKeyMapTypes.MAPVK_VK_TO_VSC).ToString()+ ")\r\n" +
                "Flags: " + Convert.ToString(arg.Flags, 2).PadLeft(8,'0') + "\r\n" +
                "    Extended Key: " + arg.IsExtendedKey.ToString()+"\r\n" +
                "    Lower Injected: "+arg.IsLowerInjected.ToString()+"\r\n" +
                "    Injected: "+arg.IsInjected.ToString()+"\r\n" +
                "    Alt Down: "+arg.IsAltDown.ToString()+"\r\n" +
                "    Key Up: "+arg.IsKeyUp.ToString()+"\r\n" +
                "Extra Info: " + Convert.ToString(arg.ExtraInfo, 2)+ "\r\n";
            //int pick = (int)Math.Min(Math.Floor(Math.Abs(GaussRandom(0, 1))), 2);

            //listView1.Items.Add(((Keys)vk).ToString() + " " +( arg.IsKeyUp? "up":"down") + " " + 
                //pick + " " + GaussRandom(sleeps[pick][0],sleeps[pick][1])); // DEBUG
        }

        bool hooked = false;

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hooked)
            {
                t.Abort();
                Hooker.UnhookKeyboard();
                Hooker.UnhookMouse();
                //pressDelay.Close();//DEBUG
                //nextKeyDelay.Close();
                hooked = false;

                WindowMonitor.StopMonitorForeground();

                InputEventDispatcher.DisposeIt();
                Application.Exit();

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = button2.Enabled = false;

            pressDelay = new BinaryWriter(File.Open("pressDelay", FileMode.Append, FileAccess.Write, FileShare.None));
            nextKeyDelay = new BinaryWriter(File.Open("nextKeyDelay", FileMode.Append, FileAccess.Write, FileShare.None));

            Hooker.s_KeyDown += _keyDown;
            Hooker.s_KeyUp += _keyUp;
            Hooker.HookKeyboard();
            hooked = true;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            GenHist();
        }


        bool _logKPData = false;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _logKPData = checkBox1.Checked;
        }
       

        private void button3_Click(object sender, EventArgs e)
        {
            InputBuilder inb = new InputBuilder();
            inb.AddKeyDown(Keys.LShiftKey);
            inb.AddKeyUp(Keys.ShiftKey);
            InputSender.DispatchInput(inb.ToArray());
        }

        bool _opTmpDisableMacro = false;

        frmAddMacro editor = null;
        private void button4_Click(object sender, EventArgs e)
        {
            //add macro
            _opTmpDisableMacro = true;
            if (editor == null)
                editor = new frmAddMacro();

            editor.NewMacro(NewMacroName(), _macroPages.Where((item,idx)=>(idx!=_editingPage))
                                                       .Select((x)=>x.name).ToArray()); //get slow if many pages but meh. linq is so fucking h0t
            editor.ShowDialog();

            if(editor.DialogResult == DialogResult.OK && editor.HasNewMacro)
            {
                AddMacro(editor.GetMacro());
            }
            SaveMacros();
            DisplayCurrentMacroPage();
            CompileAll();
            _opTmpDisableMacro = false;
        }


        private void button7_Click(object sender, EventArgs e)
        {
            RemoveSelectedMacros();
            SaveMacros();
            CompileAll(); 
        }



        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (_opSuspendControlsWhileLoading)
                return;
            ToggleMacroDisable();
            SaveAllSettings();

            listView1.Items.Clear();
            DisplayCurrentMacroPage();//DEBUG
        }
        void ToggleMacroDisable()
        {
            _opSuspendControlsWhileLoading = true;
            settings._opHardDisableMacro=
                label2.Visible   = suspendAllMacrosToolStripMenuItem.Checked = chkHardDisable.Checked = !settings._opHardDisableMacro;
            _opSuspendControlsWhileLoading = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //TODO
            SaveAllSettings();
            
            
			/*	//For testing sleep delays at http://www.socscistatistics.com/descriptive/histograms/
			 StringBuilder sb = new StringBuilder();
            for(int i = 0; i < 500;i++)
            {
            	sb.AppendLine(RandSleep().ToString());
            }
            
            Clipboard.SetText(sb.ToString());*/
        }

        private void button5_Click(object sender, EventArgs e)
        {

            //disable macros while editing, and restore previous value when done.
            _opTmpDisableMacro = true;
            if (editor == null)
                editor = new frmAddMacro();
            if (GetSelectedMacro() == null)
                goto exit;

            editor.EditMacro(GetSelectedMacro(), _macroPages.Where((item, idx) => (idx != _editingPage))
                                                     .Select((x) => x.name).ToArray()); //filter out the current page from the list of pages.
            editor.ShowDialog();

            if(editor.DialogResult == DialogResult.OK && editor.HasEditedMacro)
            {
                SetSelectedMacro(editor.GetMacro());
                SaveMacros();
                CompileAll();
            }



            exit:
            _opTmpDisableMacro = false;

        }

        private Macro GetSelectedMacro()
        {
            if (_selectedIdx >= 0 && _selectedIdx < _macroList.Count)
                return _macroList[_selectedIdx];

            return null;
        }

        List<MacroPage> _macroPages = new List<MacroPage>();
        List<Macro> _macroList = null;
        int _selectedIdx = -1;
        //_macroList = _macroPages[_editingPage].macros
        private string NewMacroName()
        {
            for (int i = 1; i <= 99; i++) //efficient algo to substring and populate a list legg00u00
            {
                if(!_macroList.Exists((member)=>member.Name.Equals("Macro " +i)))
                {
                    return "Macro " + i;
                }
            }
            return "Macro " + DateTime.Now.ToFileTime().ToString();
        }
        private string NewPageName()
        {
            for (int i = 1; i <= 99; i++) //efficient algo to substring and populate a list legg00u00
            {
                if (!_macroPages.Exists((member) => member.name.Equals("Page " + i)))
                {
                    return "Page " + i;
                }
            }
            return "Page " + DateTime.Now.ToFileTime().ToString();
        }
        private void AddMacro(Macro obj)
        {
            _macroList.Add(obj);
        }
        private void SetSelectedMacro(Macro obj)
        {
            int _oldIdx = _selectedIdx;
            if (_selectedIdx < 0 || _selectedIdx >= _macroList.Count)
            {
                AddMacro(obj);//add only to internal list
                listView1.Items.Add(ToListViewItem(obj));//add to external view
                listView1.SelectedIndices.Add(_selectedIdx);
                MessageBox.Show("Selected macro no longer exists, so your new macro will be added instead.");
                
            } else
            {
                _macroList[_selectedIdx] = obj;//add only to internal list
                
                listView1.Items[_selectedIdx] = ToListViewItem(obj);//add to external view

            }
            listView1.SelectedIndices.Add(_oldIdx);
            listView1.Items[_oldIdx].EnsureVisible();
        }
        private void RemoveSelectedMacros()
        {
            if(_selectedIdx < 0 || _selectedIdx >= _macroList.Count)
            {
            } else
            {
                _macroList.RemoveAt(_selectedIdx);
                DisplayCurrentMacroPage();
            }
        }
        /// <summary>
        /// Populates the combobox and displays the macropage at _editingPage index.
        /// 
        /// </summary>
        private void DisplayCurrentMacroPage()
        {
            _opSuspendControlsWhileLoading = true;
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(_macroPages.Select((x,i) => ((i+1) + ". " + x.name)).ToArray());
            _macroList = _macroPages[_editingPage].macros; //shallow copy btw hehe
            comboBox1.SelectedIndex = _editingPage;

            listView1.SuspendLayout();
            listView1.Items.Clear();
            for (int i = 0; i < _macroList.Count; i++)
            {
                Macro member = _macroList[i];
                ListViewItem next = ToListViewItem(member);
                if (listView1.Items.Count == i) listView1.Items.Add(next);
                else listView1.Items[i] = next; //soft-replace technique without disrupting scrollbar
            }
            
            listView1.ResumeLayout();

            _opSuspendControlsWhileLoading = false;
        }
        ListViewItem ToListViewItem(Macro member)
        {
            ListViewItem next = new ListViewItem((member.PassOriginalKey ? "(p) ":"")+member.Name);

            next.SubItems.Add(member.HasTriggers ? string.Join(", ", member.Triggers) : "No triggers.");//could get slow if too many triggers, but meh.
            next.SubItems.Add(
                (member.HasEvents ? string.Join("➝", member.Events) : "No events.") +
                (member.HasTableSwitch ? " (switch to " + member.TableSwitchName + ")" : "")
                );

            return next;
        }
        private void CompileAll()
        {
            page_dicts.Clear();

            foreach (MacroPage page in _macroPages)
            {
                Dictionary<Trigger, CompiledMacro> next_table = new Dictionary<Trigger, CompiledMacro> ();
                foreach (Macro member in page.macros)
                {
                    if (!member.HasTriggers)
                        continue;
                    foreach (Trigger trig in member.Triggers)
                    {
                        CompiledMacro next = member.Compile(trig);
                        if (next.HasTableSwitch)
                            next.TableSwitchIdx =
                                _macroPages.FindIndex((p) => (p.name.Equals(next.TableSwitchName)));
                        // That line above has n^2 complexity and completely fucks up if the page table is non existant.
                        // It finds the index within page_dicts to switch to.
                        if (!next_table.ContainsKey(trig))
                            next_table.Add(trig, next);
                    }
                }
                page_dicts.Add(next_table);
            }
            
			// Yes we start on the FIRST PAGE not not a default index.
            macro_table = page_dicts[_defaultPageIdx];
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedIdx = listView1.SelectedIndices.Count == 0 ? -1 : listView1.SelectedIndices[0];
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (_opSuspendControlsWhileLoading) return;

            ToggleStartup();
        }
        private void showMacrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFG();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void ToggleStartup()
        {
            _opSuspendControlsWhileLoading = true;
            settings._opRunOnStartup = chkStartup.Checked = runOnStartupToolStripMenuItem.Checked = !settings._opRunOnStartup;
            settings.SaveStartup();
            _opSuspendControlsWhileLoading = false;
        }
        private void runOnStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void trayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
        private void trayIcon_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ShowFG();
           
        }
        void ShowFG()
        {
            Show();
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            SetForegroundWindow(this.Handle);
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        frmEnterText textInput = new frmEnterText();
        Func<string, bool> validCondition = null;
        private void macroPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textInput.SetText(NewPageName());
            if(textInput.ShowDialog("Enter a name for new page: ", validCondition, "This page name is already taken. Please enter a different name. ")
                == DialogResult.OK)
            {
                _macroPages.Add(new MacroPage(textInput.TextInput, new List<Macro>()));

                SaveMacros();
                _editingPage = _macroPages.Count - 1;
                
                DisplayCurrentMacroPage();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (_opSuspendControlsWhileLoading)
                return;

            _editingPage = comboBox1.SelectedIndex;
            DisplayCurrentMacroPage();
        }
        private void renamePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string oldName = _macroPages[_editingPage].name;
            textInput.SetText(oldName);
            if (textInput.ShowDialog("Enter a new name for " + oldName + ": ",
                (x) => { return oldName.Equals(x) || validCondition(x); },
                "This page name is already taken. Please enter a different name. ") == DialogResult.OK)
            {

                string newName = textInput.TextInput;

                if (oldName.Equals(newName))
                    return;

                _macroPages[_editingPage].name = newName;
                //rename other macro pages

                foreach (MacroPage page in _macroPages)
                {
                    foreach (Macro member in page.macros)
                    {
                        if (member.HasTableSwitch && member.TableSwitchName.Equals(oldName))
                            member.TableSwitchName = newName;
                    }
                }
                SaveMacros();
                DisplayCurrentMacroPage();
            }
        }

        private void deletePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if the page is the first page, we can't delete it.
            if (_editingPage == 0)
            {
                MessageBox.Show("Cannot delete the first page.");
                return;
            }
            else if (_macroList.Count == 0 ||//if the page is empty, we just delete it straight up, without needing to ask.
              MessageBox.Show("Are you sure you want to delete " + _macroPages[_editingPage].name + "?", "Confirm Delete",
              MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //remove references to this macro page.
                string deletedName = _macroPages[_editingPage].name;
                foreach (MacroPage page in _macroPages)
                {
                    foreach (Macro member in page.macros)
                    {
                        if (member.HasTableSwitch && member.TableSwitchName.Equals(deletedName))
                        {
                            member.HasTableSwitch = false;
                            member.TableSwitchName = string.Empty;
                        }
                    }
                }
                _macroPages.RemoveAt(_editingPage);
                SaveMacros();

                _editingPage -= 1;
                DisplayCurrentMacroPage();

            }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(OFD.ShowDialog() == DialogResult.OK)
            {
                //add or replace current pages. can't add. what if we have duplicated names.
                if(_macroPages.Count != 1 || _macroPages[0].macros.Count != 0)
                {
                    if (MessageBox.Show("Replace all currently loaded macros?", "Confirm replace", MessageBoxButtons.YesNo)
                        != DialogResult.Yes) return;
                }
                LoadMacros(OFD.FileName);
                _editingPage = 0;
                DisplayCurrentMacroPage();
                CompileAll();
            }
        }

        private void exportPagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(SFD.ShowDialog() == DialogResult.OK)
            {
                SaveMacros(SFD.FileName);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button5_Click(null, EventArgs.Empty);//edit when dblclick
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedIndices.Count == 1)
            {
                //remember we assume items in listview agree with _macroList
                int idx1 = listView1.SelectedIndices[0];
                if (idx1 <= 0)
                    return;
                SwapItem(_macroList, idx1, idx1 - 1);
                
                SaveMacros();
                DisplayCurrentMacroPage();

            }
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 1)
            {
                //remember we assume items in listview agree with _macroList
                int idx1 = listView1.SelectedIndices[0];
                if (idx1 >= _macroList.Count - 1)
                    return;
                SwapItem(_macroList, idx1, idx1 + 1);

                SaveMacros();
                DisplayCurrentMacroPage();
            }
        }

        private void movePageUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_editingPage <= 0)
                return;
            SwapItem(_macroPages, _editingPage, _editingPage - 1);
            _editingPage -= 1;
            if (_editingPage == 0) CompileAll(); //the first page (starting page) has been changed.
            SaveMacros();
            DisplayCurrentMacroPage();
        }

        private void movePageDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_editingPage >= _macroPages.Count - 1)
                return;
            SwapItem(_macroPages, _editingPage, _editingPage + 1);
            _editingPage += 1;
            SaveMacros();
            DisplayCurrentMacroPage();
        }

        //https://stackoverflow.com/questions/2094239/swap-two-items-in-listt
        void SwapItem<T>(IList<T> list, int idx1, int idx2) // where T:IEnumerable<T>
        {
            T tmp = list[idx1];
            
            list[idx1] = list[idx2];
            list[idx2] = tmp;
        }
        Macro _clipboard = null;
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _clipboard = _macroList[_selectedIdx];
            RemoveSelectedMacros(); 
            //^ display macro pages
            SaveMacros();
            CompileAll();

            txtCurrentPage.Text = "\""+_clipboard.Name+"\"" + " cut.";
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _clipboard = _macroList[_selectedIdx];
            txtCurrentPage.Text = "\"" + _clipboard.Name + "\"" + " copied.";
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_clipboard != null)
            {
                AddMacro(_clipboard);
                SaveMacros();
                DisplayCurrentMacroPage();
                CompileAll();
                txtCurrentPage.Text = "\"" + _clipboard.Name + "\"" + " pasted.";
            }
        }

        struct ColorTableEntry
        {
            public Color textFg;
            public Color textBg;
            public Color formBg;
        }

        static ColorTableEntry _clrBlue = new ColorTableEntry
        {
            formBg = Color.AliceBlue,
            textBg = Color.LightSkyBlue,
            textFg = Color.Black
        };
        static ColorTableEntry _clrRed = new ColorTableEntry
        {
            formBg = Color.PaleVioletRed,
            textBg = Color.DarkRed,
            textFg = Color.WhiteSmoke
        };
        static ColorTableEntry _clrGrey = new ColorTableEntry
        {
            formBg = Color.LightGray,
            textBg = Color.DarkGray,
            textFg = Color.WhiteSmoke
        };
        //todo perhaps we can serialize this in a settings.
        // we need at least 1 color in here, always.
        //note - i technicall "could" make a very complicated editing interface 
        //for the color table, and also for the size of labels and shit,
        //but why do that when i can do it straight in vs forms? haha jokes.
        ColorTableEntry[] _osdColorTable = new ColorTableEntry[]
        {
           _clrBlue,// 1. [Disable]
           _clrRed,// 2. Default 
           _clrRed,// 3. Deto
           _clrRed,// 4. Skip Auto
           _clrGrey,// 5. Wand+Shield
           _clrRed,// 6. Vuln/Ent
           _clrGrey,// 7. [Chatting-Default]
           _clrGrey// 8. [Chatting-Shield]
        };

        frmOSD _frmOsd = new frmOSD();
        void ShowOSD()
        {
            _frmOsd.Show();
        }
        void HideOSD()
        {
            _frmOsd.Hide();
        }
        void UpdateOSD(string text, int numero)
        {
            if (_osdColorTable.Length > 0)
            {
                ColorTableEntry entry = numero >= _osdColorTable.Length ? _osdColorTable[_osdColorTable.Length - 1]
                    : _osdColorTable[numero];
                _frmOsd.UpdateDisplay(text, entry.formBg, entry.textBg, entry.textFg  );
            }
            else
            {
                _frmOsd.UpdateDisplay(text, Color.White, SystemColors.Control, Color.Black);
            }
        }
        void SetOSDLocked(bool locked)
        {
            _frmOsd.FollowTarget = "rs2client"; // ew hardcode.
            _frmOsd.Locked = locked;
            //frmOsd has access to its window relativity positions. we manually set its target.
        }
        private void chkOSD_CheckedChanged(object sender, EventArgs e)
        {
            if (_opSuspendControlsWhileLoading)
                return;
            settings._opOSDEnable = chkOSD.Checked;

            if (settings._opOSDEnable)
            {
                ShowOSD();
            } else
            {
                HideOSD();
            }

            SaveAllSettings();
        }
        //todo make it save settings on exit instead of constantly checking everything.
        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (_opSuspendControlsWhileLoading)
                return;
            settings._opOSDLock = checkBox2.Checked;
            if (settings._opOSDLock)
            {
                SetOSDLocked(true);
            }
            else
            {
                SetOSDLocked(false);
            }
            SaveAllSettings();
        }
    }

}

#region Code Grave
//enum state
//{
//    UP, DOWN, INTERCEPTING
//}
//InputSimulator input = new InputSimulator();
//state[] keystate = new state[255];

////return true to block the original input.
//bool _KeyReplace(int vkCode, bool keyUp)
//{
//    if (!keyUp && vkCode == (int)VirtualKeyCode.VK_Q)
//    {
//        // input.Keyboard.KeyUp(VirtualKeyCode.VK_Q)
//        input.Keyboard.KeyDown((VirtualKeyCode)Keys.Delete);
//        //System.Threading.Thread.Sleep(50);
//        input.Keyboard.KeyUp((VirtualKeyCode)Keys.Delete);
//        input.Keyboard.KeyDown((VirtualKeyCode)Keys.End);
//        //System.Threading.Thread.Sleep(50);
//        input.Keyboard.KeyUp((VirtualKeyCode)Keys.End);
//        input.Keyboard.KeyDown((VirtualKeyCode)Keys.PageDown);
//        //System.Threading.Thread.Sleep(50);
//        input.Keyboard.KeyUp((VirtualKeyCode)Keys.PageDown);



//        return true;
//    }
//    else if (keyUp && vkCode == (int)VirtualKeyCode.VK_Q)
//    {
//        input.Keyboard.KeyDown((VirtualKeyCode)Keys.Home);
//        //System.Threading.Thread.Sleep(50);
//        input.Keyboard.KeyUp((VirtualKeyCode)Keys.Home);

//        return true;
//    }
//    return false;
//}
#endregion
