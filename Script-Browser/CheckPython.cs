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
    public partial class CheckPython : Form
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

        private static Version installedVersion = new Version(0,0);

        public CheckPython()
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            if (installedVersion.CompareTo(new Version(0, 0)) != 0)
                label4.Text = "It seems like you have the wrong python version installed! (" + installedVersion + ")\nThis might cause trouble and scribts could not be working!\nYou can ignore this warning or download the newest\nversion with the button at the bottom!";
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
            this.Dispose();
        }

        #endregion

        public static PythonResult CheckPythonInstallation()
        {
            try
            {
                installedVersion = new Version(0, 0);
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\Python.exe");
                if (key == null)
                    key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\Python.exe");
                if (key == null)
                {
                    Protocol.AddToProtocol("Could not found Python!", Types.Warning);
                    return PythonResult.Nothing;
                }

                string pythonPath = key.GetValue(null).ToString();
                key.Close();
                ProcessStartInfo start = new ProcessStartInfo
                {
                    FileName = pythonPath,
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardError)
                    {
                        try
                        {
                            string result = reader.ReadToEnd().Replace("Python ", "").Replace("\n", "").Replace("\r", "");
                            Version ver = new Version(result);
                            Protocol.AddToProtocol("Python version " + ver + " detected. (" + pythonPath + ")", Types.Info);
                            installedVersion = ver;
                            if (ver.CompareTo(new Version(3,0)) < 0 && ver.CompareTo(new Version(2,0)) > 0)
                                return PythonResult.Ok;
                            else
                                return PythonResult.Wrong;
                        }
                        catch (Exception ex) { Console.WriteLine(ex.StackTrace); return PythonResult.Nothing; }
                    }
                }
            }
            catch (Exception ex) { Protocol.AddToProtocol("Could not define Python version! " + ex.Message + "\n" + ex.StackTrace, Types.Error); }
            return PythonResult.Error;
        }

        public enum PythonResult
        {
            Ok,
            Nothing,
            Wrong,
            Error
        }

        private void label3_Click(object sender, EventArgs e)
        {
            try { Process.Start("https://www.python.org/downloads/windows/"); } catch { }
        }

        private void noFocusBorderBtn2_Click(object sender, EventArgs e)
        {
            Main.sf.checkPythonVersion = !checkBox1.Checked;
            this.Dispose();
        }
    }
}
