using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyReplace
{
    public partial class frmOSD : Form
    {
        //clickthru too??
        Settings settings = Settings.Instance;

        string _followTarget = "";
        bool _locked = false;
        public bool Locked
        {
            set
            {
                _locked = value;
                follower.Enabled = Locked;
                if (!value) this.TopMost = true;
            }
            get
            {
                return _locked;
            }
        }

        public string FollowTarget
        {
            set
            {
                _followTarget = value;
            }
        }

        public frmOSD()
        {
            InitializeComponent();
        }
        private bool mouseDown;
        private Point lastPos;


        Point pos = Point.Empty;

        private void frmOSD_MouseDown(object sender, MouseEventArgs e)
        {
            if (_locked) return;
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                lastPos = MousePosition;
            }
        }

        private void frmOSD_MouseMove(object sender, MouseEventArgs e)
        {
            if (_locked) return;
            if (mouseDown)
            {
                int xoffset = MousePosition.X - lastPos.X;
                int yoffset = MousePosition.Y - lastPos.Y;
                Left += xoffset;
                Top += yoffset;
                lastPos = MousePosition;
            }
        }

        private void frmOSD_MouseUp(object sender, MouseEventArgs e)
        {
            if (_locked) return;
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = false;
                Point? _targetPos = WindowMonitor.GetWindowLocation(_followTarget);
                if (_targetPos.HasValue)
                {
                    settings._opOsdPoint = new Point(
                        this.DesktopLocation.X - _targetPos.Value.X,
                        this.DesktopLocation.Y - _targetPos.Value.Y);
                }
            }
            
        }

        public void UpdateDisplay(string text, Color clrBg, Color clrTextBg, Color clrTextFg)
        {
            label1.Text = text;
            label1.BackColor = clrTextBg;
            BackColor = clrBg;
            label1.ForeColor = clrTextFg;
        }
        Point _newDesktopLoc;
        Rectangle _fgRect;
        private void follower_Tick(object sender, EventArgs e)
        {
            txtDebug.Text = WindowMonitor.ForegroundProcessName+" "+_followTarget;
            if (WindowMonitor.IsForeground(_followTarget))
            {
               
                if (!this.TopMost) this.TopMost = true;
                _fgRect = WindowMonitor.GetForegroundRect();
                _newDesktopLoc.X = _fgRect.X + settings._opOsdPoint.X;
                _newDesktopLoc.Y = _fgRect.Y + settings._opOsdPoint.Y;

                if (_newDesktopLoc != this.DesktopLocation) 
                    this.SetDesktopLocation(_newDesktopLoc.X, _newDesktopLoc.Y);
            }
            else
            {
                if (this.TopMost) this.TopMost = false;
            }
        }
    }
}
