using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyReplace
{
    public enum TriggerType
    {
        KeyDown,
        KeyUp,
        MouseDown,
        MouseUp
    }

    public enum ModifierKey
    {
        None = 0,
        Ctrl = 1,
        Alt = 2,
        Shift = 4,
        CtrlAlt = Ctrl | Alt,
        ShiftCtrl = Ctrl | Shift,
        ShiftAlt = Alt | Shift,
        ShiftCtrlAlt = Ctrl | Shift | Alt
    }
}
