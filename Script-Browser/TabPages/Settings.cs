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
                    if (signupPass.Text == signupPassConfirm.Text)
                        Networking.SignUp(signupUsername.Text, signupPass.Text, signupEmail.Text, form);
                    else
                        MetroMessageBox.Show(this, "The password does not match the confirmation.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
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
    }
}
