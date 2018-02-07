namespace KeyReplace
{
    partial class frmOSD
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
            this.label1 = new System.Windows.Forms.Label();
            this.follower = new System.Windows.Forms.Timer(this.components);
            this.txtDebug = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Corbel", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Intuos";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmOSD_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmOSD_MouseMove);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmOSD_MouseUp);
            // 
            // follower
            // 
            this.follower.Tick += new System.EventHandler(this.follower_Tick);
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(1, 31);
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(162, 23);
            this.txtDebug.TabIndex = 1;
            this.txtDebug.Text = "Debug (visible=False)";
            this.txtDebug.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.txtDebug.Visible = false;
            // 
            // frmOSD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(164, 45);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOSD";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "frmOSD";
            this.TopMost = true;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmOSD_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmOSD_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmOSD_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer follower;
        private System.Windows.Forms.Label txtDebug;
    }
}