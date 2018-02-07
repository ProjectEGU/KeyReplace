using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace KeyReplace
{
    public partial class frmMain
    {
        Stopwatch lastEvt = new Stopwatch();
        /* Here be compiled variable */
        List<Dictionary<Trigger, CompiledMacro>> page_dicts = new List<Dictionary<Trigger, CompiledMacro>>();
        Dictionary<Trigger, CompiledMacro> macro_table = new Dictionary<Trigger, CompiledMacro>();

        int lastVk = -1;
        int prevMacroVk = -1;
        //ModifierKey curModifierKeys = ModifierKey.None;//update this at start of application.
        //true to let it pass

        static Random rand = new Random(); //reuse this if you are generating many
        private bool _keyUp(Hooker.KeyboardHookStruct arg)
        {
            int vk = arg.VirtualKeyCode;
            bool passKey = _processKeyUp(vk, arg.IsInjected);
            if (!arg.IsInjected && vk == lastVk)
            {
                lastVk = -1;
                prevMacroVk = -1;
            }
            if (passKey)
            {
                RegisterKeyUp(vk);

                // Prevent impossibly fast keystrokes!
                if (lastEvt.ElapsedMilliseconds <= 3)
                {
                    //System.Diagnostics.Debug.WriteLine("delay key up for " + Macro.GetKeyString(vk, ModifierKey.None) + ": " + passKey.ToString().ToUpper());
                    // Now, this used to be an issue when events would be stacked up due to being blocked. Now it's not the case...
                    AccurateTimer.AccurateSleep(5);
                }
                lastEvt.Restart();
            }
            //System.Diagnostics.Debug.WriteLine("pass key up for " + Macro.GetKeyString(vk, ModifierKey.None) + ": " + passKey.ToString().ToUpper());
            return passKey;
        }
        private bool _keyDown(Hooker.KeyboardHookStruct arg)
        {
            int vk = arg.VirtualKeyCode;

            bool passKey = _processKeyDown(vk, arg.IsInjected, vk == lastVk);

            if (!arg.IsInjected && vk != lastVk)
                lastVk = vk;
            /*
            BeginInvoke((MethodInvoker)delegate
                {
                    Text = "last repeat : " + (lastVk == -1 ? "(none)" : Macro.GetKeyString(lastVk, ModifierKey.None));
                });*/

            if (passKey)
            {
                RegisterKeyDown(vk);
                //In general, macro keys should clear the previously repeating key?

                // Prevent impossibly fast keystrokes!
                if (lastEvt.ElapsedMilliseconds <= 3)
                {
                    //System.Diagnostics.Debug.WriteLine("delay key down for " + Macro.GetKeyString(vk, ModifierKey.None) + ": " + passKey.ToString().ToUpper());

                    AccurateTimer.AccurateSleep(4);
                }
                // If the keystroke was passed through, we restart the timer.
                lastEvt.Restart();
            }
           
            //System.Diagnostics.Debug.WriteLine("pass key down for " + Macro.GetKeyString(vk, ModifierKey.None) + ": " + passKey.ToString().ToUpper());
            return passKey;
        }
        bool _processKeyUp(int vk, bool isInjected)
        {
            if (DisableCondition())
                return true;

            bool keyAlreadyUp = settings._opFilterKeyUp && !IsKeyDepressed(vk);
            /*
            System.Diagnostics.Debug.WriteLine("Key up event ---- ");
            System.Diagnostics.Debug.WriteLine("Key: " + Macro.GetKeyString(vk, ModifierKey.None));
            System.Diagnostics.Debug.WriteLine("Filter key up setting: " + settings._opFilterKeyDown);
            System.Diagnostics.Debug.WriteLine("Is depressed: " + IsKeyDepressed(vk));
            System.Diagnostics.Debug.WriteLine("Key already up: " + keyAlreadyUp);
            System.Diagnostics.Debug.WriteLine("is key injected: " + isInjected);
            System.Diagnostics.Debug.WriteLine("---------------- ");
            */
            if (isInjected)
            {

                //System.Diagnostics.Debug.WriteLine("[inj up] "+Macro.GetKeyString(vk, ModifierKey.None) + ": " +IsKeyDepressed(vk));
                if (keyAlreadyUp)
                    return false;
                else
                {
                    //AccurateTimer.AccurateSleep((int)(RandSleepUp()));
                    return true;
                }
            }
                
            return FindKeyMacro(vk, false);
        }
        
        bool _processKeyDown(int vk, bool isInjected, bool isRepeat)
        {
            if (DisableCondition())
                return true;
            
            bool keyAlreadyDown = settings._opFilterKeyDown && IsKeyDepressed(vk);

            /*
            System.Diagnostics.Debug.WriteLine("Key down event ---- ");
            System.Diagnostics.Debug.WriteLine("Key: " + Macro.GetKeyString(vk, ModifierKey.None));
            System.Diagnostics.Debug.WriteLine("Filter key down setting: " + settings._opFilterKeyDown);
            System.Diagnostics.Debug.WriteLine("Is depressed: " + IsKeyDepressed(vk));
            System.Diagnostics.Debug.WriteLine("Key already down: " + keyAlreadyDown);
            System.Diagnostics.Debug.WriteLine("is key injected: " + isInjected);
            System.Diagnostics.Debug.WriteLine("is key repeated: " + isRepeat);
            System.Diagnostics.Debug.WriteLine("lastVk, thisVk, prevMacroVk: " + lastVk + ", " + vk + ", " + prevMacroVk);
            System.Diagnostics.Debug.WriteLine("---------------- ");
            */
            if (isInjected)
            {

                //System.Diagnostics.Debug.WriteLine("[inj down] " + Macro.GetKeyString(vk, ModifierKey.None) + ": " + IsKeyDepressed(vk));
                if (keyAlreadyDown)
                    return false;
                else
                {
                    //AccurateTimer.AccurateSleep(RandSleepDown());
                    return true;
                }
            }
            
            if (isRepeat)
            {
                return vk != prevMacroVk;
            }

            return FindKeyMacro(vk, true);
        }

        // finds and uses a key macro
        private bool FindKeyMacro(int vk, bool isDown)
        {
            bool passKey = true;
            ModifierKey modifiers = Macro.GetCurrentModifierKeys(vk);
            foreach (Trigger trig in macro_table.Keys)
            {
                if (trig.CanTrigger(isDown, vk, modifiers))
                {
                    CompiledMacro macro = macro_table[trig];

                    if (macro.TimeSinceLastRun > settings._macroActivationDelay)
                        macro.Execute();
                    if (macro.HasTableSwitch)
                    {
                        SwitchMacroTable(macro.TableSwitchIdx);
                    }

                    // Only pass if the key was not already down. Otherwise, use macro.passOriginalKey
                    passKey = macro.PassOriginalKey;
                    if (isDown)
                        prevMacroVk = vk;
                    break;
                }
            }
            bool tempResult = passKey && (isDown ^ IsKeyDepressed(vk));
            //System.Diagnostics.Debug.WriteLine(Macro.GetKeyString(vk, ModifierKey.None) + ": " + tempResult.ToString());
            return tempResult; //let it pass
        }

        private void SwitchMacroTable(int idx)
        {
            macro_table = page_dicts[idx];
            BeginInvoke((MethodInvoker)delegate
            {
                txtCurrentPage.Text = "Switched to " + _macroPages[idx].name;
                UpdateOSD(_macroPages[idx].name, idx);
            });
        }

        // ------------- Register key depression
        HashSet<int> _keyDepressedSet = new HashSet<int>();

        void RegisterKeyDown(int vk)
        {
            if (!_keyDepressedSet.Contains(vk))
                _keyDepressedSet.Add(vk);
            /*Invoke((MethodInvoker)delegate {
                //Text = "Failed inconsistency registerkeydown";
                Text = string.Join(", ", _keyDepressedSet.Select(x=>Macro.GetKeyString(x,ModifierKey.None)));
            });*/
        }
        void RegisterKeyUp(int vk)
        {
            if (_keyDepressedSet.Contains(vk))
                _keyDepressedSet.Remove(vk);
            /*Invoke((MethodInvoker)delegate {
                //Text = "Failed inconsistency registerkeyup";
                Text = string.Join(", ", _keyDepressedSet.Select(x => Macro.GetKeyString(x, ModifierKey.None)));
            });*/
        }
        bool IsKeyDepressed(int vk)
        {
            return Macro.IsKeyPushedDown((Keys)vk);
            //return _keyDepressedSet.Contains(vk);
        }
        // ---------------  Mouse event handler
        //Note - it's not gonna be injected since as of now we cannot support mouse inputs.
        private bool _mouseUp(MouseButtons btn, Hooker.MouseLLHookStruct evt)
        {
            if (DisableCondition())
                return true;
            if (evt.IsInjected)
                return true; 

            bool passKey = true;

            foreach (Trigger trig in macro_table.Keys)
            {
                if (trig.CanTrigger(false, evt.Point.X, evt.Point.Y, btn, ModifierKey.None)) //NOTE - NO SUPPORT FOR MOUSE MOdIFIERS!
                {
                    CompiledMacro macro = macro_table[trig];
                    //listView1.Items.Add(macro._events[0].Data.Keyboard.Flags.ToString());//DEBUG
                    macro.Execute();
                    if (macro.HasTableSwitch)
                    {
                        macro_table = page_dicts[macro.TableSwitchIdx];
                        BeginInvoke((MethodInvoker)delegate
                        {
                            txtCurrentPage.Text = "Switched to " + _macroPages[macro.TableSwitchIdx].name;//DEBUG
                            UpdateOSD(_macroPages[macro.TableSwitchIdx].name, macro.TableSwitchIdx);
                        });
                    }
                   // passKey = macro.PassOriginalKey;
                    break;
                }
            }

            return passKey;
        }

        private bool _mouseDown(MouseButtons btn, Hooker.MouseLLHookStruct evt)
        {
            if (DisableCondition())
                return true;

            if (evt.IsInjected)
                return true;

            bool passKey = true;
            
            foreach (Trigger trig in macro_table.Keys)
            {
               /* System.Diagnostics.Debug.WriteLine(trig.processName.FirstOrDefault()
                    + " " + evt.Point.X + " " + evt.Point.Y);*/
                if (trig.CanTrigger(true, evt.Point.X,evt.Point.Y,btn, ModifierKey.None)) //assert here trig.vk == vk.
                {
                    CompiledMacro macro = macro_table[trig];
                    //listView1.Items.Add(macro._events[0].Data.Keyboard.Flags.ToString());//DEBUG
                    macro.Execute();
                    if (macro.HasTableSwitch)
                    {
                        macro_table = page_dicts[macro.TableSwitchIdx];
                        BeginInvoke((MethodInvoker)delegate
                        {
                            txtCurrentPage.Text = "Switched to " + _macroPages[macro.TableSwitchIdx].name;//DEBUG
                            UpdateOSD(_macroPages[macro.TableSwitchIdx].name, macro.TableSwitchIdx);
                        });
                    }
                   // passKey = macro.PassOriginalKey;//DO NOT ALLOW MOUSE SUPRESS INPUT
                    break;
                }
            }
            return passKey;
        }

        //https://stackoverflow.com/questions/577411/how-can-i-find-the-state-of-numlock-capslock-and-scrolllock-in-net
        bool DisableCondition()
        {
            bool CapsLock = (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
            //bool NumLock = (((ushort)GetKeyState(0x90)) & 0xffff) != 0;
            //bool ScrollLock = (((ushort)GetKeyState(0x91)) & 0xffff) != 0;
            return settings._opHardDisableMacro || _opTmpDisableMacro;// || !CapsLock;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

    }
}
#region Code Grave - old sleep methodology
// Another older sleep table
/*
double[][] sleepsDown =
{  new double[]{ 5.481, 9.688, 25 }, // mean, stdDev, skew
              new double[] { 63.308, 20, -1.758 },
             new double[]  { 82.813, 12.891, -0.195 }
        };
double[] selDown = { 1.265, 2 }; // mean, stdDev

double[][] sleepsUp =
 {
            new double[]{ 8.594, 8.203, 4.102 }, //mean, stdDev, skew
              new double[] { 68.75, 6.641,0.977 },
             new double[]  { 68.75, 19.531, -8.203 }
        };
double[] selUp = { -0.612, 4.898 }; // mean, stdDev

int RandSleepDown()
{
    int pick = (int)Math.Min(Math.Abs(Math.Round(GaussRandom(selDown[0], selDown[1]))), 2);

    return (int)Math.Abs(Math.Round(
        GaussRandomSkewed(sleepsDown[pick][0], sleepsDown[pick][1], sleepsDown[pick][2])
        / 1.7 + 4
        ));
}

int RandSleepUp()
{
    int pick = (int)Math.Min(Math.Abs(Math.Round(GaussRandom(selUp[0], selUp[1]))), 2);

    return (int)Math.Abs(Math.Round(
        GaussRandomSkewed(sleepsUp[pick][0], sleepsUp[pick][1], sleepsUp[pick][2])
        / 1.4 + 6
        ));
}
        */

/*static int[][] sleeps =
{
              new int[] {20,2 },
              new int[] {28,3 },
              new int[] {36,8 }
        };
static int RandSleep()
{

    int pick = (int)Math.Min(Math.Floor(Math.Abs(GaussRandom(1, 2))), 2);

    return (int)Math.Round(GaussRandom(sleeps[pick][0], sleeps[pick][1]));
    //GaussRandom(20, 5)
}*/
#endregion