﻿namespace Script_Browser
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
            this.slideShow1 = new Script_Browser.Controls.SlideShow();
            this.SuspendLayout();
            // 
            // slideShow1
            // 
            this.slideShow1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(36)))), ((int)(((byte)(45)))));
            this.slideShow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.slideShow1.Location = new System.Drawing.Point(0, 0);
            this.slideShow1.Name = "slideShow1";
            this.slideShow1.Size = new System.Drawing.Size(720, 526);
            this.slideShow1.TabIndex = 0;
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 526);
            this.Controls.Add(this.slideShow1);
            this.Name = "Test";
            this.Text = "Test";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SlideShow slideShow1;
    }
}