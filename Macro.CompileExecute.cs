using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyReplace
{
    public partial class Macro
    {
        /// <summary>
        /// We removed modifier functionality as of 2017-11-05, 6:50PM
        /// We leave this reponsibility up to the user.
        /// </summary>
        List<InputEvent> _events = new List<InputEvent>();
        
        public CompiledMacro Compile(Trigger trig)
        {
            //we commented the line below to allow for blank macros now. they will block input and maybe be used to change pages.
           // if (_triggers.Count == 0) throw new Exception("This macro has no triggers.");
            // check if trigger is in triggerset maybe
            InputBuilder magie = new InputBuilder();
            bool origKeyDown = trig.triggeredBy == TriggerType.KeyUp;
            foreach (InputEvent evt in _events)
            {
                int vk = evt.vk;
                if (vk == (int)SpecialKey.OriginalKey)
                {
                    //do not attempt to press a mouse key.
                    if (trig.triggeredBy == TriggerType.MouseDown ||
                        trig.triggeredBy == TriggerType.MouseUp)
                        continue;

                    vk = trig.vk;
                    origKeyDown = evt.type == InputEventType.KeyDown;
                }
                switch (evt.type)
                {
                    case InputEventType.KeyDown:
                        magie.AddKeyDown((Keys)vk);
                        break;
                    case InputEventType.KeyUp:
                        magie.AddKeyUp((Keys)vk);
                        break;
                    case InputEventType.KeyPress:
                        magie.AddKeyDown((Keys)vk);
                        magie.AddKeyUp((Keys)vk);
                        break;
                    default:
                        throw new Exception("Unsupported input type: " + evt.type.ToString());
                }
            }
            return new CompiledMacro(magie.ToArray(), origKeyDown, this.HasTableSwitch ? this.TableSwitchName : string.Empty, this.PassOriginalKey);
        }
    }
}
#region Code Grave
/*bool _shiftDown = false;
bool _altDown = false;
bool _ctrlDown = false;*/

/**
*         public void AddKeyDown(Keys keyCode, ModifierKey modifiers)
{

         _shiftDown |= IsShift((int)keyCode);
_altDown |= IsAlt((int)keyCode);
_ctrlDown |= IsCtrl((int)keyCode);

//make the current key state match the modifiers.

_curModifier = modifiers;
}




public new void AddKeyUp(Keys keyCode)
{
_shiftDown &= !IsShift((int)keyCode);
_altDown &= !IsAlt((int)keyCode);
_ctrlDown &= !IsCtrl((int)keyCode);

base.AddKeyUp(keyCode);
}





* */
#endregion