namespace Script_Browser.TabPages
{
    partial class Settings
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            AnimatorNS.Animation animation1 = new AnimatorNS.Animation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.signupEmail = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.signupPass = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.signupUsername = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.signupPassConfirm = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.roundedEdgesButton2 = new Script_Browser.Controls.RoundedEdgesButton();
            this.roundedEdgesButton1 = new Script_Browser.Controls.RoundedEdgesButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.loginPass = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.loginUsername = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.roundedEdgesButton3 = new Script_Browser.Controls.RoundedEdgesButton();
            this.label1 = new System.Windows.Forms.Label();
            this.animator1 = new AnimatorNS.Animator(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.roundedEdgesButton2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.roundedEdgesButton1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.animator1.SetDecoration(this.tableLayoutPanel1, AnimatorNS.DecorationType.None);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(747, 235);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.signupEmail, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.signupPass, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.signupUsername, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.signupPassConfirm, 0, 2);
            this.animator1.SetDecoration(this.tableLayoutPanel4, AnimatorNS.DecorationType.None);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(383, 13);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(198, 165);
            this.tableLayoutPanel4.TabIndex = 12;
            // 
            // signupEmail
            // 
            this.signupEmail.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.animator1.SetDecoration(this.signupEmail, AnimatorNS.DecorationType.None);
            this.signupEmail.Depth = 0;
            this.signupEmail.Hint = "Email";
            this.signupEmail.Location = new System.Drawing.Point(3, 132);
            this.signupEmail.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.signupEmail.MouseState = MaterialSkin.MouseState.HOVER;
            this.signupEmail.Name = "signupEmail";
            this.signupEmail.PasswordChar = '\0';
            this.signupEmail.SelectedText = "";
            this.signupEmail.SelectionLength = 0;
            this.signupEmail.SelectionStart = 0;
            this.signupEmail.Size = new System.Drawing.Size(192, 23);
            this.signupEmail.TabIndex = 6;
            this.signupEmail.UseSystemPasswordChar = false;
            // 
            // signupPass
            // 
            this.signupPass.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.animator1.SetDecoration(this.signupPass, AnimatorNS.DecorationType.None);
            this.signupPass.Depth = 0;
            this.signupPass.Hint = "Password";
            this.signupPass.Location = new System.Drawing.Point(3, 46);
            this.signupPass.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.signupPass.MouseState = MaterialSkin.MouseState.HOVER;
            this.signupPass.Name = "signupPass";
            this.signupPass.PasswordChar = '•';
            this.signupPass.SelectedText = "";
            this.signupPass.SelectionLength = 0;
            this.signupPass.SelectionStart = 0;
            this.signupPass.Size = new System.Drawing.Size(192, 23);
            this.signupPass.TabIndex = 4;
            this.signupPass.UseSystemPasswordChar = false;
            // 
            // signupUsername
            // 
            this.signupUsername.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.animator1.SetDecoration(this.signupUsername, AnimatorNS.DecorationType.None);
            this.signupUsername.Depth = 0;
            this.signupUsername.Hint = "Username";
            this.signupUsername.Location = new System.Drawing.Point(3, 3);
            this.signupUsername.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.signupUsername.MouseState = MaterialSkin.MouseState.HOVER;
            this.signupUsername.Name = "signupUsername";
            this.signupUsername.PasswordChar = '\0';
            this.signupUsername.SelectedText = "";
            this.signupUsername.SelectionLength = 0;
            this.signupUsername.SelectionStart = 0;
            this.signupUsername.Size = new System.Drawing.Size(192, 23);
            this.signupUsername.TabIndex = 3;
            this.signupUsername.UseSystemPasswordChar = false;
            // 
            // signupPassConfirm
            // 
            this.signupPassConfirm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.animator1.SetDecoration(this.signupPassConfirm, AnimatorNS.DecorationType.None);
            this.signupPassConfirm.Depth = 0;
            this.signupPassConfirm.Hint = "Confirm Password";
            this.signupPassConfirm.Location = new System.Drawing.Point(3, 89);
            this.signupPassConfirm.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.signupPassConfirm.MouseState = MaterialSkin.MouseState.HOVER;
            this.signupPassConfirm.Name = "signupPassConfirm";
            this.signupPassConfirm.PasswordChar = '•';
            this.signupPassConfirm.SelectedText = "";
            this.signupPassConfirm.SelectionLength = 0;
            this.signupPassConfirm.SelectionStart = 0;
            this.signupPassConfirm.Size = new System.Drawing.Size(192, 23);
            this.signupPassConfirm.TabIndex = 5;
            this.signupPassConfirm.UseSystemPasswordChar = false;
            // 
            // roundedEdgesButton2
            // 
            this.roundedEdgesButton2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.roundedEdgesButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(72)))), ((int)(((byte)(70)))));
            this.roundedEdgesButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.animator1.SetDecoration(this.roundedEdgesButton2, AnimatorNS.DecorationType.None);
            this.roundedEdgesButton2.FlatAppearance.BorderSize = 0;
            this.roundedEdgesButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundedEdgesButton2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F);
            this.roundedEdgesButton2.ForeColor = System.Drawing.Color.White;
            this.roundedEdgesButton2.Location = new System.Drawing.Point(408, 191);
            this.roundedEdgesButton2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.roundedEdgesButton2.Name = "roundedEdgesButton2";
            this.roundedEdgesButton2.Size = new System.Drawing.Size(140, 41);
            this.roundedEdgesButton2.TabIndex = 7;
            this.roundedEdgesButton2.Text = "Sign up";
            this.roundedEdgesButton2.UseVisualStyleBackColor = false;
            this.roundedEdgesButton2.Click += new System.EventHandler(this.roundedEdgesButton2_Click);
            // 
            // roundedEdgesButton1
            // 
            this.roundedEdgesButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.roundedEdgesButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(72)))), ((int)(((byte)(70)))));
            this.roundedEdgesButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.animator1.SetDecoration(this.roundedEdgesButton1, AnimatorNS.DecorationType.None);
            this.roundedEdgesButton1.FlatAppearance.BorderSize = 0;
            this.roundedEdgesButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundedEdgesButton1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F);
            this.roundedEdgesButton1.ForeColor = System.Drawing.Color.White;
            this.roundedEdgesButton1.Location = new System.Drawing.Point(197, 191);
            this.roundedEdgesButton1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.roundedEdgesButton1.Name = "roundedEdgesButton1";
            this.roundedEdgesButton1.Size = new System.Drawing.Size(140, 41);
            this.roundedEdgesButton1.TabIndex = 2;
            this.roundedEdgesButton1.Text = "Login";
            this.roundedEdgesButton1.UseVisualStyleBackColor = false;
            this.roundedEdgesButton1.Click += new System.EventHandler(this.roundedEdgesButton1_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.loginPass, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.loginUsername, 0, 0);
            this.animator1.SetDecoration(this.tableLayoutPanel3, AnimatorNS.DecorationType.None);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(165, 59);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(198, 72);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // loginPass
            // 
            this.loginPass.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.animator1.SetDecoration(this.loginPass, AnimatorNS.DecorationType.None);
            this.loginPass.Depth = 0;
            this.loginPass.Hint = "Password";
            this.loginPass.Location = new System.Drawing.Point(3, 46);
            this.loginPass.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.loginPass.MouseState = MaterialSkin.MouseState.HOVER;
            this.loginPass.Name = "loginPass";
            this.loginPass.PasswordChar = '•';
            this.loginPass.SelectedText = "";
            this.loginPass.SelectionLength = 0;
            this.loginPass.SelectionStart = 0;
            this.loginPass.Size = new System.Drawing.Size(192, 23);
            this.loginPass.TabIndex = 1;
            this.loginPass.UseSystemPasswordChar = false;
            // 
            // loginUsername
            // 
            this.loginUsername.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.animator1.SetDecoration(this.loginUsername, AnimatorNS.DecorationType.None);
            this.loginUsername.Depth = 0;
            this.loginUsername.Hint = "Username";
            this.loginUsername.Location = new System.Drawing.Point(3, 3);
            this.loginUsername.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.loginUsername.MouseState = MaterialSkin.MouseState.HOVER;
            this.loginUsername.Name = "loginUsername";
            this.loginUsername.PasswordChar = '\0';
            this.loginUsername.SelectedText = "";
            this.loginUsername.SelectionLength = 0;
            this.loginUsername.SelectionStart = 0;
            this.loginUsername.Size = new System.Drawing.Size(192, 23);
            this.loginUsername.TabIndex = 0;
            this.loginUsername.UseSystemPasswordChar = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.animator1.SetDecoration(this.panel1, AnimatorNS.DecorationType.None);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(747, 453);
            this.panel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.roundedEdgesButton3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.animator1.SetDecoration(this.tableLayoutPanel2, AnimatorNS.DecorationType.None);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 235);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(747, 74);
            this.tableLayoutPanel2.TabIndex = 1;
            this.tableLayoutPanel2.Visible = false;
            // 
            // roundedEdgesButton3
            // 
            this.roundedEdgesButton3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.roundedEdgesButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(72)))), ((int)(((byte)(76)))));
            this.roundedEdgesButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.animator1.SetDecoration(this.roundedEdgesButton3, AnimatorNS.DecorationType.None);
            this.roundedEdgesButton3.FlatAppearance.BorderSize = 0;
            this.roundedEdgesButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundedEdgesButton3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F);
            this.roundedEdgesButton3.ForeColor = System.Drawing.Color.White;
            this.roundedEdgesButton3.Location = new System.Drawing.Point(164, 10);
            this.roundedEdgesButton3.Margin = new System.Windows.Forms.Padding(20, 10, 3, 3);
            this.roundedEdgesButton3.Name = "roundedEdgesButton3";
            this.roundedEdgesButton3.Size = new System.Drawing.Size(140, 41);
            this.roundedEdgesButton3.TabIndex = 3;
            this.roundedEdgesButton3.Text = "Logout";
            this.roundedEdgesButton3.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.animator1.SetDecoration(this.label1, AnimatorNS.DecorationType.None);
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(10, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 10, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Logged in as ";
            // 
            // animator1
            // 
            this.animator1.AnimationType = AnimatorNS.AnimationType.VertSlide;
            this.animator1.Cursor = null;
            animation1.AnimateOnlyDifferences = true;
            animation1.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.BlindCoeff")));
            animation1.LeafCoeff = 0F;
            animation1.MaxTime = 1F;
            animation1.MinTime = 0F;
            animation1.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicCoeff")));
            animation1.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicShift")));
            animation1.MosaicSize = 0;
            animation1.Padding = new System.Windows.Forms.Padding(0);
            animation1.RotateCoeff = 0F;
            animation1.RotateLimit = 0F;
            animation1.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.ScaleCoeff")));
            animation1.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.SlideCoeff")));
            animation1.TimeCoeff = 0F;
            animation1.TransparencyCoeff = 0F;
            this.animator1.DefaultAnimation = animation1;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(36)))), ((int)(((byte)(45)))));
            this.Controls.Add(this.panel1);
            this.animator1.SetDecoration(this, AnimatorNS.DecorationType.None);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(747, 453);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private MaterialSkin.Controls.MaterialSingleLineTextField loginPass;
        private MaterialSkin.Controls.MaterialSingleLineTextField loginUsername;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private MaterialSkin.Controls.MaterialSingleLineTextField signupEmail;
        private MaterialSkin.Controls.MaterialSingleLineTextField signupPass;
        private MaterialSkin.Controls.MaterialSingleLineTextField signupUsername;
        private MaterialSkin.Controls.MaterialSingleLineTextField signupPassConfirm;
        private Controls.RoundedEdgesButton roundedEdgesButton1;
        private Controls.RoundedEdgesButton roundedEdgesButton2;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Controls.RoundedEdgesButton roundedEdgesButton3;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public AnimatorNS.Animator animator1;
        public System.Windows.Forms.Label label1;
    }
}
