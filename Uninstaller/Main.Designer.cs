namespace Uninstaller
{
    partial class Main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tcTlp1 = new Installer.TcTlp();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.noFocusBorderBtnNext = new Script_Browser.Controls.NoFocusBorderBtn();
            this.noFocusBorderBtn1 = new Script_Browser.Controls.NoFocusBorderBtn();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.noFocusBorderBtn2 = new Script_Browser.Controls.NoFocusBorderBtn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tcTlp1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(25)))), ((int)(((byte)(31)))));
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(536, 35);
            this.tableLayoutPanel1.TabIndex = 4;
            this.tableLayoutPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveForm_MouseDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Uninstaller.Properties.Resources.Logo_white_small;
            this.pictureBox1.Location = new System.Drawing.Point(10, 5);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(10, 5, 0, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(10, 5, 0, 5);
            this.pictureBox1.Size = new System.Drawing.Size(30, 25);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveForm_MouseDown);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(506, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 10, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "X";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(50, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(336, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Script Browser for Streamlabs Chatbot - UNINSTALLER";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveForm_MouseDown);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(25)))), ((int)(((byte)(31)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 300);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(536, 36);
            this.label3.TabIndex = 5;
            this.label3.Text = "SLCB Script-Browser © 2019 Digital-Programming";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tcTlp1
            // 
            this.tcTlp1.ColumnCount = 2;
            this.tcTlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tcTlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tcTlp1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tcTlp1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tcTlp1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTlp1.Location = new System.Drawing.Point(0, 35);
            this.tcTlp1.Name = "tcTlp1";
            this.tcTlp1.RowCount = 1;
            this.tcTlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tcTlp1.Size = new System.Drawing.Size(536, 265);
            this.tcTlp1.TabIndex = 6;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.noFocusBorderBtnNext, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.noFocusBorderBtn1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(530, 259);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // noFocusBorderBtnNext
            // 
            this.noFocusBorderBtnNext.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.noFocusBorderBtnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(139)))), ((int)(((byte)(118)))));
            this.noFocusBorderBtnNext.FlatAppearance.BorderSize = 0;
            this.noFocusBorderBtnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noFocusBorderBtnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noFocusBorderBtnNext.ForeColor = System.Drawing.Color.White;
            this.noFocusBorderBtnNext.Location = new System.Drawing.Point(437, 224);
            this.noFocusBorderBtnNext.Margin = new System.Windows.Forms.Padding(5);
            this.noFocusBorderBtnNext.Name = "noFocusBorderBtnNext";
            this.noFocusBorderBtnNext.Size = new System.Drawing.Size(88, 30);
            this.noFocusBorderBtnNext.TabIndex = 3;
            this.noFocusBorderBtnNext.TabStop = false;
            this.noFocusBorderBtnNext.Text = "Uninstall";
            this.noFocusBorderBtnNext.UseVisualStyleBackColor = false;
            this.noFocusBorderBtnNext.Click += new System.EventHandler(this.noFocusBorderBtnNext_Click);
            // 
            // noFocusBorderBtn1
            // 
            this.noFocusBorderBtn1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.noFocusBorderBtn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(25)))), ((int)(((byte)(31)))));
            this.noFocusBorderBtn1.FlatAppearance.BorderSize = 0;
            this.noFocusBorderBtn1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noFocusBorderBtn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noFocusBorderBtn1.ForeColor = System.Drawing.Color.White;
            this.noFocusBorderBtn1.Location = new System.Drawing.Point(339, 224);
            this.noFocusBorderBtn1.Margin = new System.Windows.Forms.Padding(5);
            this.noFocusBorderBtn1.Name = "noFocusBorderBtn1";
            this.noFocusBorderBtn1.Size = new System.Drawing.Size(88, 30);
            this.noFocusBorderBtn1.TabIndex = 4;
            this.noFocusBorderBtn1.TabStop = false;
            this.noFocusBorderBtn1.Text = "Exit";
            this.noFocusBorderBtn1.UseVisualStyleBackColor = false;
            this.noFocusBorderBtn1.Click += new System.EventHandler(this.label2_Click);
            // 
            // label4
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.label4, 3);
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(524, 219);
            this.label4.TabIndex = 5;
            this.label4.Text = "This uninstaller will remove any files and associations made by the application S" +
    "treamlabs Chatbot - Script Browser on your computer.\r\n\r\nInstalled scripts are st" +
    "ill available.";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.noFocusBorderBtn2, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(539, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1, 259);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // noFocusBorderBtn2
            // 
            this.noFocusBorderBtn2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.noFocusBorderBtn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(139)))), ((int)(((byte)(118)))));
            this.noFocusBorderBtn2.FlatAppearance.BorderSize = 0;
            this.noFocusBorderBtn2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noFocusBorderBtn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noFocusBorderBtn2.ForeColor = System.Drawing.Color.White;
            this.noFocusBorderBtn2.Location = new System.Drawing.Point(-92, 224);
            this.noFocusBorderBtn2.Margin = new System.Windows.Forms.Padding(5);
            this.noFocusBorderBtn2.Name = "noFocusBorderBtn2";
            this.noFocusBorderBtn2.Size = new System.Drawing.Size(88, 30);
            this.noFocusBorderBtn2.TabIndex = 3;
            this.noFocusBorderBtn2.TabStop = false;
            this.noFocusBorderBtn2.Text = "Finish";
            this.noFocusBorderBtn2.UseVisualStyleBackColor = false;
            this.noFocusBorderBtn2.Visible = false;
            this.noFocusBorderBtn2.Click += new System.EventHandler(this.noFocusBorderBtn2_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(25)))), ((int)(((byte)(31)))));
            this.tableLayoutPanel3.SetColumnSpan(this.panel5, 2);
            this.panel5.Controls.Add(this.richTextBoxLog);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Margin = new System.Windows.Forms.Padding(5);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(3);
            this.panel5.Size = new System.Drawing.Size(1, 209);
            this.panel5.TabIndex = 4;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(25)))), ((int)(((byte)(31)))));
            this.richTextBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLog.ForeColor = System.Drawing.Color.White;
            this.richTextBoxLog.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(0, 203);
            this.richTextBoxLog.TabIndex = 2;
            this.richTextBoxLog.Text = "";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(36)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(536, 336);
            this.Controls.Add(this.tcTlp1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tcTlp1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private Installer.TcTlp tcTlp1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Script_Browser.Controls.NoFocusBorderBtn noFocusBorderBtnNext;
        private Script_Browser.Controls.NoFocusBorderBtn noFocusBorderBtn1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Script_Browser.Controls.NoFocusBorderBtn noFocusBorderBtn2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
    }
}

