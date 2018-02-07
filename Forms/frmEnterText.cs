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
    public partial class frmEnterText : Form
    {
        public frmEnterText()
        {
            InitializeComponent();
        }
        string _messageIfInvalid = string.Empty;
        Func<string,bool> validCondition = null;
        /// <summary>
        /// Shows a dialog with validation conditions
        /// </summary>
        /// <param name="prompt">the prompt to display</param>
        /// <param name="validCondition">null if there is no condition. return true if valid</param>
        /// <param name="messageIfInvalid">message displayed if invalid input. %s is replaced with original input.</param>
        public DialogResult ShowDialog(string prompt, Func<string,bool> validCondition, string messageIfInvalid)
        {
            label1.Text = prompt;
            this.validCondition = validCondition;
            this._messageIfInvalid = messageIfInvalid;

            return base.ShowDialog();
        }

        
        public string TextInput { get { return textBox1.Text.Replace("\r\n",""); } }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) {
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            if(validCondition!= null && !validCondition(textBox1.Text))
            {
                MessageBox.Show(_messageIfInvalid.Replace("%s",textBox1.Text));
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmEnterText_Shown(object sender, EventArgs e)
        {
            textBox1.Focus(); //settext does select all so we gud
        }

        public void SetText(string oldName)
        {
            textBox1.Text = oldName;
            textBox1.SelectAll();
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
                            this.DialogResult = DialogResult.Cancel;

                    }

                }
            }

            base.WndProc(ref m);
        }
    }
}
