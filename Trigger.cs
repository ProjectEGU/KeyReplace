using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyReplace
{
    [Serializable]
    public struct Trigger
    {
        public TriggerType triggeredBy; //int 0-2
        public int vk; //int 0-255 - 8 bits
        public ModifierKey modifiers;//int 2^3 - 3 bits

        /// <summary>
        /// The name of the process within which this can trigger. 
        /// 
        /// If this is null or empty, then it will be ANYWHERE on the screen.
        /// </summary>
        public HashSet<string> processName;
        /// <summary>
        /// The area within the process or screen to click.
        /// 
        /// If this is rectangle.Empty, then it can be ANYWHERE in the process window.
        /// 
        /// If processName is empty or null, then this will be the region on the screen.
        /// </summary>
        public Rectangle winRect;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="vk">If this is a keyboard trigger, then vk is the virtual code. If this is a mouse trigger, it is the integer value of the MouseButtons enum.</param>
        /// <param name="modifiers"></param>
        /// <param name="processName"></param>
        /// <param name="winRect"></param>
        public Trigger(TriggerType evt, int vk, ModifierKey modifiers, string[] processName, Rectangle winRect)
        {
            this.triggeredBy = evt;
            this.vk = vk;
            this.modifiers = modifiers;

            this.processName = new HashSet<string>(processName,StringComparer.OrdinalIgnoreCase);
            this.winRect = winRect;
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Trigger))
                return false;
            Trigger a = (Trigger)obj;
            return this.triggeredBy == a.triggeredBy && this.vk == a.vk && this.modifiers == a.modifiers
                && this.processName == a.processName && this.winRect == a.winRect;
        }
        public override int GetHashCode()
        {
            return vk + ((int)modifiers << 8) + ((int)triggeredBy << 11);
        }
        /// <summary>
        /// Make sure your foreground window checker is running.
        /// </summary>
        public bool CanTrigger(bool isKeyDown, int vk, ModifierKey modifiers)
        {
            //foreground name check
            if (processName.Any() && !processName.Contains(WindowMonitor.ForegroundProcessName))
            {
                return false;
            }

            if (isKeyDown)
                return this.triggeredBy == TriggerType.KeyDown && this.vk == vk && this.modifiers == modifiers;
            else 
                return this.triggeredBy == TriggerType.KeyUp && this.vk == vk && this.modifiers == modifiers;
        }

        /// <summary>
        /// Make sure your foreground window checker is running.
        /// </summary>
        public bool CanTrigger(bool isMouseButtonDown, int x, int y, MouseButtons button, ModifierKey modifiers)
        {

            //button/modifier check
           /* System.Diagnostics.Debug.WriteLine(this.triggeredBy.ToString () + ": "+isMouseButtonDown.ToString()+", " +
                "Source - " + button.ToString() + " Dest - " +( (MouseButtons)vk).ToString()
                );*/
            if (this.vk == (int)button && this.modifiers == modifiers)
            {
                //foreground name check
               // System.Diagnostics.Debug.WriteLine(WindowMonitor.ForegroundProcessName);
                if (processName.Contains(WindowMonitor.ForegroundProcessName))
                {
                    //foreground rect check
                    Point fgLoc = WindowMonitor.GetWindowLocation();
                    x -= fgLoc.X; y -= fgLoc.Y;
                }
                else if (processName.Any()) return false; 
                //so we just checked the name. the name was non null. this means it wasnt the universal identifier, so we return false.
               /* System.Diagnostics.Debug.WriteLine(
    "Point: " + x + " " + y + " -- " + winRect.ToString()

    );*/

                if (winRect != Rectangle.Empty && !winRect.Contains(x, y))
                    return false;
                //mouse check heh Aids Enjoi!

                if (isMouseButtonDown)
                    return this.triggeredBy == TriggerType.MouseDown;
                else
                    return this.triggeredBy == TriggerType.MouseUp;

            }
            else return false;
        }

        public override string ToString()
        {
            if (triggeredBy == TriggerType.KeyDown || triggeredBy == TriggerType.KeyUp)
            {
                return
                    (((winRect == Rectangle.Empty) && !processName.Any()) ? "" : "*") +
                    Macro.GetKeyString(vk, modifiers) + (triggeredBy == TriggerType.KeyDown ? "" : " " + triggeredBy.ToString().Replace("Key", "").ToLower());
            } else
            {
                return
                  (((winRect == Rectangle.Empty) && !processName.Any()) ? "" : "*") +
                  ((MouseButtons)vk).ToString()+ " button" + (triggeredBy == TriggerType.MouseDown ? "" : " up");

            }

        }
    }

}
#region Code Grave

//public static Trigger Unmarshal(string s)
//{
//    string[] data = s.Split(',');
//    if (data.Length != 3) throw new Exception("Malformed string for Trigger");

//    return new Trigger(
//        (TriggerType)int.Parse(data[0]),
//        int.Parse(data[1]),
//        (ModifierKey)int.Parse(data[2]));
//}
///// <summary>
///// Converts this Trigger into a line of text that does not contain newlines.
///// </summary>
//public string Marshal()
//{
//    return ((int)triggeredBy).ToString() + "," + ((int)vk).ToString() + "," + ((int)modifiers).ToString();
//}
#endregion