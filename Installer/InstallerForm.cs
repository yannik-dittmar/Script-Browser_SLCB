using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Script_Browser.Controls;

namespace Installer
{
    public partial class InstallerForm : Form
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

        private string version = "1.0.0";

        private WebClient web;
        private long receivedBytes = 0;
        private Stopwatch sw = new Stopwatch();
        private double averageSpeed = 0;
        private bool downloadSuccess = false;

        public InstallerForm()
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            tcTlp1.Tab(0);
            label3.Text = "SLCB Script-Browser v" + version + " © 2018 Digital-Programming";

            richTextBox2.SelectAll();
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.Select(0, 70);
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;

            try { File.Delete(Environment.CurrentDirectory + @"\Script-Browser Setup.exe"); } catch { }
        }

        #region Windows API, Window Settings

        private void label2_Click(object sender, EventArgs e)
        {
            try
            {
                if (web.IsBusy)
                {
                    if (MessageBox.Show("Do you really want to abort the installation?", "Abort Installation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        web.CancelAsync();
                        try { Directory.Delete(textBox1.Text, true); } catch { }
                        Environment.Exit(0);
                    }
                }
                else
                    Environment.Exit(0);
            }
            catch { Environment.Exit(0); }
        }

        private void MoveForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void InstallerForm_Load(object sender, EventArgs e)
        {
            ShapeArrow(noFocusBorderBtn1, 0);
            ShapeArrow(noFocusBorderBtn2, 1);
            ShapeArrow(noFocusBorderBtn3, 1);
            ShapeArrow(noFocusBorderBtn4, 1);
            ShapeArrow(noFocusBorderBtn5, 2);

            textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SLCB Script-Browser\\";
        }

        private void ShapeArrow(Control btn, int pos)
        {
            Size b = btn.Bounds.Size;
            Point l = btn.Location;
            List<Point> pts = new List<Point>{
                new Point(0, 0),
                new Point(b.Width - 15, 0),
                new Point(b.Width, (b.Height / 2)),
                new Point(b.Width - 15, b.Height),
                new Point(0, b.Height),
                new Point(15, (b.Height / 2))
            };

            if (pos == 0)
                pts.RemoveAt(5);
            else if (pos == 2)
            {
                pts[1] = new Point(b.Width, 0);
                pts[3] = new Point(b.Width, b.Height);
                pts.RemoveAt(2);
            }

            using (GraphicsPath polygon_path = new GraphicsPath(FillMode.Winding))
            {
                polygon_path.AddPolygon(pts.ToArray());
                btn.Region = new Region(polygon_path);
                btn.Location = l;
            }
        }

        #endregion

        #region TabControls

        private void noFocusBorderBtnNext_Click(object sender, EventArgs e)
        {
            if (tcTlp2.currentTab == 4)
            {
                tcTlp1.Tab(1);
                PrepareDownload();
            }
            else
            {
                tcTlp2.NextTab(noFocusBorderBtnBack, noFocusBorderBtnNext);
                UpdateNavbar();
            }
        }

        private void noFocusBorderBtnBack_Click(object sender, EventArgs e)
        {
            tcTlp2.PrevTab(noFocusBorderBtnBack, noFocusBorderBtnNext);
            UpdateNavbar();
        }

        private void Navbar_Click(object sender, EventArgs e)
        {
            tcTlp2.Tab(Int32.Parse((sender as Control).Tag.ToString()), noFocusBorderBtnBack, noFocusBorderBtnNext);
            UpdateNavbar();
        }

        private void UpdateNavbar()
        {
            List<NoFocusBorderBtn> navButtons = new List<NoFocusBorderBtn>
            {
                noFocusBorderBtn1,
                noFocusBorderBtn2,
                noFocusBorderBtn3,
                noFocusBorderBtn4,
                noFocusBorderBtn5
            };

            foreach (NoFocusBorderBtn btn in navButtons)
            {
                if (Int32.Parse(btn.Tag.ToString()) == tcTlp2.currentTab)
                    btn.BackColor = Color.FromArgb(51, 139, 118);
                else
                    btn.BackColor = Color.FromArgb(25, 72, 70);
            }
        }

        private void Check(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                tcTlp2.allowed = 2;
                noFocusBorderBtn3.Enabled = true;
                noFocusBorderBtn4.Enabled = checkBox2.Checked;
                noFocusBorderBtn5.Enabled = checkBox2.Checked;
                if (checkBox2.Checked)
                    tcTlp2.allowed = 4;
            }
            else
            {
                tcTlp2.allowed = 1;
                noFocusBorderBtn3.Enabled = false;
                noFocusBorderBtn4.Enabled = false;
                noFocusBorderBtn5.Enabled = false;
            }

            tcTlp2.Tab(tcTlp2.currentTab, noFocusBorderBtnBack, noFocusBorderBtnNext);
            UpdateNavbar();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (folderBrowserDialog1.SelectedPath.ToCharArray()[folderBrowserDialog1.SelectedPath.ToCharArray().Length - 1] == '\\')
                    textBox1.Text = folderBrowserDialog1.SelectedPath + "SLCB Script-Browser\\";
                else
                    textBox1.Text = folderBrowserDialog1.SelectedPath + "\\SLCB Script-Browser\\";
            }
        }

        #region Progress

        private void tableLayoutPanel9_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Column == 0)
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(25, 72, 70)), e.CellBounds);
        }

        private void SetProg(int per)
        {
            IAsyncResult wait = this.BeginInvoke(new MethodInvoker(delegate () 
            { 
                if (per >= 0)
                {
                    tableLayoutPanel9.ColumnStyles[0].Width = per;
                    tableLayoutPanel9.ColumnStyles[1].Width = 100 - per;
                    tableLayoutPanel9.ColumnStyles[2].Width = 0;
                    noFocusBorderBtn6.Visible = false;
                }
                else
                {
                    tableLayoutPanel9.ColumnStyles[0].Width = 0;
                    tableLayoutPanel9.ColumnStyles[1].Width = 0;
                    tableLayoutPanel9.ColumnStyles[2].Width = 100;
                    noFocusBorderBtn6.Visible = true;
                    labelSpeed.Visible = false;
                }

                labelSpeed.Visible = per != 0 && per != -1;
            }));
        }

        #endregion

        #region Prepare, Download and Install

        public void PrepareDownload()
        {
            richTextBoxLog.Text = "=== Start Installation ===\n";
            richTextBoxLog.AppendText("\n=== Start Preperation ===\n\n");
            labelStatus.Text = "Preparing installation";
            noFocusBorderBtn7.Visible = false;
            SetProg(0);

            new Thread(delegate ()
            {
                //File Permission Check
                Log("Testing file permissions...");
                try
                {
                    if (!Directory.Exists(textBox1.Text))
                        Directory.CreateDirectory(textBox1.Text);

                    string[] files = Directory.GetFiles(textBox1.Text);
                    foreach (string file in files)
                        File.Delete(file);

                    string[] directorys = Directory.GetDirectories(textBox1.Text);
                    foreach (string dir in directorys)
                        Directory.Delete(dir, true);

                    File.WriteAllText(textBox1.Text + "test.txt", "test file");
                    File.Delete(textBox1.Text + "test.txt");
                }
                catch (Exception ex)
                {
                    ShowError("Installation Failed: Could not write to or create directory!\nPlease check permissions and run as administrator if necessary.\n\nException:"
                        + ex.Message + "\n" + ex.StackTrace);
                    return;
                }
                Log("Tested file permissions");

                //Connection check
                Log("Testing internet connection...");
                if (!PingHost("www.digital-programming.de"))
                {
                    ShowError("Installation Failed: Could not connect to server!\nPlease check your internet connection.", true);
                    return;
                }
                Log("Tested internet connection");

                Log("Receiving file size...");
                string fileSize = FileSize("http://digital-programming.de/ScriptBrowser/setup.zip");
                if (fileSize == "ERROR")
                {
                    ShowError("Installation Failed: Could not receive file size!\nPlease check your internet connection.", true);
                    return;
                }
                Log("Received file size");

                Log("\n=== End Preperation ===");
                this.BeginInvoke(new MethodInvoker(delegate () { labelStatus.Text = "Downloading Script-Browser"; }));
                Log("\n=== Start Download ===\n");

                //Download

                web = new WebClient();
                web.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                web.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                receivedBytes = 0;
                sw.Restart();
                downloadSuccess = false;
                Log("Downloading Script-Browser (" + fileSize + ")...");
                web.DownloadFileAsync(new Uri("http://digital-programming.de/ScriptBrowser/setup.zip"), textBox1.Text + "SB.zip");

                while (web.IsBusy)
                    Thread.Sleep(50);
                web.Dispose();

                if (downloadSuccess)
                {
                    this.BeginInvoke(new MethodInvoker(delegate () { labelSpeed.Text = "Download Finished"; }));
                    Log("Download complete!");
                    Log("\n=== End Download ===");
                    this.BeginInvoke(new MethodInvoker(delegate () { labelStatus.Text = "Extracting Script-Browser"; }));
                    Log("\n=== Start Extraction ===\n");

                    try
                    {
                        Log("Extracting zip-archive...");
                        ZipFile.ExtractToDirectory(textBox1.Text + "SB.zip", textBox1.Text);
                        try { File.Delete(textBox1.Text + "SB.zip"); } catch { }
                        Log("Extracted zip-archive");
                    }
                    catch (Exception ex)
                    {
                        ShowError("Installation Failed: Could not extract zip-archive!\n\nException: " + ex.Message + "\n" + ex.StackTrace);
                        return;
                    }

                    Log("\n=== End Extraction ===");
                    this.BeginInvoke(new MethodInvoker(delegate () { labelStatus.Text = "Finalising"; }));
                    Log("\n=== Start Finalisation ===\n");

                    Log("Register application...");

                    //Registration
                    try
                    {
                        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion", true))
                        {
                            using (RegistryKey checkKey = key.OpenSubKey(@"App Paths", true))
                            {
                                if (checkKey == null)
                                    key.CreateSubKey("App Paths");

                                using (RegistryKey appKey = checkKey.OpenSubKey("Script Browser", true) ?? checkKey.CreateSubKey("Script Browser"))
                                {
                                    appKey.SetValue("", textBox1.Text + "Script-Browser.exe");
                                    appKey.SetValue("Path", textBox1.Text + "Script-Browser.exe");
                                }
                            }
                        }
                        Log("Registed application");
                    }
                    catch { Log("Could not register application!"); }

                    //Uninstall
                    Log("Register uninstaller...");
                    using (RegistryKey parent = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
                    {
                        try
                        {
                            RegistryKey key = null;

                            try
                            {
                                string guidText = "Script Browser";
                                key = parent.OpenSubKey(guidText, true) ??
                                      parent.CreateSubKey(guidText);

                                key.SetValue("DisplayName", "Script Browser");
                                key.SetValue("ApplicationVersion", version);
                                key.SetValue("Publisher", "Digital-Programming");
                                key.SetValue("DisplayIcon", textBox1.Text + "Script-Browser.exe,0");
                                key.SetValue("DisplayVersion", version);
                                key.SetValue("URLInfoAbout", "http://www.digital-programming.com");
                                key.SetValue("Contact", "sl.chatbot.script.browser@gmail.com");
                                key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                                key.SetValue("InstallLocation", textBox1.Text);
                                key.SetValue("UninstallString", textBox1.Text + "Uninstaller.exe");
                                Log("Registed uninstaller");
                            }
                            finally
                            {
                                if (key != null)
                                    key.Close();
                            }
                        }
                        catch { Log("Could not create uninstaller"); }
                    }

                    //Startup
                    try
                    { 
                        if (checkBox3.Checked)
                        {
                            Log("Register application to startup...");
                            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                                key.SetValue("Script Browser", textBox1.Text + "Script-Browser.exe /hide");
                            Log("Registed application to startup");
                        }
                    }
                    catch { Log("Could not register application to startup"); }

                    //Shortcut
                    try
                    {
                        if (checkBox4.Checked)
                        {
                            Log("Creating shortcut...");
                            IWshRuntimeLibrary.WshShell wsh = new IWshRuntimeLibrary.WshShell();
                            IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Script Browser.lnk") as IWshRuntimeLibrary.IWshShortcut;
                            shortcut.Arguments = "";
                            shortcut.TargetPath = textBox1.Text + "Script-Browser.exe";
                            shortcut.WindowStyle = 1;
                            shortcut.Description = "Streamlabs Chatbot Script-Browser";
                            shortcut.WorkingDirectory = textBox1.Text;
                            shortcut.IconLocation = textBox1.Text + "Script-Browser.exe,0";
                            shortcut.Save();
                            Log("Created shortcut");
                        }
                    }
                    catch { Log("Could not create shortcut"); }

                    //Start Menu
                    try
                    {
                        if (checkBox7.Checked)
                        {
                            Log("Adding application to start menu...");
                            IWshRuntimeLibrary.WshShell wsh = new IWshRuntimeLibrary.WshShell();
                            IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\Programs\\Script Browser.lnk") as IWshRuntimeLibrary.IWshShortcut;
                            shortcut.Arguments = "";
                            shortcut.TargetPath = textBox1.Text + "Script-Browser.exe";
                            shortcut.WindowStyle = 1;
                            shortcut.Description = "Streamlabs Chatbot Script-Browser";
                            shortcut.WorkingDirectory = textBox1.Text;
                            shortcut.IconLocation = textBox1.Text + "Script-Browser.exe,0";
                            shortcut.Save();
                            Log("Added application to start menu");
                        }
                    }
                    catch { Log("Could not add application to start menu"); }

                    this.BeginInvoke(new MethodInvoker(delegate () { noFocusBorderBtn7.Visible = true; }));
                    Log("\n=== End Finalisation ===");
                    Log("\n=== End Installation ===");
                }
            }).Start();
        }

        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            IAsyncResult wait = this.BeginInvoke(new MethodInvoker(delegate () { 
                if (sw.ElapsedMilliseconds != 0 && sw.ElapsedMilliseconds > 1000)
                {
                    long bytes = e.BytesReceived - receivedBytes;
                    double speed = Math.Round((double)bytes / ((double)sw.ElapsedMilliseconds / 1000.0));

                    speed = (averageSpeed * 0.7) + (speed * 0.3);
                    averageSpeed = speed;

                    string show = "";

                    //MBs
                    if (speed > 1000000)
                        show = Math.Round(speed / 1000000.0, 1) + " MB/s";
                    //KBs
                    else if (speed > 1000)
                        show = Math.Round(speed / 1000.0, 1) + " KB/s";
                    //Bs
                    else
                        show = speed + " B/s";
                    sw.Restart();
                    receivedBytes = e.BytesReceived;

                    if (TextRenderer.MeasureText(show, labelSpeed.Font).Width + 20 < labelSpeed.Width)
                        labelSpeed.Text = show;
                    else
                        labelSpeed.Text = "";
                }

                SetProg(e.ProgressPercentage);
            }));
            while (!wait.IsCompleted)
                Thread.Sleep(50);
        }

        private void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate () 
            {
                downloadSuccess = false;
                if (e.Error != null)
                {
                    richTextBoxLog.AppendText("Installation Failed: Script Browser couldn't be downloaded!\nPlease check your internet connection and try again.\n\nException:" + e.Error.Message);
                    labelStatus.Text = "Installation Failed";

                    noFocusBorderBtn6.Visible = true;
                    SetProg(-1);
                }
                else if (!e.Cancelled)
                    downloadSuccess = true;
                sw.Stop();
            }));
        }

        public static bool PingHost(string nameOrAddress)
        {
            try
            {
                using (Ping pinger = new Ping())
                {
                    PingReply reply = pinger.Send(nameOrAddress, 5000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return false;
        }

        private string FileSize(string file)
        {
            WebRequest req = HttpWebRequest.Create(file);
            req.Method = "HEAD";
            WebResponse resp = req.GetResponse();

            long ContentLength = 0;
            string result;
            if (long.TryParse(resp.Headers.Get("Content-Length"), out ContentLength))
            {
                if (ContentLength >= 1073741824)
                    result = (ContentLength / 1073741824) + " GB";
                else if (ContentLength >= 1048576)
                    result = (ContentLength / 1048576) + " MB";
                else
                    result = (ContentLength / 1024) + " KB";

                return result;
            }
            return "ERROR";
        }

        private void Log(string text)
        {
            IAsyncResult wait = this.BeginInvoke(new MethodInvoker(delegate()
            {
                richTextBoxLog.AppendText(text + "\n");
                richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                richTextBoxLog.ScrollToCaret();
            }));
            while (!wait.IsCompleted)
                Thread.Sleep(50);
        }

        private void ShowError(string text, bool retry = false)
        {
            this.BeginInvoke(new MethodInvoker(delegate () { labelStatus.Text = "Installation Failed"; }));
            Log(text + "\n\n\nPlease contact us if this error remains.\n" 
                + "Discord: http://discord.gg/KDe7Vyu (ENG)     http://discord.gg/zMmbYeh (GER)\n"
                + "Website: http://digital-programming.com \n"
                + "E-Mail: sl.chatbot.script.browser@gmail.com");

            if (retry)
                SetProg(-1);
        }

        private void noFocusBorderBtn6_Click(object sender, EventArgs e)
        {
            if (noFocusBorderBtn6.Visible)
            {
                PrepareDownload();
            }
        }

        private void richTextBoxLog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try { Process.Start(e.LinkText); } catch { }
        }

        private void noFocusBorderBtn7_Click(object sender, EventArgs e)
        {
            tcTlp1.Tab(2);
        }


        #endregion

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try { Process.Start((sender as Control).Tag.ToString()); } catch { }
        }

        private void noFocusBorderBtn8_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBox5.Checked)
                    Process.Start(textBox1.Text + "Script-Browser.exe");
            }
            catch { }
            try
            {
                if (checkBox6.Checked)
                {
                    ProcessStartInfo Info = new ProcessStartInfo
                    {
                        Arguments = "/C choice /C Y /N /D Y /T 3 & Del " +
                                   Application.ExecutablePath,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                        FileName = "cmd.exe"
                    };
                    Process.Start(Info);
                }
            }
            catch { }
            Environment.Exit(1);
        }
    }
}
