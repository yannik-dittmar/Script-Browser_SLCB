namespace Installer
{
    partial class InstallerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallerForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tcTlp1 = new Installer.TcTlp();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tcTlp2 = new Installer.TcTlp();
            this.noFocusBorderBtn1 = new Script_Browser.Controls.NoFocusBorderBtn();
            this.noFocusBorderBtn2 = new Script_Browser.Controls.NoFocusBorderBtn();
            this.noFocusBorderBtn3 = new Script_Browser.Controls.NoFocusBorderBtn();
            this.noFocusBorderBtn4 = new Script_Browser.Controls.NoFocusBorderBtn();
            this.noFocusBorderBtn5 = new Script_Browser.Controls.NoFocusBorderBtn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.tcTlp1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(617, 35);
            this.tableLayoutPanel1.TabIndex = 3;
            this.tableLayoutPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveForm_MouseDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Installer.Properties.Resources.Logo_white;
            this.pictureBox1.Location = new System.Drawing.Point(10, 5);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(10, 5, 0, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(10, 5, 0, 5);
            this.pictureBox1.Size = new System.Drawing.Size(30, 25);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(587, 7);
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
            this.label1.Size = new System.Drawing.Size(282, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Script Browser for Streamlabs Chatbot SETUP";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveForm_MouseDown);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 337);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(617, 36);
            this.label3.TabIndex = 4;
            this.label3.Text = "SLCB Script-Browser v1.0.0 © 2018 Digital-Programming";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(36)))), ((int)(((byte)(45)))));
            this.panel1.Controls.Add(this.tcTlp1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(617, 302);
            this.panel1.TabIndex = 5;
            // 
            // tcTlp1
            // 
            this.tcTlp1.ColumnCount = 3;
            this.tcTlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tcTlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tcTlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tcTlp1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tcTlp1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTlp1.Location = new System.Drawing.Point(0, 0);
            this.tcTlp1.Name = "tcTlp1";
            this.tcTlp1.RowCount = 1;
            this.tcTlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tcTlp1.Size = new System.Drawing.Size(617, 302);
            this.tcTlp1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tcTlp2, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(617, 302);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Controls.Add(this.noFocusBorderBtn5, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.noFocusBorderBtn3, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.noFocusBorderBtn1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.noFocusBorderBtn4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.noFocusBorderBtn2, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(617, 40);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tcTlp2
            // 
            this.tcTlp2.ColumnCount = 5;
            this.tcTlp2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcTlp2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcTlp2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcTlp2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcTlp2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcTlp2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTlp2.Location = new System.Drawing.Point(3, 43);
            this.tcTlp2.Name = "tcTlp2";
            this.tcTlp2.RowCount = 1;
            this.tcTlp2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tcTlp2.Size = new System.Drawing.Size(611, 256);
            this.tcTlp2.TabIndex = 1;
            // 
            // noFocusBorderBtn1
            // 
            this.noFocusBorderBtn1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.noFocusBorderBtn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(139)))), ((int)(((byte)(118)))));
            this.noFocusBorderBtn1.FlatAppearance.BorderSize = 0;
            this.noFocusBorderBtn1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noFocusBorderBtn1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            this.noFocusBorderBtn1.ForeColor = System.Drawing.Color.White;
            this.noFocusBorderBtn1.Location = new System.Drawing.Point(0, 0);
            this.noFocusBorderBtn1.Margin = new System.Windows.Forms.Padding(0);
            this.noFocusBorderBtn1.Name = "noFocusBorderBtn1";
            this.noFocusBorderBtn1.Size = new System.Drawing.Size(123, 40);
            this.noFocusBorderBtn1.TabIndex = 1;
            this.noFocusBorderBtn1.TabStop = false;
            this.noFocusBorderBtn1.Tag = "1";
            this.noFocusBorderBtn1.Text = "Welcome";
            this.noFocusBorderBtn1.UseVisualStyleBackColor = false;
            // 
            // noFocusBorderBtn2
            // 
            this.noFocusBorderBtn2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.noFocusBorderBtn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(72)))), ((int)(((byte)(70)))));
            this.noFocusBorderBtn2.Enabled = false;
            this.noFocusBorderBtn2.FlatAppearance.BorderSize = 0;
            this.noFocusBorderBtn2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noFocusBorderBtn2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            this.noFocusBorderBtn2.ForeColor = System.Drawing.Color.White;
            this.noFocusBorderBtn2.Location = new System.Drawing.Point(369, 0);
            this.noFocusBorderBtn2.Margin = new System.Windows.Forms.Padding(0);
            this.noFocusBorderBtn2.Name = "noFocusBorderBtn2";
            this.noFocusBorderBtn2.Size = new System.Drawing.Size(123, 40);
            this.noFocusBorderBtn2.TabIndex = 6;
            this.noFocusBorderBtn2.TabStop = false;
            this.noFocusBorderBtn2.Tag = "2";
            this.noFocusBorderBtn2.Text = "Install. Path";
            this.noFocusBorderBtn2.UseVisualStyleBackColor = false;
            // 
            // noFocusBorderBtn3
            // 
            this.noFocusBorderBtn3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.noFocusBorderBtn3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(72)))), ((int)(((byte)(70)))));
            this.noFocusBorderBtn3.Enabled = false;
            this.noFocusBorderBtn3.FlatAppearance.BorderSize = 0;
            this.noFocusBorderBtn3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noFocusBorderBtn3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            this.noFocusBorderBtn3.ForeColor = System.Drawing.Color.White;
            this.noFocusBorderBtn3.Location = new System.Drawing.Point(492, 0);
            this.noFocusBorderBtn3.Margin = new System.Windows.Forms.Padding(0);
            this.noFocusBorderBtn3.Name = "noFocusBorderBtn3";
            this.noFocusBorderBtn3.Size = new System.Drawing.Size(125, 40);
            this.noFocusBorderBtn3.TabIndex = 7;
            this.noFocusBorderBtn3.TabStop = false;
            this.noFocusBorderBtn3.Tag = "2";
            this.noFocusBorderBtn3.Text = "Install. Options";
            this.noFocusBorderBtn3.UseVisualStyleBackColor = false;
            // 
            // noFocusBorderBtn4
            // 
            this.noFocusBorderBtn4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.noFocusBorderBtn4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(72)))), ((int)(((byte)(70)))));
            this.noFocusBorderBtn4.Enabled = false;
            this.noFocusBorderBtn4.FlatAppearance.BorderSize = 0;
            this.noFocusBorderBtn4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noFocusBorderBtn4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            this.noFocusBorderBtn4.ForeColor = System.Drawing.Color.White;
            this.noFocusBorderBtn4.Location = new System.Drawing.Point(123, 0);
            this.noFocusBorderBtn4.Margin = new System.Windows.Forms.Padding(0);
            this.noFocusBorderBtn4.Name = "noFocusBorderBtn4";
            this.noFocusBorderBtn4.Size = new System.Drawing.Size(123, 40);
            this.noFocusBorderBtn4.TabIndex = 8;
            this.noFocusBorderBtn4.TabStop = false;
            this.noFocusBorderBtn4.Tag = "2";
            this.noFocusBorderBtn4.Text = "License";
            this.noFocusBorderBtn4.UseVisualStyleBackColor = false;
            // 
            // noFocusBorderBtn5
            // 
            this.noFocusBorderBtn5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.noFocusBorderBtn5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(72)))), ((int)(((byte)(70)))));
            this.noFocusBorderBtn5.Enabled = false;
            this.noFocusBorderBtn5.FlatAppearance.BorderSize = 0;
            this.noFocusBorderBtn5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noFocusBorderBtn5.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            this.noFocusBorderBtn5.ForeColor = System.Drawing.Color.White;
            this.noFocusBorderBtn5.Location = new System.Drawing.Point(246, 0);
            this.noFocusBorderBtn5.Margin = new System.Windows.Forms.Padding(0);
            this.noFocusBorderBtn5.Name = "noFocusBorderBtn5";
            this.noFocusBorderBtn5.Size = new System.Drawing.Size(123, 40);
            this.noFocusBorderBtn5.TabIndex = 9;
            this.noFocusBorderBtn5.TabStop = false;
            this.noFocusBorderBtn5.Tag = "2";
            this.noFocusBorderBtn5.Text = "Rules";
            this.noFocusBorderBtn5.UseVisualStyleBackColor = false;
            // 
            // InstallerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(25)))), ((int)(((byte)(31)))));
            this.ClientSize = new System.Drawing.Size(617, 373);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InstallerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SLCBSB Setup";
            this.Load += new System.EventHandler(this.InstallerForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tcTlp1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private TcTlp tcTlp1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private TcTlp tcTlp2;
        private Script_Browser.Controls.NoFocusBorderBtn noFocusBorderBtn1;
        private Script_Browser.Controls.NoFocusBorderBtn noFocusBorderBtn5;
        private Script_Browser.Controls.NoFocusBorderBtn noFocusBorderBtn3;
        private Script_Browser.Controls.NoFocusBorderBtn noFocusBorderBtn4;
        private Script_Browser.Controls.NoFocusBorderBtn noFocusBorderBtn2;
    }
}

