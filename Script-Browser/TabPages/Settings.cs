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
        }

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
    }
}
