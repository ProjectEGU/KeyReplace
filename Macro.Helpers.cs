using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyReplace
{
    public partial class Macro
    {
        public bool HasEvents { get { return _events.Count > 0; } }

        public bool HasTriggers { get { return _triggers.Count > 0; } }

        public static bool IsShift(int vk)
        {
            return vk == (int)Keys.ShiftKey || vk == (int)Keys.LShiftKey || vk == (int)Keys.RShiftKey;
        }
        public static bool IsCtrl(int vk)
        {
            return vk == (int)Keys.ControlKey || vk == (int)Keys.LControlKey || vk == (int)Keys.RControlKey;
        }
        public static bool IsAlt(int vk)
        {
            return vk == (int)Keys.Menu || vk == (int)Keys.LMenu || vk == (int)Keys.RMenu;
        }
        /// <summary>
        /// Gets the current modifier keys.
        /// 
        /// If used during a low level hook callback, it may not include the current key - 
        /// 
        /// Ie pressing shift by itself will cause shift to not register.
        /// </summary>
        /// <param name="vk">The keycode of the currently pressed key. </param>
        /// <returns></returns>
        public static ModifierKey GetCurrentModifierKeys(int vk)
        {
            bool isDownShift = IsKeyPushedDown(Keys.ShiftKey) || IsShift(vk);
            bool isDownAlt = IsKeyPushedDown(Keys.Menu) || IsAlt(vk);//sneaky AF LOL
            bool isDownCtrl = IsKeyPushedDown(Keys.ControlKey) || IsCtrl(vk);
            return GetModifierKeys(isDownShift, isDownAlt, isDownCtrl);
        }

        public static ModifierKey GetModifierKeys(bool isDownShift, bool isDownAlt, bool isDownCtrl)
        {
            return (isDownShift ? ModifierKey.Shift : ModifierKey.None) |
                            (isDownCtrl ? ModifierKey.Ctrl : ModifierKey.None) |
                            (isDownAlt ? ModifierKey.Alt : ModifierKey.None);
        }

        public static string GetKeyString(int vk, ModifierKey modifiers)
        {
            string keyString = GetKeyString(vk)
                .Replace("Control", "Ctrl").Replace("Menu", "Alt").Replace("Return", "Enter")
                .Replace("Next","PageDn").Replace("Capital","CapsLock").Replace("Scroll","ScrLock");
            // todo. optimize the above to use less replace statements... perhaps store a remap dict...
            string tmp = modifiers.ToString();
            if (IsShift(vk) || IsAlt(vk) || IsCtrl(vk))
                tmp = tmp.Replace(keyString.Replace("L","").Replace("R",""), ""); //prevent Shift+Shift or Shift+LShift

            string modiString = Regex.Replace(tmp.ToString(), "([a-z])([A-Z])", "$1+$2"); //i.e ShiftAlt -> Shift+Alt
            return (modifiers == ModifierKey.None || modiString.Length == 0 ? "" : modiString + "+") + keyString;

        }


        private static string GetKeyString(int vk)
        {
            if (vk == (int)SpecialKey.OriginalKey)
                return "(orig)";// a bit risky - what if system decides to use that.
            else if (vk == (int)SpecialKey.MouseLeft)
                return "Left Mouse";
            else if (vk == (int)SpecialKey.MouseMid)
                return "Mid Mouse";
            else if (vk == (int)SpecialKey.MouseRight)
                return "Right Mouse";
            else if (vk == (int)SpecialKey.MouseX1)
                return "MouseX1";
            else if (vk == (int)SpecialKey.MouseX2)
                return "MouseX2";
            int nonVirtualKey = MapVirtualKey((uint)vk, 2);
            char mappedChar = Convert.ToChar(nonVirtualKey);//todo - special case for modifier key and if == 0 and whatnot.
            if (mappedChar <= 32 || mappedChar >= 92) //http://www.asciitable.com/ - uppercase or shift-chars will not be used anyway
                return CapitalizeFirst(((Keys)vk).ToString().Replace("Oem", "").Replace("Key",""));
            else
                return Encoding.ASCII.GetString(new byte[] { (byte)mappedChar });
        }
        /// <summary>
        /// Returns the input string with the first character converted to uppercase, or mutates any nulls passed into string.Empty
        /// 
        /// https://stackoverflow.com/questions/4135317/make-first-letter-of-a-string-upper-case-with-maximum-performance/27073919#27073919
        /// </summary>
        private static string CapitalizeFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        private static string TitleCase(string s )
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                        ToTitleCase(s);
        }

        public static string GetKeyString( int vk, bool shift, bool alt, bool ctrl)
        {
            return GetKeyString(vk, GetModifierKeys(shift, alt, ctrl));
        }

        [DllImport("user32.dll")]
        public static extern ushort GetAsyncKeyState(int vKey);
        [DllImport("user32.dll")]
        static extern int MapVirtualKey(uint uCode, uint uMapType);

        public static bool IsKeyPushedDown(Keys vKey)
        {
            return 0 != (GetAsyncKeyState((int)vKey) & 0x8000);
        }
    }
}
