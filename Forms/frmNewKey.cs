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
    public partial class frmNewKey : Form
    {
        public frmNewKey()
        {
            InitializeComponent();
        }

        public bool IsInput { get; internal set; }
        public Rectangle WinRect { get; private set; }

        public ModifierKey Modifiers {
            get {
                return _modifier;
            }
        }
        public int Vk {
            get
            {
                if (chkOrigKey.Checked)
                    return (int)SpecialKey.OriginalKey;
                else return _vk;
            }
        }
        bool _hasValue = false;

        public new void ShowDialog()
        {
            this.SuspendLayout();


            setKeyState(false, false, false, 10);

            _hasValue = false;
            this.Text = IsInput ? "Macro Edit" : "Trigger Edit";

            cmbMouseEventType.SelectedIndex = cmbMouseTriggerButton.SelectedIndex = 0;
            maintabs.TabPages.Clear();
            btnRegion.Enabled = chkRegion.Checked = false;
            if (IsInput)
            {

                maintabs.TabPages.Add(keyInput);
            }
            else
            {
             
                maintabs.TabPages.Add(keyTrigger);
                maintabs.TabPages.Add(mouse_Trigger);
            }

            chkOrigKey.Checked = false;

            this.ResumeLayout(true);
            cmbProcessKey.Text = cmbProcessMouse.Text = "<Any>";
           txtKeyInt.Text = txtKeyInt2.Text = "Press any key.";
            txtMouseSubregion.Text = "No subregion selected.";
            base.ShowDialog();
        }

        public void ShowDialog(Trigger t)
        {
            this.SuspendLayout();
            this.Text = IsInput ? "Macro Edit" : "Trigger Edit";

            cmbMouseEventType.SelectedIndex = cmbMouseTriggerButton.SelectedIndex = 0;
            maintabs.TabPages.Clear();

            maintabs.TabPages.Add(keyTrigger);
            maintabs.TabPages.Add(mouse_Trigger);
            this.ResumeLayout(true);
            switch (t.triggeredBy)
            {
                case TriggerType.KeyDown:
                    maintabs.SelectedTab = keyTrigger;
                    chkKeyDown2.Checked = true;
                    setKeyState(t.modifiers, t.vk);
                    chkOrigKey.Checked = t.vk == (int)SpecialKey.OriginalKey;
                    break;
                case TriggerType.KeyUp:
                    maintabs.SelectedTab = keyTrigger;
                    chkKeyUp2.Checked = true;
                    setKeyState(t.modifiers, t.vk);
                    chkOrigKey.Checked = t.vk == (int)SpecialKey.OriginalKey;
                    break;
                case TriggerType.MouseDown:
                    maintabs.SelectedTab = mouse_Trigger;
                    cmbMouseTriggerButton.SelectedIndex = Array.IndexOf(mbtn, (MouseButtons)t.vk);
                    cmbMouseEventType.SelectedIndex = 0;
                    break;
                case TriggerType.MouseUp:
                    maintabs.SelectedTab = mouse_Trigger;
                    cmbMouseTriggerButton.SelectedIndex = Array.IndexOf(mbtn, (MouseButtons)t.vk);
                    cmbMouseEventType.SelectedIndex = 1;
                    break;
                default:
                    break;
            }

            WinRect = t.winRect;
            if (t.winRect == Rectangle.Empty)
            {
                chkRegion.Checked = btnRegion.Enabled = false;
                txtMouseSubregion.Text = "No subregion selected.";
            } else
            {
         
                txtMouseSubregion.Text = t.winRect.ToString();
                chkRegion.Checked = btnRegion.Enabled = true;
            }

   
            _tmpProcName = ((t.processName == null || !t.processName.Any()) ? "<Any>" :
                string.Join("|", t.processName));
    
            base.ShowDialog();
        }
        public void ShowDialog(InputEvent t)
        {
            this.SuspendLayout();
            this.Text = IsInput ? "Macro Edit" : "Trigger Edit";

            cmbMouseEventType.SelectedIndex = cmbMouseTriggerButton.SelectedIndex = 0;

            maintabs.TabPages.Clear();
            maintabs.TabPages.Add(keyInput);

            setKeyState(t.modifiers, t.vk);
            

            switch (t.type)
            {
                case InputEventType.KeyDown:
                    chkKeyDown.Checked = true;
                    break;
                case InputEventType.KeyUp:
                    chkKeyUp.Checked = true;
                    break;
                case InputEventType.KeyPress:
                    chkKeyPress.Checked = true;
                    break;
                default:
                    break;
            }

            this.ResumeLayout();

            chkOrigKey.Checked = t.vk == (int)SpecialKey.OriginalKey;

            base.ShowDialog();
        }
        string _tmpProcName = null;
        private void frmNewKey_Shown(object sender, EventArgs e)
        {
            cmbProcessKey.Text = cmbProcessMouse.Text = _tmpProcName;//set process name of macro

            //draw input to the textbox so the user can type keys right away
            if (IsInput) txtKeyInt.Focus();
            else txtKeyInt2.Focus();

            txtKeyInt.Select(0, 0);//hides the selection
            txtKeyInt2.Select(0, 0);

            textBox1_TextChanged(null, EventArgs.Empty);//hide their caret
        }
        static MouseButtons[] mbtn = new MouseButtons[] {MouseButtons.Left, MouseButtons.Right,MouseButtons.Middle,MouseButtons.XButton1,MouseButtons.XButton2 };

        public Trigger GetTrigger()
        {
            if (maintabs.SelectedTab == mouse_Trigger)
            {
                if(string.IsNullOrEmpty(cmbProcessMouse.Text) || cmbProcessMouse.Text.Equals("<any>",StringComparison.CurrentCultureIgnoreCase))
                {
                    return new Trigger(
                        cmbMouseEventType.SelectedIndex == 0 ? TriggerType.MouseDown : TriggerType.MouseUp,
                        (int)mbtn[cmbMouseTriggerButton.SelectedIndex],
                        ModifierKey.None,
                        new string[0], chkRegion.Checked ? WinRect : Rectangle.Empty);
                } else
                {
                    return new Trigger(
                        cmbMouseEventType.SelectedIndex == 0 ? TriggerType.MouseDown : TriggerType.MouseUp,
                        (int)mbtn[cmbMouseTriggerButton.SelectedIndex],
                        ModifierKey.None,
                        cmbProcessMouse.Text.Split('|'), WinRect);
                }
            }
            else if (maintabs.SelectedTab == keyTrigger)
            {

                    if(string.IsNullOrEmpty(cmbProcessKey.Text) || cmbProcessKey.Text.Equals("<any>",StringComparison.OrdinalIgnoreCase))
                    {
                        return new Trigger(
                            chkKeyDown2.Checked ? TriggerType.KeyDown : TriggerType.KeyUp,
                            Vk, Modifiers, new string[0], Rectangle.Empty);
                    } else
                    {
                        return new Trigger(
                            chkKeyDown2.Checked ? TriggerType.KeyDown : TriggerType.KeyUp,
                            Vk, Modifiers, cmbProcessKey.Text.Split('|'), WinRect);
                    }
            }
            else throw new Exception("Idk what tab u selected");
        }
        public InputEvent GetInputEvent()
        {
            return new InputEvent(GetKeyInputEventType(), Vk, Modifiers);
        }

        public TriggerType GetKeyTrigger()
        {
            if (chkKeyDown.Checked)
                return TriggerType.KeyDown;
            else if (chkKeyUp.Checked)
                return TriggerType.KeyUp;
            else throw new Exception();
        }

        public InputEventType GetKeyInputEventType()
        {
            if (chkKeyDown.Checked)
                return InputEventType.KeyDown;
            else if (chkKeyUp.Checked)
                return InputEventType.KeyUp;
            else if (chkKeyPress.Checked)
                return InputEventType.KeyPress;
            else throw new Exception();
      
        }

        ModifierKey _modifier;
        int _vk;

        private void frmNewKey_KeyDown(object sender, KeyEventArgs e)
        {
            int vk = e.KeyValue;
            if (Macro.IsShift(vk)) //ah man idk if hookcallback can sometimes return the actual shiftkey vk
                vk = Macro.IsKeyPushedDown(Keys.LShiftKey) ? (int)Keys.LShiftKey : (int)Keys.RShiftKey;
            else if (Macro.IsCtrl(vk))
                vk = Macro.IsKeyPushedDown(Keys.LControlKey) ? (int)Keys.LControlKey : (int)Keys.RControlKey;
            else if (Macro.IsAlt(vk))
                vk = Macro.IsKeyPushedDown(Keys.LMenu) ? (int)Keys.LMenu : (int)Keys.RMenu;

            setKeyState(e.Control, e.Alt, e.Shift, vk);
            e.SuppressKeyPress = true;
            
            //txtKeyInt.Text = e.KeyValue.ToString();
        }

        private void frmNewKey_KeyUp(object sender, KeyEventArgs e)
        {

        }

        void setKeyState(ModifierKey modifier, int keyValue)
        {
            setKeyState((modifier & ModifierKey.Ctrl )!= 0,
               ( modifier & ModifierKey.Alt) != 0,
               ( modifier & ModifierKey.Shift) != 0, keyValue
                );
        }

        private void setKeyState(bool CTRL, bool ALT, bool SHIFT,  int keyValue)
        {
            txtCTRL2.BackColor = txtCTRL.BackColor = CTRL ? Color.Lime : SystemColors.Control;
            txtALT2.BackColor = txtALT.BackColor = ALT ? Color.Lime : SystemColors.Control;
            txtSHIFT2.BackColor = txtSHIFT.BackColor = SHIFT ? Color.Lime : SystemColors.Control;

            _modifier = Macro.GetModifierKeys(SHIFT, ALT, CTRL);
            
            //txtKeyInt.Text = Macro.GetKeyString(_vk, ModifierKey.None);
            _vk = keyValue;
            string keystring = Macro.GetKeyString(_vk, _modifier); 
            if (!chkOrigKey.Checked)
                txtKeyInt.Text =  txtKeyInt2.Text=keystring;
            else 
                tmp = Macro.GetKeyString(_vk, _modifier); 


            _hasValue = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_hasValue || chkOrigKey.Checked)
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;

        }

        private void frmNewKey_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyValue != 12)
               e.IsInputKey = true;
        }
        //https://stackoverflow.com/questions/3730968/how-to-disable-cursor-in-textbox
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            HideCaret(txtKeyInt.Handle);
            HideCaret(txtKeyInt2.Handle);
        }

        private void frmNewKey_Deactivate(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.Cancel;
        }
        //Courtesy: https://stackoverflow.com/questions/33533908/is-there-an-event-thrown-when-a-user-tries-to-click-off-a-modal-dialog-created-w
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
                      //  button1_Click(null, EventArgs.Empty);//clicking off screen = clicking OK
                    }

                }
            }

            base.WndProc(ref m);
        }
        string tmp = null;
        //won't do anything when we are adding new key TRIGGER as the first form wont be shown.
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(chkOrigKey.Checked)
            {
                tmp = txtKeyInt.Text;
                txtKeyInt.Text = "(original)";
            } else
            {
                txtKeyInt.Text = tmp;
            }
        }

        private void txtCTRL_MouseDown(object sender, MouseEventArgs e)
        {
            _modifier ^= ModifierKey.Ctrl;
            setKeyState((_modifier & ModifierKey.Ctrl) != 0,
                (_modifier & ModifierKey.Alt) != 0,
                (_modifier & ModifierKey.Shift) != 0,
                
                _vk);
        }

        private void txtSHIFT_MouseDown(object sender, MouseEventArgs e)
        {
            _modifier ^= ModifierKey.Shift;
            setKeyState((_modifier & ModifierKey.Ctrl) != 0,
                (_modifier & ModifierKey.Alt) != 0,
                (_modifier & ModifierKey.Shift) != 0,

                _vk);
        }

        private void txtALT_MouseDown(object sender, MouseEventArgs e)
        {
            _modifier ^= ModifierKey.Alt;
            setKeyState((_modifier & ModifierKey.Ctrl) != 0,
                (_modifier & ModifierKey.Alt) != 0,
                (_modifier & ModifierKey.Shift) != 0,

                _vk);
        }

 
        private void chkRegion_CheckedChanged(object sender, EventArgs e)
        {
            btnRegion.Enabled = chkRegion.Checked;
            if (chkRegion.Checked && WinRect != Rectangle.Empty)
                txtMouseSubregion.Text = WinRect.ToString();
            else
                txtMouseSubregion.Text = "No subregion selected.";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;//mouse is presumed default.
        }

        bool _clearOffset = false;

        WinSelect wm = new WinSelect();
        private void btnRegion_Click(object sender, EventArgs e)
        {
            //mouse REGION
            DialogResult res;
            if (string.IsNullOrEmpty(cmbProcessMouse.Text.Trim()) || cmbProcessMouse.Text.Equals("<any>",StringComparison.OrdinalIgnoreCase))
            {
                res = wm.ShowDialog(null,WinRect);
            }else
            {
                res = wm.ShowDialog(cmbProcessMouse.Text.Split('|'), _clearOffset ? Rectangle.Empty : WinRect);
            }
            if (res == DialogResult.OK)
            {

                WinRect = wm.GetRect();
                txtMouseSubregion.Text = WinRect.ToString();

                
            }
            _clearOffset = false;
        }

        private void cmbProcessMouse_TextUpdate(object sender, EventArgs e)
        {
            _clearOffset = true;
        }
    }
}
