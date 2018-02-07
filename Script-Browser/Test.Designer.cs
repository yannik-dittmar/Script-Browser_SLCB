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
            this.showScript1 = new Script_Browser.Controls.ShowScript();
            this.SuspendLayout();
            // 
            // showScript1
            // 
            this.showScript1.AutoScroll = true;
            this.showScript1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(36)))), ((int)(((byte)(45)))));
            this.showScript1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showScript1.Location = new System.Drawing.Point(0, 0);
            this.showScript1.Name = "showScript1";
            this.showScript1.Size = new System.Drawing.Size(720, 526);
            this.showScript1.TabIndex = 0;
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 526);
            this.Controls.Add(this.showScript1);
            this.Name = "Test";
            this.Text = "Test";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ShowScript showScript1;
    }
}