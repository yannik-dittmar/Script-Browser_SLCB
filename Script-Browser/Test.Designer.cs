namespace Script_Browser
{
    partial class Test
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
            this.comments1 = new Script_Browser.Controls.Comments();
            this.SuspendLayout();
            // 
            // comments1
            // 
            this.comments1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(25)))), ((int)(((byte)(31)))));
            this.comments1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comments1.Location = new System.Drawing.Point(0, 0);
            this.comments1.Name = "comments1";
            this.comments1.Size = new System.Drawing.Size(720, 526);
            this.comments1.TabIndex = 0;
            this.comments1.Load += new System.EventHandler(this.comments1_Load);
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 526);
            this.Controls.Add(this.comments1);
            this.Name = "Test";
            this.Text = "Test";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Comments comments1;
    }
}