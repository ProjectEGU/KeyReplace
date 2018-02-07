namespace KeyReplace
{
    partial class frmNewKey
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.mouse_Trigger = new System.Windows.Forms.TabPage();
            this.txtMouseSubregion = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbProcessMouse = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.chkRegion = new System.Windows.Forms.CheckBox();
            this.btnRegion = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbMouseTriggerButton = new System.Windows.Forms.ComboBox();
            this.cmbMouseEventType = new System.Windows.Forms.ComboBox();
            this.keyInput = new System.Windows.Forms.TabPage();
            this.txtCTRL = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.chkOrigKey = new System.Windows.Forms.CheckBox();
            this.txtSHIFT = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.txtALT = new System.Windows.Forms.Label();
            this.txtKeyInt = new System.Windows.Forms.TextBox();
            this.chkKeyPress = new System.Windows.Forms.RadioButton();
            this.chkKeyDown = new System.Windows.Forms.RadioButton();
            this.chkKeyUp = new System.Windows.Forms.RadioButton();
            this.maintabs = new System.Windows.Forms.TabControl();
            this.keyTrigger = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbProcessKey = new System.Windows.Forms.ComboBox();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.txtCTRL2 = new System.Windows.Forms.Label();
            this.txtSHIFT2 = new System.Windows.Forms.Label();
            this.txtALT2 = new System.Windows.Forms.Label();
            this.txtKeyInt2 = new System.Windows.Forms.TextBox();
            this.chkKeyDown2 = new System.Windows.Forms.RadioButton();
            this.chkKeyUp2 = new System.Windows.Forms.RadioButton();
            this.mouse_Trigger.SuspendLayout();
            this.keyInput.SuspendLayout();
            this.maintabs.SuspendLayout();
            this.keyTrigger.SuspendLayout();
            this.SuspendLayout();
            // 
            // mouse_Trigger
            // 
            this.mouse_Trigger.Controls.Add(this.txtMouseSubregion);
            this.mouse_Trigger.Controls.Add(this.panel1);
            this.mouse_Trigger.Controls.Add(this.label6);
            this.mouse_Trigger.Controls.Add(this.cmbProcessMouse);
            this.mouse_Trigger.Controls.Add(this.button5);
            this.mouse_Trigger.Controls.Add(this.button4);
            this.mouse_Trigger.Controls.Add(this.chkRegion);
            this.mouse_Trigger.Controls.Add(this.btnRegion);
            this.mouse_Trigger.Controls.Add(this.label5);
            this.mouse_Trigger.Controls.Add(this.label4);
            this.mouse_Trigger.Controls.Add(this.cmbMouseTriggerButton);
            this.mouse_Trigger.Controls.Add(this.cmbMouseEventType);
            this.mouse_Trigger.Location = new System.Drawing.Point(4, 22);
            this.mouse_Trigger.Name = "mouse_Trigger";
            this.mouse_Trigger.Padding = new System.Windows.Forms.Padding(3);
            this.mouse_Trigger.Size = new System.Drawing.Size(314, 132);
            this.mouse_Trigger.TabIndex = 3;
            this.mouse_Trigger.Text = "Mouse Trigger";
            this.mouse_Trigger.UseVisualStyleBackColor = true;
            // 
            // txtMouseSubregion
            // 
            this.txtMouseSubregion.Location = new System.Drawing.Point(15, 100);
            this.txtMouseSubregion.Name = "txtMouseSubregion";
            this.txtMouseSubregion.Size = new System.Drawing.Size(134, 29);
            this.txtMouseSubregion.TabIndex = 40;
            this.txtMouseSubregion.Text = "No subregion selected.";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(15, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(267, 1);
            this.panel1.TabIndex = 39;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "Process:";
            // 
            // cmbProcessMouse
            // 
            this.cmbProcessMouse.FormattingEnabled = true;
            this.cmbProcessMouse.Items.AddRange(new object[] {
            "<Any>",
            "rs2client|jagexlauncher"});
            this.cmbProcessMouse.Location = new System.Drawing.Point(112, 53);
            this.cmbProcessMouse.Name = "cmbProcessMouse";
            this.cmbProcessMouse.Size = new System.Drawing.Size(139, 21);
            this.cmbProcessMouse.TabIndex = 37;
            this.cmbProcessMouse.Text = "<Any>";
            this.cmbProcessMouse.TextUpdate += new System.EventHandler(this.cmbProcessMouse_TextUpdate);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(233)))));
            this.button5.Location = new System.Drawing.Point(152, 106);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 36;
            this.button5.Text = "Cancel";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(233)))));
            this.button4.Location = new System.Drawing.Point(233, 106);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 35;
            this.button4.Text = "OK";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // chkRegion
            // 
            this.chkRegion.AutoSize = true;
            this.chkRegion.Location = new System.Drawing.Point(18, 78);
            this.chkRegion.Name = "chkRegion";
            this.chkRegion.Size = new System.Drawing.Size(85, 17);
            this.chkRegion.TabIndex = 34;
            this.chkRegion.Text = "Sub Region:";
            this.chkRegion.UseVisualStyleBackColor = true;
            this.chkRegion.CheckedChanged += new System.EventHandler(this.chkRegion_CheckedChanged);
            // 
            // btnRegion
            // 
            this.btnRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(233)))));
            this.btnRegion.Location = new System.Drawing.Point(112, 74);
            this.btnRegion.Name = "btnRegion";
            this.btnRegion.Size = new System.Drawing.Size(139, 23);
            this.btnRegion.TabIndex = 31;
            this.btnRegion.Text = "Select Region";
            this.btnRegion.UseVisualStyleBackColor = false;
            this.btnRegion.Click += new System.EventHandler(this.btnRegion_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(139, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Event Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Button";
            // 
            // cmbMouseTriggerButton
            // 
            this.cmbMouseTriggerButton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMouseTriggerButton.FormattingEnabled = true;
            this.cmbMouseTriggerButton.Items.AddRange(new object[] {
            "Left Button",
            "Right Button",
            "Middle Button",
            "X1",
            "X2"});
            this.cmbMouseTriggerButton.Location = new System.Drawing.Point(15, 20);
            this.cmbMouseTriggerButton.Name = "cmbMouseTriggerButton";
            this.cmbMouseTriggerButton.Size = new System.Drawing.Size(121, 21);
            this.cmbMouseTriggerButton.TabIndex = 25;
            // 
            // cmbMouseEventType
            // 
            this.cmbMouseEventType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMouseEventType.FormattingEnabled = true;
            this.cmbMouseEventType.Items.AddRange(new object[] {
            "Mouse Down",
            "Mouse Up"});
            this.cmbMouseEventType.Location = new System.Drawing.Point(142, 20);
            this.cmbMouseEventType.Name = "cmbMouseEventType";
            this.cmbMouseEventType.Size = new System.Drawing.Size(121, 21);
            this.cmbMouseEventType.TabIndex = 24;
            // 
            // keyInput
            // 
            this.keyInput.Controls.Add(this.txtCTRL);
            this.keyInput.Controls.Add(this.button1);
            this.keyInput.Controls.Add(this.chkOrigKey);
            this.keyInput.Controls.Add(this.txtSHIFT);
            this.keyInput.Controls.Add(this.button2);
            this.keyInput.Controls.Add(this.txtALT);
            this.keyInput.Controls.Add(this.txtKeyInt);
            this.keyInput.Controls.Add(this.chkKeyPress);
            this.keyInput.Controls.Add(this.chkKeyDown);
            this.keyInput.Controls.Add(this.chkKeyUp);
            this.keyInput.Location = new System.Drawing.Point(4, 22);
            this.keyInput.Name = "keyInput";
            this.keyInput.Padding = new System.Windows.Forms.Padding(3);
            this.keyInput.Size = new System.Drawing.Size(314, 132);
            this.keyInput.TabIndex = 0;
            this.keyInput.Text = "Key Input";
            this.keyInput.UseVisualStyleBackColor = true;
            // 
            // txtCTRL
            // 
            this.txtCTRL.AutoSize = true;
            this.txtCTRL.Location = new System.Drawing.Point(8, 9);
            this.txtCTRL.Name = "txtCTRL";
            this.txtCTRL.Size = new System.Drawing.Size(35, 13);
            this.txtCTRL.TabIndex = 7;
            this.txtCTRL.Text = "CTRL";
            this.txtCTRL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtCTRL_MouseDown);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(233)))));
            this.button1.Location = new System.Drawing.Point(240, 102);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 24);
            this.button1.TabIndex = 14;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyDown);
            this.button1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyUp);
            this.button1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmNewKey_PreviewKeyDown);
            // 
            // chkOrigKey
            // 
            this.chkOrigKey.AutoSize = true;
            this.chkOrigKey.Location = new System.Drawing.Point(8, 58);
            this.chkOrigKey.Name = "chkOrigKey";
            this.chkOrigKey.Size = new System.Drawing.Size(104, 17);
            this.chkOrigKey.TabIndex = 16;
            this.chkOrigKey.Text = "Use Original Key";
            this.chkOrigKey.UseVisualStyleBackColor = true;
            this.chkOrigKey.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            this.chkOrigKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyDown);
            this.chkOrigKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyUp);
            this.chkOrigKey.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmNewKey_PreviewKeyDown);
            // 
            // txtSHIFT
            // 
            this.txtSHIFT.AutoSize = true;
            this.txtSHIFT.Location = new System.Drawing.Point(49, 9);
            this.txtSHIFT.Name = "txtSHIFT";
            this.txtSHIFT.Size = new System.Drawing.Size(38, 13);
            this.txtSHIFT.TabIndex = 8;
            this.txtSHIFT.Text = "SHIFT";
            this.txtSHIFT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtSHIFT_MouseDown);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(233)))));
            this.button2.Location = new System.Drawing.Point(171, 102);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(63, 24);
            this.button2.TabIndex = 15;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyDown);
            this.button2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyUp);
            this.button2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmNewKey_PreviewKeyDown);
            // 
            // txtALT
            // 
            this.txtALT.AutoSize = true;
            this.txtALT.Location = new System.Drawing.Point(93, 9);
            this.txtALT.Name = "txtALT";
            this.txtALT.Size = new System.Drawing.Size(27, 13);
            this.txtALT.TabIndex = 9;
            this.txtALT.Text = "ALT";
            this.txtALT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtALT_MouseDown);
            // 
            // txtKeyInt
            // 
            this.txtKeyInt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(233)))));
            this.txtKeyInt.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtKeyInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKeyInt.Location = new System.Drawing.Point(8, 25);
            this.txtKeyInt.MaxLength = 1;
            this.txtKeyInt.Name = "txtKeyInt";
            this.txtKeyInt.ReadOnly = true;
            this.txtKeyInt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtKeyInt.Size = new System.Drawing.Size(122, 26);
            this.txtKeyInt.TabIndex = 10;
            this.txtKeyInt.Text = "Press any key.";
            this.txtKeyInt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtKeyInt.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.txtKeyInt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyDown);
            this.txtKeyInt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyUp);
            this.txtKeyInt.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmNewKey_PreviewKeyDown);
            // 
            // chkKeyPress
            // 
            this.chkKeyPress.AutoSize = true;
            this.chkKeyPress.Location = new System.Drawing.Point(169, 6);
            this.chkKeyPress.Name = "chkKeyPress";
            this.chkKeyPress.Size = new System.Drawing.Size(72, 17);
            this.chkKeyPress.TabIndex = 13;
            this.chkKeyPress.Text = "Key Press";
            this.chkKeyPress.UseVisualStyleBackColor = true;
            this.chkKeyPress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyDown);
            this.chkKeyPress.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyUp);
            this.chkKeyPress.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmNewKey_PreviewKeyDown);
            // 
            // chkKeyDown
            // 
            this.chkKeyDown.AutoSize = true;
            this.chkKeyDown.Checked = true;
            this.chkKeyDown.Location = new System.Drawing.Point(169, 29);
            this.chkKeyDown.Name = "chkKeyDown";
            this.chkKeyDown.Size = new System.Drawing.Size(74, 17);
            this.chkKeyDown.TabIndex = 11;
            this.chkKeyDown.TabStop = true;
            this.chkKeyDown.Text = "Key Down";
            this.chkKeyDown.UseVisualStyleBackColor = true;
            this.chkKeyDown.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyDown);
            this.chkKeyDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyUp);
            this.chkKeyDown.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmNewKey_PreviewKeyDown);
            // 
            // chkKeyUp
            // 
            this.chkKeyUp.AutoSize = true;
            this.chkKeyUp.Location = new System.Drawing.Point(169, 52);
            this.chkKeyUp.Name = "chkKeyUp";
            this.chkKeyUp.Size = new System.Drawing.Size(60, 17);
            this.chkKeyUp.TabIndex = 12;
            this.chkKeyUp.Text = "Key Up";
            this.chkKeyUp.UseVisualStyleBackColor = true;
            this.chkKeyUp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyDown);
            this.chkKeyUp.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyUp);
            this.chkKeyUp.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmNewKey_PreviewKeyDown);
            // 
            // maintabs
            // 
            this.maintabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maintabs.Controls.Add(this.keyInput);
            this.maintabs.Controls.Add(this.keyTrigger);
            this.maintabs.Controls.Add(this.mouse_Trigger);
            this.maintabs.Location = new System.Drawing.Point(12, 12);
            this.maintabs.Name = "maintabs";
            this.maintabs.SelectedIndex = 0;
            this.maintabs.Size = new System.Drawing.Size(322, 158);
            this.maintabs.TabIndex = 17;
            // 
            // keyTrigger
            // 
            this.keyTrigger.Controls.Add(this.panel2);
            this.keyTrigger.Controls.Add(this.label15);
            this.keyTrigger.Controls.Add(this.cmbProcessKey);
            this.keyTrigger.Controls.Add(this.button10);
            this.keyTrigger.Controls.Add(this.button11);
            this.keyTrigger.Controls.Add(this.txtCTRL2);
            this.keyTrigger.Controls.Add(this.txtSHIFT2);
            this.keyTrigger.Controls.Add(this.txtALT2);
            this.keyTrigger.Controls.Add(this.txtKeyInt2);
            this.keyTrigger.Controls.Add(this.chkKeyDown2);
            this.keyTrigger.Controls.Add(this.chkKeyUp2);
            this.keyTrigger.Location = new System.Drawing.Point(4, 22);
            this.keyTrigger.Name = "keyTrigger";
            this.keyTrigger.Padding = new System.Windows.Forms.Padding(3);
            this.keyTrigger.Size = new System.Drawing.Size(314, 132);
            this.keyTrigger.TabIndex = 2;
            this.keyTrigger.Text = "Key Trigger";
            this.keyTrigger.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(18, 54);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(267, 1);
            this.panel2.TabIndex = 46;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(17, 62);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(48, 13);
            this.label15.TabIndex = 45;
            this.label15.Text = "Process:";
            // 
            // cmbProcessKey
            // 
            this.cmbProcessKey.FormattingEnabled = true;
            this.cmbProcessKey.Items.AddRange(new object[] {
            "<Any>",
            "rs2client|jagexlauncher"});
            this.cmbProcessKey.Location = new System.Drawing.Point(111, 59);
            this.cmbProcessKey.Name = "cmbProcessKey";
            this.cmbProcessKey.Size = new System.Drawing.Size(139, 21);
            this.cmbProcessKey.TabIndex = 44;
            this.cmbProcessKey.Text = "<Any>";
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(233)))));
            this.button10.Location = new System.Drawing.Point(152, 103);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 23);
            this.button10.TabIndex = 43;
            this.button10.Text = "Cancel";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.button2_Click);
            this.button10.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyDown);
            this.button10.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyUp);
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(233)))));
            this.button11.Location = new System.Drawing.Point(233, 103);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(75, 23);
            this.button11.TabIndex = 42;
            this.button11.Text = "OK";
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtCTRL2
            // 
            this.txtCTRL2.AutoSize = true;
            this.txtCTRL2.Location = new System.Drawing.Point(15, 6);
            this.txtCTRL2.Name = "txtCTRL2";
            this.txtCTRL2.Size = new System.Drawing.Size(35, 13);
            this.txtCTRL2.TabIndex = 17;
            this.txtCTRL2.Text = "CTRL";
            this.txtCTRL2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtCTRL_MouseDown);
            // 
            // txtSHIFT2
            // 
            this.txtSHIFT2.AutoSize = true;
            this.txtSHIFT2.Location = new System.Drawing.Point(56, 6);
            this.txtSHIFT2.Name = "txtSHIFT2";
            this.txtSHIFT2.Size = new System.Drawing.Size(38, 13);
            this.txtSHIFT2.TabIndex = 18;
            this.txtSHIFT2.Text = "SHIFT";
            this.txtSHIFT2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtSHIFT_MouseDown);
            // 
            // txtALT2
            // 
            this.txtALT2.AutoSize = true;
            this.txtALT2.Location = new System.Drawing.Point(100, 6);
            this.txtALT2.Name = "txtALT2";
            this.txtALT2.Size = new System.Drawing.Size(27, 13);
            this.txtALT2.TabIndex = 19;
            this.txtALT2.Text = "ALT";
            this.txtALT2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtALT_MouseDown);
            // 
            // txtKeyInt2
            // 
            this.txtKeyInt2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(233)))));
            this.txtKeyInt2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtKeyInt2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKeyInt2.Location = new System.Drawing.Point(15, 22);
            this.txtKeyInt2.MaxLength = 1;
            this.txtKeyInt2.Name = "txtKeyInt2";
            this.txtKeyInt2.ReadOnly = true;
            this.txtKeyInt2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtKeyInt2.Size = new System.Drawing.Size(122, 26);
            this.txtKeyInt2.TabIndex = 20;
            this.txtKeyInt2.Text = "Press any key.";
            this.txtKeyInt2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtKeyInt2.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.txtKeyInt2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyDown);
            this.txtKeyInt2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyUp);
            this.txtKeyInt2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmNewKey_PreviewKeyDown);
            // 
            // chkKeyDown2
            // 
            this.chkKeyDown2.AutoSize = true;
            this.chkKeyDown2.Checked = true;
            this.chkKeyDown2.Location = new System.Drawing.Point(178, 10);
            this.chkKeyDown2.Name = "chkKeyDown2";
            this.chkKeyDown2.Size = new System.Drawing.Size(74, 17);
            this.chkKeyDown2.TabIndex = 21;
            this.chkKeyDown2.TabStop = true;
            this.chkKeyDown2.Text = "Key Down";
            this.chkKeyDown2.UseVisualStyleBackColor = true;
            this.chkKeyDown2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyDown);
            this.chkKeyDown2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyUp);
            this.chkKeyDown2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmNewKey_PreviewKeyDown);
            // 
            // chkKeyUp2
            // 
            this.chkKeyUp2.AutoSize = true;
            this.chkKeyUp2.Location = new System.Drawing.Point(178, 31);
            this.chkKeyUp2.Name = "chkKeyUp2";
            this.chkKeyUp2.Size = new System.Drawing.Size(60, 17);
            this.chkKeyUp2.TabIndex = 22;
            this.chkKeyUp2.Text = "Key Up";
            this.chkKeyUp2.UseVisualStyleBackColor = true;
            this.chkKeyUp2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyDown);
            this.chkKeyUp2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyUp);
            this.chkKeyUp2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmNewKey_PreviewKeyDown);
            // 
            // frmNewKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(346, 182);
            this.Controls.Add(this.maintabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmNewKey";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Event/Trigger";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.frmNewKey_Deactivate);
            this.Shown += new System.EventHandler(this.frmNewKey_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNewKey_KeyUp);
            this.Leave += new System.EventHandler(this.frmNewKey_Deactivate);
            this.mouse_Trigger.ResumeLayout(false);
            this.mouse_Trigger.PerformLayout();
            this.keyInput.ResumeLayout(false);
            this.keyInput.PerformLayout();
            this.maintabs.ResumeLayout(false);
            this.keyTrigger.ResumeLayout(false);
            this.keyTrigger.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage mouse_Trigger;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbProcessMouse;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox chkRegion;
        private System.Windows.Forms.Button btnRegion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbMouseTriggerButton;
        private System.Windows.Forms.ComboBox cmbMouseEventType;
        private System.Windows.Forms.TabPage keyInput;
        private System.Windows.Forms.Label txtCTRL;
        private System.Windows.Forms.CheckBox chkOrigKey;
        private System.Windows.Forms.Label txtSHIFT;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label txtALT;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtKeyInt;
        private System.Windows.Forms.RadioButton chkKeyPress;
        private System.Windows.Forms.RadioButton chkKeyDown;
        private System.Windows.Forms.RadioButton chkKeyUp;
        private System.Windows.Forms.TabControl maintabs;
        private System.Windows.Forms.TabPage keyTrigger;
        private System.Windows.Forms.Label txtCTRL2;
        private System.Windows.Forms.Label txtSHIFT2;
        private System.Windows.Forms.Label txtALT2;
        private System.Windows.Forms.TextBox txtKeyInt2;
        private System.Windows.Forms.RadioButton chkKeyDown2;
        private System.Windows.Forms.RadioButton chkKeyUp2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbProcessKey;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Label txtMouseSubregion;
    }
}