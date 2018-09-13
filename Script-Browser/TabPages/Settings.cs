using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Script_Browser.TabPages
{
    public partial class Settings : UserControl
    {
        public Main form = null;

        public Settings()
        {
            InitializeComponent();
            loginUsername.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            loginPass.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            signupUsername.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            signupPass.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            signupPassConfirm.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            signupEmail.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;

            textBox1.Text = Main.sf.streamlabsPath;
            noFocusBorderBtn8.NotEnabledBG = Color.FromArgb(25, 72, 70);
            checkBox4.Checked = Main.sf.useUnverifiedScripts;
        }

        #region Account

        //Login
        private void roundedEdgesButton1_Click(object sender, EventArgs e)
        {
            if (loginUsername.Text.Trim(' ').Length != 0 && loginPass.Text.Trim(' ').Length != 0)
                Networking.Login(loginUsername.Text, loginPass.Text, form);
            else
                MetroMessageBox.Show(this, "Please make sure your entries are completely!", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
        }

        //Sign up
        private void roundedEdgesButton2_Click(object sender, EventArgs e)
        {
            if (signupUsername.Text.Trim(' ').Length != 0 && signupPass.Text.Trim(' ').Length != 0 && signupPassConfirm.Text.Trim(' ').Length != 0 && signupEmail.Text.Trim(' ').Length != 0 && IsValidEmail(signupEmail.Text))
            {
                if (signupPass.Text.Trim(' ').Length >= 6)
                {
                    if (Regex.IsMatch(signupUsername.Text, @"^[a-zA-Z0-9]+$"))
                    {
                        if (signupPass.Text == signupPassConfirm.Text)
                            Networking.SignUp(signupUsername.Text, signupPass.Text, signupEmail.Text, form);
                        else
                            MetroMessageBox.Show(this, "The password does not match the confirmation.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
                    }
                    else
                        MetroMessageBox.Show(this, "Please make sure your username only contains letters and numbers.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
                }
                else
                    MetroMessageBox.Show(this, "Your password has to be at least 6 characters long!", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
            }
            else
                MetroMessageBox.Show(this, "Please make sure your entries are completely and correct!", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
        }

        //Verify Email address
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        //Twitch Login
        private void flowLayoutPanel1_MouseEnter(object sender, EventArgs e)
        {
            flowLayoutPanel1.BackColor = Color.FromArgb(120, 78, 196);
        }

        private void flowLayoutPanel1_MouseLeave(object sender, EventArgs e)
        {
            flowLayoutPanel1.BackColor = Color.FromArgb(100, 65, 164);
        }

        private void TwitchLogin_MouseClick(object sender, MouseEventArgs e)
        {
            form.Opacity = 0.5;
            new TwitchLogin(form).ShowDialog();
            form.Opacity = 1;
        }

        //Forgot Password
        private void label8_Click(object sender, EventArgs e)
        {
            try { Process.Start("http://slcbsb.duckdns.org/Script%20Browser/forgotPassword.php"); } catch { }
        }

        //Send verification E-Mail again
        private void noFocusBorderBtn6_Click(object sender, EventArgs e)
        {
            Networking.SendVerificationAgain(form);
        }

        //Change Password
        private void noFocusBorderBtn4_Click(object sender, EventArgs e)
        {
            if (materialSingleLineTextField1.Text.Trim(' ').Length != 0 && materialSingleLineTextField2.Text.Trim(' ').Length != 0 && materialSingleLineTextField3.Text.Trim(' ').Length != 0)
            {
                if (materialSingleLineTextField3.Text.Trim(' ').Length >= 6)
                {
                    if (materialSingleLineTextField3.Text == materialSingleLineTextField1.Text)
                        Networking.ChangePass(materialSingleLineTextField2.Text, materialSingleLineTextField3.Text, form);
                    else
                        MetroMessageBox.Show(this, "The password does not match the confirmation.", "Could not change password", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
                }
                else
                    MetroMessageBox.Show(this, "Your new password has to be at least 6 characters long!", "Could not change password", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
            }
            else
                MetroMessageBox.Show(this, "Please make sure your entries are completely and correct!", "Could not change password", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
        }

        //Change Username
        private void noFocusBorderBtn5_Click(object sender, EventArgs e)
        {
            if (materialSingleLineTextField5.Text.Trim(' ').Length != 0 && materialSingleLineTextField4.Text.Trim(' ').Length != 0)
            {
                if (Regex.IsMatch(materialSingleLineTextField4.Text, @"^[a-zA-Z0-9]+$"))
                {
                    if (materialSingleLineTextField4.Text.ToLower() != Main.sf.username.ToLower())
                        Networking.ChangeUsername(materialSingleLineTextField4.Text, materialSingleLineTextField5.Text, form);
                    else
                        MetroMessageBox.Show(this, "Please select another username than your current one.", "Could not change username", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
                }
                else
                    MetroMessageBox.Show(this, "Please make sure your new username only contains letters and numbers.", "Could not change username", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
            }
            else
                MetroMessageBox.Show(this, "Please make sure your entries are completely and correct!", "Could not change username", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
        }

        //Change EMail
        private void noFocusBorderBtn7_Click(object sender, EventArgs e)
        {
            if (materialSingleLineTextField6.Text.Trim(' ').Length != 0 && materialSingleLineTextField7.Text.Trim(' ').Length != 0 && IsValidEmail(materialSingleLineTextField6.Text))
                Networking.ChangeEMail(materialSingleLineTextField6.Text, materialSingleLineTextField7.Text, form);
            else
                MetroMessageBox.Show(this, "Please make sure your entries are completely and correct!", "Could not change E-Mail address", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
        }

        //Logout
        private void noFocusBorderBtn1_Click(object sender, EventArgs e)
        {
            animator1.Hide(tableLayoutPanel2);
            animator1.Hide(tableLayoutPanel11);
            animator1.Show(tableLayoutPanel1);
            Main.sf.username = "";
            Main.sf.password = "";
            Main.sf.Save();
        }

        //Notify Settings
        private void noFocusBorderBtn8_Click(object sender, EventArgs e)
        {
            try
            {
                Networking.NotifySettings(form, checkBox1.Checked ? 1 : 0, checkBox2.Checked ? 1 : 0, checkBox3.Checked ? 1 : 0);
                noFocusBorderBtn8.Enabled = false;
            }
            catch { MetroMessageBox.Show(form, "Could't update your notification settings. Please try again later.", "Could update settings", MessageBoxButtons.OK, MessageBoxIcon.Error, 100); }
        }

        //Notify enable button
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            noFocusBorderBtn8.Enabled = true;
        }

        #endregion

        #region Streamlabs Chatbot

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderBrowserDialog1.SelectedPath += "\\";
                if (Set_SCB_Path.CheckSLCBPath(folderBrowserDialog1.SelectedPath))
                {
                    textBox1.Text = folderBrowserDialog1.SelectedPath;
                    Main.sf.streamlabsPath = folderBrowserDialog1.SelectedPath;
                }
                else
                    MetroMessageBox.Show(form, "Could not find a valid Streamlabs Chatbot installation!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 100);
            }
        }

        #endregion

        #region Scripts

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Main.sf.useUnverifiedScripts = checkBox4.Checked;
        }

        #endregion
    }
}
