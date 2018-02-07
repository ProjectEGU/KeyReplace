using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyReplace
{
    [Serializable]
    public class InputEvent
    {
        public InputEventType type;

        /// <summary>
        /// If this is a mouse input,  then vk is the MouseButton code
        /// If this is a keyboard input, then vk is the Vk.
        /// </summary>
        public int vk;
        public ModifierKey modifiers;

        public InputEvent(InputEventType type, int vk, ModifierKey modifiers)
        {
            this.type = type;
            this.vk = vk;
            this.modifiers = modifiers;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is InputEvent))
                return false;
            InputEvent a = (InputEvent)obj;
            return this.type == a.type && this.vk == a.vk && this.modifiers == a.modifiers;
        }
        public override int GetHashCode()
        {
            return vk + ((int)modifiers << 8) + ((int)type << 11);
        }
        public override string ToString()
        {
            return Macro.GetKeyString(vk, modifiers) + (type == InputEventType.KeyPress ? "" :  " " + type.ToString().Replace("Key", "").ToLower());
            //just display 'B press' as 'B', for example.
        }
    }
    public enum SpecialKey
    {
        OriginalKey = -2,
        MouseLeft = -3,
        MouseMid = -4,
        MouseRight = -5,
        MouseX1 = -6,
        MouseX2 = -7
    }
    public enum InputEventType
    {
        None,
        KeyDown,
        KeyUp,
        KeyPress,
        MouseDown,
        MouseUp
    }
}
