using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uninstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "" + AppDomain.CurrentDomain.BaseDirectory;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //delete desktop shortcut
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            try
            {
                File.Delete(desktopPath + "\\Script-Browser.url");
            }
            catch {}

            //remove start menu shortcut
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Programs) + "\\Script-Browser"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Programs) + "\\Script-Browser" + "\\Script-Browser.url");
            }

            //uninstall script-Browser
            DirectoryInfo di = new DirectoryInfo("" + AppDomain.CurrentDomain.BaseDirectory);

            foreach (FileInfo file in di.GetFiles())
            {
                if (file.Name != "Uninstaller.exe")
                {
                    try {
                        file.Delete();
                    }
                    catch { }
                }
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            //delete uninstaller using cmd

            Process.Start("cmd.exe", "/C ping 1.1.1.1 -n 1 -w 3000 > Nul & Del " + Application.ExecutablePath);
            //Process.Start("cmd.exe", "/C ping 1.1.1.1 -n 1 -w 3000 > Nul & Del " + AppDomain.CurrentDomain.BaseDirectory);
            Application.Exit();
        }
    }
}
