using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KeyReplace
{
    /// <summary>
    /// Monitor the foreground window process in the background, and provides fast lookup
    /// to its window rectangle and process name.
    /// </summary>
    public static class WindowMonitor
    {
        /// <summary>
        /// Gets the foreground process name, or null if there is none.
        /// </summary>
        public static string ForegroundProcessName { get { return _procName; } }
        public static int X { get { return _winRect.X; } }
        public static int Y { get { return _winRect.Y; } }

        /// <summary>
        /// Gets/sets the refresh delay of the background thread. If the value is lower than 50, it will be set to 50.
        /// </summary>
        public static int Refresh_delay
        {
            get { return refresh_delay; }

            set { refresh_delay = Math.Max(50, value); }
        }

        static string _procName = null; //ForegroundProcessName
        static IntPtr _hWnd = IntPtr.Zero; //ForegroundWindowHandle

        static Rectangle _winRect = Rectangle.Empty;

        static int refresh_delay = 200;

        static Thread t = null;
        static ThreadStart method = null;
        static object bufferlock = new object();
        static WindowMonitor()
        {
            method = delegate {
                RECT _tempRect; //ForegroundWindowRect

                IntPtr _temphWnd = IntPtr.Zero;
                IntPtr fg = IntPtr.Zero;
                string _tempName = null;
                uint pid = 0;
                try
                {
                    while (true)
                    {
                        fg = GetForegroundWindow();
                        if (fg != _temphWnd) //if the foreground window has changed, then we update name/hwnd variables
                        {
                            _temphWnd = fg; //set [current fg window handle] to the new fg window handle

                            GetWindowThreadProcessId(_temphWnd, out pid); //get the pid from the hwnd

                            _tempName = ProcessNameFromPid((int)pid); //get the [name] from the pid
                        }
                        //always update the rect variable
                        if (_tempName == null || !GetWindowRect(_temphWnd, out _tempRect))//get the [rectangle] from the fg window handle
                        {
                            lock (bufferlock)
                            {
                                _procName = _tempName;
                                _hWnd = IntPtr.Zero;
                                _winRect = Rectangle.Empty; //fail
                            }
                        }
                        else
                        {
                            lock (bufferlock)
                            {
                                _procName = _tempName;
                                _hWnd = _temphWnd;

                                _winRect.X = _tempRect.Left;//update the rectangle
                                _winRect.Y = _tempRect.Top;

                                _winRect.Width = _tempRect.Right - _tempRect.Left;
                                _winRect.Height = _tempRect.Bottom - _tempRect.Top;
                            }
                        }


                        Thread.Sleep(refresh_delay);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    Thread.Sleep(1500);
                }
            };
        }



        public static void BeginMonitorForeground()
        {
            if (t == null)
            {
                t = new Thread(method);
                t.Start();
            }
            else
            {
                throw new InvalidOperationException("Already monitoring foreground.");
            }
        }
        public static void StopMonitorForeground()
        {
            if (t != null)
            {
                t.Abort();
                t = null;
            }
            else
            {
                throw new InvalidOperationException("Was not monitoring foreground.");
            }
        }
        /// <summary>
        /// Gets the window location of the current active window, or whatever
        /// the last recorded position was.
        /// </summary>
        /// <returns></returns>
        public static Point GetWindowLocation()
        {
            return new Point(_winRect.Left, _winRect.Top);
        }
        /// <summary>
        /// Checks if the point is within the specified region in the window
        /// </summary>
        /// <param name="procName">The foreground process name</param>
        /// <param name="x">The screen x coordinate of the point to check</param>
        /// <param name="y">The screen y coordinate of the point to check</param>
        /// <param name="target">The rectangle to check, relative to the top-left of the window rect.</param>
        /// <returns></returns>
        public static bool CheckPoint(string procName, int x, int y, Rectangle target)
        {
            lock (bufferlock)
            {
                return _procName.Equals(procName) && _hWnd != IntPtr.Zero &&
                    target.Contains(x - _winRect.Left, y - _winRect.Top);
            }
        }
        /// <summary>
        /// Returns a bool indicating whether or not the foreground process is any of the elements in procName.
        /// </summary>
        public static bool IsForeground(IEnumerable<string> procNames)
        {
            lock (bufferlock)
            {
                return procNames != null && procNames.Contains(_procName);
            }
        }

        /// <summary>
        /// Returns a bool indicating whether or not the foreground process name is procName.
        /// </summary>
        public static bool IsForeground(string procName)
        {
            lock (bufferlock)
            {
                return _procName != null && _procName.Equals(procName); // && _hWnd == GetForegroundWindow();
            }
        }
        /// <summary>
        /// Returns the foreground rectangle, or Rectangle.Empty if it is invalid.
        /// </summary>
        public static Rectangle GetForegroundRect()
        {
            lock (bufferlock)
            {
                return _winRect;
            }
        }

        /// <summary>
        /// Gets the window location of exactly one process that has a name matching procName.
        /// 
        /// Otherwise, this will return null!
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public static Point? GetWindowLocation(params string[] procName)
        {
            var processes = Process.GetProcesses()
                .Where(proc => procName.Contains(proc.ProcessName,StringComparer.OrdinalIgnoreCase));

            if (processes.Count() == 1)
            {
                RECT tmp = new RECT();
                GetWindowRect(processes.Single().MainWindowHandle, out tmp);
                return new Point(tmp.Left, tmp.Top);
            }
            else return null;

        }

        //faster than getprocessbyid! takes about 3ms
        static string ProcessNameFromPid(int pid)
        {
            return Process.GetProcesses()
                .Where(p => p.Id == pid)
                .Select(p => p.ProcessName)
                .DefaultIfEmpty(null)
                .FirstOrDefault();
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        [PreserveSig]
        public static extern uint GetModuleFileName
(
    [In]
    IntPtr hModule,

    [Out]
    StringBuilder lpFilename,

    [In]
    [MarshalAs(UnmanagedType.U4)]
    int nSize
);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
     ProcessAccessFlags processAccess,
     bool bInheritHandle,
     int processId
);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("psapi.dll")]
        public static extern uint GetProcessImageFileName(IntPtr hProcess,
            [Out] StringBuilder lpImageFileName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }


    }

}
