namespace KeyReplace
{
    partial class WinSelect
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
            this.SuspendLayout();
            // 
            // WinSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(389, 182);
            this.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(35, 35);
            this.Name = "WinSelect";
            this.Opacity = 0.75D;
            this.Text = "WinSelect";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.WinSelect_Deactivate);
            this.Shown += new System.EventHandler(this.WinSelect_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nub_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WinSelect_MouseDown);
            this.Move += new System.EventHandler(this.WinSelect_Move);
            this.Resize += new System.EventHandler(this.WinSelect_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}