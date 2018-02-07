using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyReplace
{
    /// <summary>
    /// A helper class for building a list of <see cref="INPUT"/> messages ready to be sent to the native Windows API.
    /// </summary>
    public class InputBuilder : IEnumerable<INPUT>
    {
        /// <summary>
        /// The public list of <see cref="INPUT"/> messages being built by this instance.
        /// </summary>
        private readonly List<INPUT> _inputList;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBuilder"/> class.
        /// </summary>
        public InputBuilder()
        {
            _inputList = new List<INPUT>();
        }

        /// <summary>
        /// Returns the list of <see cref="INPUT"/> messages as a <see cref="System.Array"/> of <see cref="INPUT"/> messages.
        /// </summary>
        /// <returns>The <see cref="System.Array"/> of <see cref="INPUT"/> messages.</returns>
        public INPUT[] ToArray()
        {
            return _inputList.ToArray();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list of <see cref="INPUT"/> messages.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the list of <see cref="INPUT"/> messages.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<INPUT> GetEnumerator()
        {
            return _inputList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list of <see cref="INPUT"/> messages.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the list of <see cref="INPUT"/> messages.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the <see cref="INPUT"/> at the specified position.
        /// </summary>
        /// <value>The <see cref="INPUT"/> message at the specified position.</value>
        public INPUT this[int position]
        {
            get
            {
                return _inputList[position];
            }
        }

        /// <summary>
        /// Determines if the <see cref="Keys"/> is an ExtendedKey
        /// </summary>
        /// <param name="keyCode">The key code.</param>
        /// <returns>true if the key code is an extended key; otherwise, false.</returns>
        /// <remarks>
        /// The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP, PAGE DOWN, and arrow keys in the clusters to the left of the numeric keypad; the NUM LOCK key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in the numeric keypad.
        /// 
        /// See http://msdn.microsoft.com/en-us/library/ms646267(v=vs.85).aspx Section "Extended-Key Flag"
        /// </remarks>
        public static bool IsExtendedKey(Keys keyCode)
        {
            if (keyCode == Keys.Menu ||
                keyCode == Keys.LMenu ||
                keyCode == Keys.RMenu ||
                keyCode == Keys.Control ||
                keyCode == Keys.RControlKey ||
                keyCode == Keys.Insert ||
                keyCode == Keys.Delete ||
                keyCode == Keys.Home ||
                keyCode == Keys.End ||
                keyCode == Keys.Prior ||
                keyCode == Keys.Next ||
                keyCode == Keys.Right ||
                keyCode == Keys.Up ||
                keyCode == Keys.Left ||
                keyCode == Keys.Down ||
                keyCode == Keys.NumLock ||
                keyCode == Keys.Cancel ||
                keyCode == Keys.Snapshot ||
                keyCode == Keys.Divide)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a key down to the list of <see cref="INPUT"/> messages.
        /// </summary>
        /// <param name="keyCode">The <see cref="Keys"/>.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddKeyDown(Keys keyCode)
        {
            var down =
                new INPUT
                {
                    Type = (UInt32)InputType.Keyboard,
                    Data =
                            {
                                Keyboard =
                                    new KEYBDINPUT
                                        {
                                            KeyCode = (UInt16) keyCode,
                                            Scan = (ushort)MapVirtualKey((uint)keyCode,MapVirtualKeyMapTypes.MAPVK_VK_TO_VSC),
                                            Flags = IsExtendedKey(keyCode) ? (UInt32) KeyboardFlag.ExtendedKey : 0,
                                            Time = (uint)(Environment.TickCount + _inputList.Count*25),
                                            ExtraInfo = IntPtr.Zero
                                        }
                            }
                };

            _inputList.Add(down);
            return this;
        }

        /// <summary>
        /// Adds a key up to the list of <see cref="INPUT"/> messages.
        /// </summary>
        /// <param name="keyCode">The <see cref="Keys"/>.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddKeyUp(Keys keyCode)
        {
            var up =
                new INPUT
                {
                    Type = (UInt32)InputType.Keyboard,
                    Data =
                            {
                                Keyboard =
                                    new KEYBDINPUT
                                        {
                                            KeyCode = (UInt16) keyCode,
                                            Scan = (ushort)MapVirtualKey((uint)keyCode,MapVirtualKeyMapTypes.MAPVK_VK_TO_VSC),
                                            Flags = (UInt32) (IsExtendedKey(keyCode)
                                                                  ? KeyboardFlag.KeyUp | KeyboardFlag.ExtendedKey
                                                                  : KeyboardFlag.KeyUp),
                                            Time = (uint)(Environment.TickCount + _inputList.Count*25),
                                            ExtraInfo = IntPtr.Zero
                                        }
                            }
                };

            _inputList.Add(up);
            return this;
        }

        /// <summary>
        /// Adds a mouse button down for the specified button.
        /// </summary>
        /// <param name="button"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseButtonDown(MouseButtons button)
        {
            var buttonDown = new INPUT { Type = (UInt32)InputType.Mouse };
            buttonDown.Data.Mouse.X = buttonDown.Data.Mouse.Y = 0;
            buttonDown.Data.Mouse.Flags = (UInt32)ToMouseButtonDownFlag(button);
            buttonDown.Data.Mouse.MouseData = ToXData(button);
            _inputList.Add(buttonDown);

            return this;
        }


        /// <summary>
        /// Adds a mouse button up for the specified button.
        /// </summary>
        /// <param name="button"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseButtonUp(MouseButtons button)
        {
            var buttonUp = new INPUT { Type = (UInt32)InputType.Mouse };
            buttonUp.Data.Mouse.X = buttonUp.Data.Mouse.Y = 0;
            buttonUp.Data.Mouse.Flags = (UInt32)ToMouseButtonUpFlag(button);
            buttonUp.Data.Mouse.MouseData = ToXData(button);
            _inputList.Add(buttonUp);

            return this;
        }
        uint ToXData(MouseButtons m)//hey why not put this in a dict idiot.
        {
            if(m==MouseButtons.XButton1 || m == MouseButtons.XButton2)
            {
                return (uint)(m == MouseButtons.XButton1 ? 1 : 2);
            }
            return 0;
        }
        MouseFlag ToMouseButtonDownFlag(MouseButtons m)//hey why not put this in a dict idiot.
        {
            switch (m)
            {
                case MouseButtons.Left:
                    return MouseFlag.LeftDown;
                case MouseButtons.Right:
                    return MouseFlag.RightDown;
                case MouseButtons.Middle:
                    return MouseFlag.MiddleDown;
                case MouseButtons.XButton1:
                    return MouseFlag.XDown;
                case MouseButtons.XButton2:
                    return MouseFlag.XDown;
                default:
                    throw new ArgumentException("Invalid MouseButton");
            }
        }
        MouseFlag ToMouseButtonUpFlag(MouseButtons m)//hey why not put this in a dict idiot.
        {

            switch (m)
            {
                case MouseButtons.Left:
                    return MouseFlag.LeftUp;
                case MouseButtons.Right:
                    return MouseFlag.RightUp;
                case MouseButtons.Middle:
                    return MouseFlag.MiddleUp;
                case MouseButtons.XButton1:
                    return MouseFlag.XUp;
                case MouseButtons.XButton2:
                    return MouseFlag.XUp;
                default:
                    throw new ArgumentException("Invalid MouseButton");
            }
        }

        /// <summary>
        /// The set of MouseFlags for use in the Flags property of the <see cref="MOUSEINPUT"/> structure. (See: http://msdn.microsoft.com/en-us/library/ms646273(VS.85).aspx)
        /// </summary>
        [Flags]
        internal enum MouseFlag : uint // UInt32
        {
            /// <summary>
            /// Specifies that movement occurred.
            /// </summary>
            Move = 0x0001,

            /// <summary>
            /// Specifies that the left button was pressed.
            /// </summary>
            LeftDown = 0x0002,

            /// <summary>
            /// Specifies that the left button was released.
            /// </summary>
            LeftUp = 0x0004,

            /// <summary>
            /// Specifies that the right button was pressed.
            /// </summary>
            RightDown = 0x0008,

            /// <summary>
            /// Specifies that the right button was released.
            /// </summary>
            RightUp = 0x0010,

            /// <summary>
            /// Specifies that the middle button was pressed.
            /// </summary>
            MiddleDown = 0x0020,

            /// <summary>
            /// Specifies that the middle button was released.
            /// </summary>
            MiddleUp = 0x0040,

            /// <summary>
            /// Windows 2000/XP: Specifies that an X button was pressed.
            /// </summary>
            XDown = 0x0080,

            /// <summary>
            /// Windows 2000/XP: Specifies that an X button was released.
            /// </summary>
            XUp = 0x0100,

            /// <summary>
            /// Windows NT/2000/XP: Specifies that the wheel was moved, if the mouse has a wheel. The amount of movement is specified in mouseData. 
            /// </summary>
            VerticalWheel = 0x0800,

            /// <summary>
            /// Specifies that the wheel was moved horizontally, if the mouse has a wheel. The amount of movement is specified in mouseData. Windows 2000/XP:  Not supported.
            /// </summary>
            HorizontalWheel = 0x1000,

            /// <summary>
            /// Windows 2000/XP: Maps coordinates to the entire desktop. Must be used with MOUSEEVENTF_ABSOLUTE.
            /// </summary>
            VirtualDesk = 0x4000,

            /// <summary>
            /// Specifies that the dx and dy members contain normalized absolute coordinates. If the flag is not set, dxand dy contain relative data (the change in position since the last reported position). This flag can be set, or not set, regardless of what kind of mouse or other pointing device, if any, is connected to the system. For further information about relative mouse motion, see the following Remarks section.
            /// </summary>
            Absolute = 0x8000,
        }


        /// <summary>
        /// The MapVirtualKey function translates (maps) a virtual-key code into a scan
        /// code or character value, or translates a scan code into a virtual-key code    
        /// </summary>
        /// <param name="uCode">[in] Specifies the virtual-key code or scan code for a key.
        /// How this value is interpreted depends on the value of the uMapType parameter
        /// </param>
        /// <param name="uMapType">[in] Specifies the translation to perform. The value of this
        /// parameter depends on the value of the uCode parameter.
        /// </param>
        /// <returns>Either a scan code, a virtual-key code, or a character value, depending on
        /// the value of uCode and uMapType. If there is no translation, the return value is zero
        /// </returns>
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapVirtualKeyMapTypes uMapType);
        /// <summary>
        /// The set of valid MapTypes used in MapVirtualKey
        /// </summary>
        public enum MapVirtualKeyMapTypes : uint
        {
            /// <summary>
            /// uCode is a virtual-key code and is translated into a scan code.
            /// If it is a virtual-key code that does not distinguish between left- and
            /// right-hand keys, the left-hand scan code is returned.
            /// If there is no translation, the function returns 0.
            /// </summary>
            MAPVK_VK_TO_VSC = 0x00,

            /// <summary>
            /// uCode is a scan code and is translated into a virtual-key code that
            /// does not distinguish between left- and right-hand keys. If there is no
            /// translation, the function returns 0.
            /// </summary>
            MAPVK_VSC_TO_VK = 0x01,

            /// <summary>
            /// uCode is a virtual-key code and is translated into an unshifted
            /// character value in the low-order word of the return value. Dead keys (diacritics)
            /// are indicated by setting the top bit of the return value. If there is no
            /// translation, the function returns 0.
            /// </summary>
            MAPVK_VK_TO_CHAR = 0x02,

            /// <summary>
            /// Windows NT/2000/XP: uCode is a scan code and is translated into a
            /// virtual-key code that distinguishes between left- and right-hand keys. If
            /// there is no translation, the function returns 0.
            /// </summary>
            MAPVK_VSC_TO_VK_EX = 0x03,

            /// <summary>
            /// Not currently documented
            /// </summary>
            MAPVK_VK_TO_VSC_EX = 0x04
        }

    }
  
}
#region Unused Code

///// <summary>
///// Adds a key press to the list of <see cref="INPUT"/> messages which is equivalent to a key down followed by a key up.
///// </summary>
///// <param name="keyCode">The <see cref="Keys"/>.</param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddKeyPress(Keys keyCode)
//{
//    AddKeyDown(keyCode);
//    AddKeyUp(keyCode);
//    return this;
//}

///// <summary>
///// Adds the character to the list of <see cref="INPUT"/> messages.
///// </summary>
///// <param name="character">The <see cref="System.Char"/> to be added to the list of <see cref="INPUT"/> messages.</param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddCharacter(char character)
//{
//    UInt16 scanCode = character;

//    var down = new INPUT
//    {
//        Type = (UInt32)InputType.Keyboard,
//        Data =
//                                   {
//                                       Keyboard =
//                                           new KEYBDINPUT
//                                               {
//                                                   KeyCode = 0,
//                                                   Scan = scanCode,
//                                                   Flags = (UInt32)KeyboardFlag.Unicode,
//                                                   Time = 0,
//                                                   ExtraInfo = IntPtr.Zero
//                                               }
//                                   }
//    };

//    var up = new INPUT
//    {
//        Type = (UInt32)InputType.Keyboard,
//        Data =
//                                 {
//                                     Keyboard =
//                                         new KEYBDINPUT
//                                             {
//                                                 KeyCode = 0,
//                                                 Scan = scanCode,
//                                                 Flags =
//                                                     (UInt32)(KeyboardFlag.KeyUp | KeyboardFlag.Unicode),
//                                                 Time = 0,
//                                                 ExtraInfo = IntPtr.Zero
//                                             }
//                                 }
//    };

//    // Handle extended keys:
//    // If the scan code is preceded by a prefix byte that has the value 0xE0 (224),
//    // we need to include the KEYEVENTF_EXTENDEDKEY flag in the Flags property. 
//    if ((scanCode & 0xFF00) == 0xE000)
//    {
//        down.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.ExtendedKey;
//        up.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.ExtendedKey;
//    }

//    _inputList.Add(down);
//    _inputList.Add(up);
//    return this;
//}

///// <summary>
///// Adds all of the characters in the specified <see cref="IEnumerable{T}"/> of <see cref="char"/>.
///// </summary>
///// <param name="characters">The characters to add.</param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddCharacters(IEnumerable<char> characters)
//{
//    foreach (var character in characters)
//    {
//        AddCharacter(character);
//    }
//    return this;
//}

///// <summary>
///// Adds the characters in the specified <see cref="string"/>.
///// </summary>
///// <param name="characters">The string of <see cref="char"/> to add.</param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddCharacters(string characters)
//{
//    return AddCharacters(characters.ToCharArray());
//}

///// <summary>
///// Moves the mouse relative to its current position.
///// </summary>
///// <param name="x"></param>
///// <param name="y"></param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddRelativeMouseMovement(int x, int y)
//{
//    var movement = new INPUT { Type = (UInt32)InputType.Mouse };
//    movement.Data.Mouse.Flags = (UInt32)MouseFlag.Move;
//    movement.Data.Mouse.X = x;
//    movement.Data.Mouse.Y = y;

//    _inputList.Add(movement);

//    return this;
//}

///// <summary>
///// Move the mouse to an absolute position.
///// </summary>
///// <param name="absoluteX"></param>
///// <param name="absoluteY"></param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddAbsoluteMouseMovement(int absoluteX, int absoluteY)
//{
//    var movement = new INPUT { Type = (UInt32)InputType.Mouse };
//    movement.Data.Mouse.Flags = (UInt32)(MouseFlag.Move | MouseFlag.Absolute);
//    movement.Data.Mouse.X = absoluteX;
//    movement.Data.Mouse.Y = absoluteY;

//    _inputList.Add(movement);

//    return this;
//}

///// <summary>
///// Move the mouse to the absolute position on the virtual desktop.
///// </summary>
///// <param name="absoluteX"></param>
///// <param name="absoluteY"></param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddAbsoluteMouseMovementOnVirtualDesktop(int absoluteX, int absoluteY)
//{
//    var movement = new INPUT { Type = (UInt32)InputType.Mouse };
//    movement.Data.Mouse.Flags = (UInt32)(MouseFlag.Move | MouseFlag.Absolute | MouseFlag.VirtualDesk);
//    movement.Data.Mouse.X = absoluteX;
//    movement.Data.Mouse.Y = absoluteY;

//    _inputList.Add(movement);

//    return this;
//}

///// <summary>
///// Adds a single click of the specified button.
///// </summary>
///// <param name="button"></param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddMouseButtonClick(MouseButton button)
//{
//    return AddMouseButtonDown(button).AddMouseButtonUp(button);
//}

///// <summary>
///// Adds a single click of the specified button.
///// </summary>
///// <param name="xButtonId"></param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddMouseXButtonClick(int xButtonId)
//{
//    return AddMouseXButtonDown(xButtonId).AddMouseXButtonUp(xButtonId);
//}

///// <summary>
///// Adds a double click of the specified button.
///// </summary>
///// <param name="button"></param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddMouseButtonDoubleClick(MouseButton button)
//{
//    return AddMouseButtonClick(button).AddMouseButtonClick(button);
//}

///// <summary>
///// Adds a double click of the specified button.
///// </summary>
///// <param name="xButtonId"></param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddMouseXButtonDoubleClick(int xButtonId)
//{
//    return AddMouseXButtonClick(xButtonId).AddMouseXButtonClick(xButtonId);
//}

///// <summary>
///// Scroll the vertical mouse wheel by the specified amount.
///// </summary>
///// <param name="scrollAmount"></param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddMouseVerticalWheelScroll(int scrollAmount)
//{
//    var scroll = new INPUT { Type = (UInt32)InputType.Mouse };
//    scroll.Data.Mouse.Flags = (UInt32)MouseFlag.VerticalWheel;
//    scroll.Data.Mouse.MouseData = (UInt32)scrollAmount;

//    _inputList.Add(scroll);

//    return this;
//}

///// <summary>
///// Scroll the horizontal mouse wheel by the specified amount.
///// </summary>
///// <param name="scrollAmount"></param>
///// <returns>This <see cref="InputBuilder"/> instance.</returns>
//public InputBuilder AddMouseHorizontalWheelScroll(int scrollAmount)
//{
//    var scroll = new INPUT { Type = (UInt32)InputType.Mouse };
//    scroll.Data.Mouse.Flags = (UInt32)MouseFlag.HorizontalWheel;
//    scroll.Data.Mouse.MouseData = (UInt32)scrollAmount;

//    _inputList.Add(scroll);

//    return this;
//}

//private static MouseFlag ToMouseButtonDownFlag(MouseButton button)
//{
//    switch (button)
//    {
//        case MouseButton.LeftButton:
//            return MouseFlag.LeftDown;

//        case MouseButton.MiddleButton:
//            return MouseFlag.MiddleDown;

//        case MouseButton.RightButton:
//            return MouseFlag.RightDown;

//        default:
//            return MouseFlag.LeftDown;
//    }
//}

//private static MouseFlag ToMouseButtonUpFlag(MouseButton button)
//{
//    switch (button)
//    {
//        case MouseButton.LeftButton:
//            return MouseFlag.LeftUp;

//        case MouseButton.MiddleButton:
//            return MouseFlag.MiddleUp;

//        case MouseButton.RightButton:
//            return MouseFlag.RightUp;

//        default:
//            return MouseFlag.LeftUp;
//    }
//}

#endregion