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

        private void timerProgressbar_Tick(object sender, EventArgs e)
        {
            try
            {
                if (progressBarEx1.Value < Int32.Parse(progressBarEx1.Tag.ToString()))
                    progressBarEx1.Value += 1;
                else if (progressBarEx1.Value > Int32.Parse(progressBarEx1.Tag.ToString()))
                    progressBarEx1.Value -= 1;
            }
            catch { }
        }

        private void roundedEdgesButton1_Click(object sender, EventArgs e)
        {
            if (loginUsername.Text.Trim(' ').Length != 0 && loginPass.Text.Trim(' ').Length != 0)
            {
                Networking.Login(loginUsername.Text, loginPass.Text, form);
            }
            else
                MetroMessageBox.Show(this, "Please make sure your entries are completely!", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, 125);
        }
    }
}
