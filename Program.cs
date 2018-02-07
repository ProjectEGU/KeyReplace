using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyReplace
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// https://www.guidgenerator.com/online-guid-generator.aspx
        /// </summary>
        static Mutex mutex = new Mutex(true, "{7329AF01-2B48-4B7C-8661-DE42AA8BFE89}");
        [STAThread]
        static void Main(string[] args)
        {
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                MessageBox.Show("KeyReplace is already running.","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if(args.Length > 0 && args[0].Equals("/tray"))
            {
                Application.Run(new HiddenContext());
            } else
            {
                Application.Run(new frmMain());
            }
        }
        //start form hidden, due to : https://social.msdn.microsoft.com/Forums/windows/en-US/dece45c8-9076-497e-9414-8cd9b34f572f/how-to-start-a-form-in-hidden-mode?forum=winforms


    }

    class HiddenContext : ApplicationContext
    {
        public HiddenContext()
        {
            frmMain form1 = new frmMain();
            form1.Visible = false;
            form1.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form1_FormClosed);
            form1.FormClosing += new FormClosingEventHandler(form1_FormClosing);
        }

        void form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (MessageBox.Show("Are you sure you want to close the application?", "Application Closing", MessageBoxButtons.YesNo) == DialogResult.No)
                //e.Cancel = true;
        }

        void form1_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            this.ExitThread();
        }
    }
}
