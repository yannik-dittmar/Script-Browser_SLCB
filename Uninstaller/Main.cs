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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uninstaller
{
    public partial class Main : Form
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

        public Main()
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Script Browser", true))
                    label3.Text = "SLCB Script-Browser v" + key.GetValue("DisplayVersion") + " © 2018 Digital-Programming";
            }
            catch { }

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
            if (noFocusBorderBtn2.Visible)
            {
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", String.Format("/c {0} & {1} & {2}", "timeout /T 1 /NOBREAK >NUL", "rmdir /s /q \"" + AppDomain.CurrentDomain.BaseDirectory + "\"", "exit"))
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                Process.Start(psi);
            }

            Environment.Exit(0);
        }

        #endregion

        //Uninstall
        private void noFocusBorderBtnNext_Click(object sender, EventArgs e)
        {
            tcTlp1.Tab(1);
            richTextBoxLog.Text = "=== Start Uninstall ===\n\n";
            new Thread(delegate() 
            {
                try
                {
                    Log("Removing registry entries...");
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\App Paths", true))
                        key.DeleteSubKey("Script Browser");
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
                        key.DeleteSubKey("Script Browser");
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                        key.DeleteValue("Script Browser");
                    Log("Removed registry entries");
                }
                catch { Log("Could not remove registry entries"); }

                try
                {
                    Log("Removing shortcuts...");
                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Script Browser.lnk"))
                        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Script Browser.lnk");
                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\Programs\\Script Browser.lnk"))
                        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\Programs\\Script Browser.lnk");
                    Log("Removed shortcut");
                }
                catch { Log("Could not remove shortcuts"); }

                try
                {
                    Log("Removing files...");
                    DirectoryInfo directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

                    foreach (FileInfo file in directory.GetFiles())
                        try { file.Delete(); } catch { }
                    foreach (DirectoryInfo dir in directory.GetDirectories())
                        try { dir.Delete(true); } catch { }
                    Log("Removed files");
                }
                catch { Log("Could not remove files"); }

                Log("\n=== End Uninstall ===");

                this.BeginInvoke(new MethodInvoker(delegate () { noFocusBorderBtn2.Visible = true; }));
            }).Start();
        }

        private void Log(string text)
        {
            IAsyncResult wait = this.BeginInvoke(new MethodInvoker(delegate ()
            {
                richTextBoxLog.AppendText(text + "\n");
                richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                richTextBoxLog.ScrollToCaret();
            }));
            while (!wait.IsCompleted)
                Thread.Sleep(50);
        }

        private void noFocusBorderBtn2_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", String.Format("/c {0} & {1} & {2}", "timeout /T 1 /NOBREAK >NUL", "rmdir /s /q \"" + AppDomain.CurrentDomain.BaseDirectory + "\"", "exit"))
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process.Start(psi);

            Environment.Exit(0);
        }
    }
}
