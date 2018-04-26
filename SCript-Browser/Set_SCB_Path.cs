using MetroFramework;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser
{
    public partial class Set_SCB_Path : Form
    {
        #region DLL-Methodes & Variables

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;

        #endregion

        public Set_SCB_Path(string path)
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            noFocusBorderBtn2.NotEnabledBG = Color.FromArgb(25, 72, 70);
            noFocusBorderBtn2.Refresh();
            textBox1.Text = path;

            if (GetSLCBPath() != "")
                textBox1.Text = GetSLCBPath();
        }

        #region Windows API, Window Settings

        private void MoveForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion

        //Open Download Page
        private void label3_Click(object sender, EventArgs e)
        {
            try { Process.Start("https://streamlabs.com/chatbot"); } catch { }
        }

        //Check path
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            noFocusBorderBtn2.Enabled = CheckSLCBPath(textBox1.Text);
        }

        public static string GetSLCBPath()
        {
            try
            {
                string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
                {
                    foreach (string subkey_name in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                            try
                            {
                                if (subkey.GetValue("DisplayName") != null)
                                {
                                    if (subkey.GetValue("DisplayName").ToString().Contains("Streamlabs Chatbot"))
                                        return subkey.GetValue("InstallLocation").ToString();
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }
            return "";
        }

        public static bool CheckSLCBPath(string path)
        {
            return Directory.Exists(path + @"Services\Scripts") && File.Exists(path + "Streamlabs Chatbot.exe");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (CheckSLCBPath(folderBrowserDialog1.SelectedPath))
                    textBox1.Text = folderBrowserDialog1.SelectedPath;
                else
                    MetroMessageBox.Show(this, "Could not find a valid Streamlabs Chatbot installation!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 100);
            }
        }

        private void noFocusBorderBtn2_Click(object sender, EventArgs e)
        {
            if (CheckSLCBPath(textBox1.Text))
            {
                Main.sf.streamlabsPath = textBox1.Text;
                this.Dispose();
            }
        }
    }
}
