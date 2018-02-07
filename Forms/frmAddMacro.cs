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
    public partial class frmAddMacro : Form
    {
        frmNewKey newKeyForm = new frmNewKey();
        public frmAddMacro()
        {
            InitializeComponent();

        }

        public bool HasEditedMacro { get; internal set; }
        public bool HasNewMacro { get; internal set; }
        

        public Macro GetMacro()
        {
            Macro next =  new Macro(textBox1.Text.Replace("\r\n"," "), 
                triggerList.Items.Cast<Trigger>().ToArray(),
                macroList.Items.Cast<InputEvent>().ToArray());

            if(checkBox1.Checked)
            {
                next.HasTableSwitch = true;
                next.TableSwitchName = cmbTables.Text;
            }

            next.PassOriginalKey = chkPassOriginalInput.Checked;

            return next;
        }

        public void EditMacro(Macro macro, string[] tableList)
        {
            SuspendLayout();
           
            Text = "Edit Macro";

            textBox1.Text = macro.Name;

            triggerList.Items.Clear();
            macroList.Items.Clear();

            foreach (Trigger trig in macro.Triggers)
            {
                triggerList.Items.Add(trig);
            }

            foreach (InputEvent evt in macro.Events)
            {
                macroList.Items.Add(evt);
            }

            checkBox1.Enabled = cmbTables.Enabled = tableList.Length > 0;
            checkBox1.Checked = false;
            cmbTables.Items.Clear();
            cmbTables.Items.AddRange(tableList);
            cmbTables.Enabled = macro.HasTableSwitch;

            chkPassOriginalInput.Checked = macro.PassOriginalKey;

            if (macro.HasTableSwitch)
            {
                checkBox1.Checked = true;
                cmbTables.SelectedIndex = Array.IndexOf(tableList, macro.TableSwitchName);//ERROR CHECK HERE PLS
            } 

            ResumeLayout();
        }

        public void NewMacro(string name, string[] tableList)
        {
            SuspendLayout();
            Text = name;

            textBox1.Text = name;
            
            triggerList.Items.Clear();
            macroList.Items.Clear();

            checkBox1.Enabled = cmbTables.Enabled = tableList.Length > 0;
            checkBox1.Checked = false;
            cmbTables.Items.Clear();
            if(tableList.Length > 0 ){
            	cmbTables.Items.AddRange(tableList);
            	
            	cmbTables.SelectedIndex = 0;
            }
            chkPassOriginalInput.Checked = false;
            ResumeLayout();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            //trigger add
            newKeyForm.IsInput = false;
            newKeyForm.ShowDialog();
            if(newKeyForm.DialogResult == DialogResult.OK)
            {
                Trigger next = newKeyForm.GetTrigger();
                if (!triggerList.Items.Cast<Trigger>().Contains(next))
                {
                    triggerList.Items.Add(next);
                    HasEditedMacro = HasNewMacro = true;
                    info.Text = "";
                    //info.Text = next.ToString() + " added. ";
                }
                else info.Text = next.ToString() + " is already a trigger for this macro. ";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //trigger remove
            if (triggerList.Items.Count > 0 && triggerList.SelectedIndex >= 0)
            {
                //info.Text = triggerList.Items[triggerList.SelectedIndex].ToString() + " removed. ";
                triggerList.Items.RemoveAt(triggerList.SelectedIndex);
                HasEditedMacro = HasNewMacro = true;
                info.Text = "";
            }
            else info.Text = "No item selected to remove.";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //macro add
            newKeyForm.IsInput = true;
            newKeyForm.ShowDialog();
            if (newKeyForm.DialogResult == DialogResult.OK)
            {
                InputEvent next = newKeyForm.GetInputEvent();

                    macroList.Items.Add(next);
                    HasEditedMacro = HasNewMacro = true;
                info.Text = "";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //macro remove
            if (macroList.Items.Count > 0 && macroList.SelectedIndex >= 0)
            {
                macroList.Items.RemoveAt(macroList.SelectedIndex);
                HasEditedMacro = HasNewMacro = true;
                info.Text = "";
            }
            else info.Text = "No item selected to remove.";
        }
        private void triggerList_DoubleClick(object sender, EventArgs e)
        {
            if (triggerList.SelectedIndex >= 0)
            {
                //edit trigger
                newKeyForm.IsInput = false;
                newKeyForm.ShowDialog((Trigger)triggerList.SelectedItem);
                
                if (newKeyForm.DialogResult == DialogResult.OK)
                {
                    Trigger next = newKeyForm.GetTrigger();
                    if (triggerList.Items.Cast<Trigger>().Count(t => t.Equals(next)) == 0)
                    {
                        triggerList.Items[triggerList.SelectedIndex] = next;
                        HasEditedMacro = HasNewMacro = true;
                        info.Text = next.ToString() + " edited. ";
                    }
                    else info.Text = next.ToString() + " is already a trigger for this macro. ";
                }
            }
        }

        private void macroList_DoubleClick(object sender, EventArgs e)
        {
            if (macroList.SelectedIndex >= 0)
            {
                //edit macro
                newKeyForm.IsInput = true;
                newKeyForm.ShowDialog((InputEvent)macroList.SelectedItem);
                if (newKeyForm.DialogResult == DialogResult.OK)
                {
                    InputEvent next = newKeyForm.GetInputEvent();

                    macroList.Items[macroList.SelectedIndex] = (next);
                    HasEditedMacro = HasNewMacro = true;
                    info.Text = next.ToString() + " edited.";
                }
            }
        }
        private void frmAddMacro_Shown(object sender, EventArgs e)
        {
            HasEditedMacro = HasNewMacro = false;

            info.ResetText(); // clear text (if any) from previous macro add/editing session.

            //focus on the textbox so ppl can type right in there
            textBox1.Focus();
            textBox1.SelectAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //move input event up
            if (macroList.SelectedIndex == -1)
                info.Text = "No selected item to move.";
            else if (macroList.SelectedIndex == 0)
                info.Text = "The selected event is already first to execute.";
            else
            {
                int nextIdx = macroList.SelectedIndex - 1;
                object next = macroList.SelectedItem;
                macroList.SuspendLayout();
                macroList.Items.RemoveAt(macroList.SelectedIndex);
                macroList.Items.Insert(nextIdx, next);
                macroList.SelectedIndex = nextIdx;
                macroList.ResumeLayout();
                HasEditedMacro = HasNewMacro = true;
                info.Text = "";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //move input event down
            if (macroList.SelectedIndex == -1)
                info.Text = "No selected item to move.";
            else if (macroList.SelectedIndex == macroList.Items.Count - 1)
                info.Text = "The selected event is already last to execute.";
            else
            {
                int nextIdx = macroList.SelectedIndex + 1;
                object next = macroList.SelectedItem;
                macroList.SuspendLayout();
                macroList.Items.RemoveAt(macroList.SelectedIndex);
                macroList.Items.Insert(nextIdx, next);
                macroList.SelectedIndex = nextIdx;
                macroList.ResumeLayout();
                HasEditedMacro = HasNewMacro = true;
                info.Text = "";
            }
        }

        private void cmbTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            HasEditedMacro = true;//only edited - not new macro.
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            HasEditedMacro = true;//only edited - not new macro.
            cmbTables.Enabled = checkBox1.Checked;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            HasEditedMacro = true;
        }
    }
}
