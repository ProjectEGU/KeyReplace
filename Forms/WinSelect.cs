using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyReplace
{
    public partial class WinSelect : Form
    {
        public WinSelect()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw, true);
        }
        public string ProcessName
        {
            get; set;
        }

        public Point BaseLocation
        {
            get;set;
        }
        Point Rloc;
        void DisplayOffset()
        {
            Rloc = DesktopLocation;
            Rloc.X -= BaseLocation.X;
            Rloc.Y -= BaseLocation.Y;
            Refresh();
        }
        Font def = new Font("Lucidas Sans", 9);
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (Width > 277 && Height > 145)
            {
                g.DrawString(@"Region selector
    Press [ENTER] or [Right-click] to accept
    Press [ESC] to cancel
    Press [Arrow keys] for fine movement
    Drag/resize this window over the area of the process using mouse
    ", def, Brushes.Black, new RectangleF(15, 15, Width - 30, Height - 30));
            }
            g.DrawString("Base: " + BaseLocation.ToString() + "\r\n" +
                    "Offset: " + Rloc.ToString() + "\r\n" +
                    "Size: " + Size.ToString(),
                    def, Brushes.Black, new PointF(15, Height - 50));
           
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        public Rectangle GetRect()
        {
            return new Rectangle(DesktopLocation.X - BaseLocation.X,
                DesktopLocation.Y - BaseLocation.Y,
                this.Width,
                this.Height);
        }
        Rectangle? tmpRect = null;
        public DialogResult ShowDialog(IEnumerable<string> procName, Rectangle? target)
        {
            if (procName != null)
                BaseLocation = WindowMonitor.GetWindowLocation(procName.ToArray()).GetValueOrDefault(Point.Empty);
            else BaseLocation = Point.Empty;
            tmpRect = target;
            var result = ShowDialog();


            return result;
        }
        private void WinSelect_Shown(object sender, EventArgs e)
        {
            if (tmpRect.HasValue && tmpRect.Value != Rectangle.Empty)
            {
                SetDesktopLocation(BaseLocation.X + tmpRect.Value.Location.X, BaseLocation.Y + tmpRect.Value.Location.Y);
                Size = tmpRect.Value.Size;

            }
            else
            {
                Size = new Size(347, 154);
            }


        }
        #region copypasta
        private void nub_KeyDown(object sender, KeyEventArgs e)
        {

                Point p = this.DesktopLocation;
                bool changed = false;
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        p.Offset(-1, 0);
                        changed = true;
                        break;
                    case Keys.Right:
                        p.Offset(1, 0);
                        changed = true;
                        break;
                    case Keys.Up:
                        p.Offset(0, -1);
                        changed = true;
                        break;
                    case Keys.Down:
                        p.Offset(0, 1);
                        changed = true;
                        break;
                    case Keys.Enter:
                        DialogResult = DialogResult.OK;
                        break;
                    case Keys.Escape:
                        DialogResult = DialogResult.Cancel;
                        break;
                    default:
                        break;
                }
                if (changed)
                {
                    this.DesktopLocation = p;

                }
        }

        private void WinSelect_Resize(object sender, EventArgs e)
        {
            DisplayOffset();
        }

        private void WinSelect_Move(object sender, EventArgs e)
        {
            DisplayOffset();
        }
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;


        #region Mouse Handling
        enum BorderHittest
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 4,
            Bottom = 8,
            TopLeft = Top | Left,
            TopRight = Top | Right,
            BottomLeft = Bottom | Left,
            BottomRight = Bottom | Right
        }

        //copypasta drag code from aku's answer:
        //https://stackoverflow.com/questions/30184/winforms-click-drag-anywhere-in-the-form-to-move-it-as-if-clicked-in-the-form
        private bool mouseDown;
        private Point lastPos;


        private const int WM_NCLBUTTONDBLCLK =
            0x00A3; //double click on a title bar a.k.a. non-client area of the form

        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        Point pos = Point.Empty;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseDown)
            {
                int xoffset = MousePosition.X - lastPos.X;
                int yoffset = MousePosition.Y - lastPos.Y;
                Left += xoffset;
                Top += yoffset;
                lastPos = MousePosition;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = false;
            }
            base.OnMouseUp(e);
        }
        const int BORDER_SZ = 15;
        ///
        /// Handling the window messages
        ///
        protected override void WndProc(ref Message m)
        {
            //134 = WM_NCACTIVATE
            if (m.Msg == 134)
            {
                //Check if other app is activating - if so, we do not close         
                if (m.LParam == IntPtr.Zero)
                {
                    if (m.WParam != IntPtr.Zero)
                    {
                        //Other form in our app has focus
                        //this.DialogResult = DialogResult.Cancel;
                    }

                }
            }

            if (m.Msg == WM_NCLBUTTONDBLCLK)
            {
                m.Result = IntPtr.Zero;
                return;
            }

            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
            {
                // message.Result = (IntPtr)HTBOTTOM;
                int x = (short)(m.LParam.ToInt32() & 0x0000FFFF);
                int y = (short)((m.LParam.ToInt32() & 0xFFFF0000) >> 16);
               Point pos = this.PointToClient(new Point(x, y));
              
                BorderHittest left = pos.X <= BORDER_SZ ? BorderHittest.Left : BorderHittest.None;
                BorderHittest right = pos.X >= Width - BORDER_SZ ? BorderHittest.Right : BorderHittest.None;
                BorderHittest top = pos.Y <= BORDER_SZ ? BorderHittest.Top : BorderHittest.None;
                BorderHittest bottom = pos.Y >= Height - BORDER_SZ ? BorderHittest.Bottom : BorderHittest.None;

                BorderHittest result = left | right | top | bottom; //Behold, the power of logical OR'ing!
                                                                    //DispText = left.ToString() + right.ToString() + top.ToString() + bottom.ToString();
                switch (result)
                {
                    case BorderHittest.Left:
                        m.Result = (IntPtr)HTLEFT;
                        break;
                    case BorderHittest.Right:
                        m.Result = (IntPtr)HTRIGHT;
                        break;
                    case BorderHittest.Top:
                        m.Result = (IntPtr)HTTOP;
                        break;
                    case BorderHittest.Bottom:
                        m.Result = (IntPtr)HTBOTTOM;
                        break;
                    case BorderHittest.TopLeft:
                        m.Result = (IntPtr)HTTOPLEFT;
                        break;
                    case BorderHittest.TopRight:
                        m.Result = (IntPtr)HTTOPRIGHT;
                        break;
                    case BorderHittest.BottomLeft:
                        m.Result = (IntPtr)HTBOTTOMLEFT;
                        break;
                    case BorderHittest.BottomRight:
                        m.Result = (IntPtr)HTBOTTOMRIGHT;
                        break;
                    default:
                        break;
                }
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                lastPos = MousePosition;
            }
            base.OnMouseDown(e);
        }

        #endregion

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(
            [MarshalAs(UnmanagedType.LPTStr)] string lpClassName,
            [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(
            IntPtr hWndChild,      // handle to window
            IntPtr hWndNewParent   // new parent window
        );
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x20;
        #endregion

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {

        }
        private void WinSelect_Deactivate(object sender, EventArgs e)
        {
          //  this.DialogResult = DialogResult.Cancel;
        }

        private void WinSelect_MouseDown(object sender, MouseEventArgs e)
        {
          // if (e.Button == MouseButtons.Right) this.DialogResult = DialogResult.OK;
        }
    }
}
