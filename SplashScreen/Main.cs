using Newtonsoft.Json.Linq;
using SaveManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SplashScreen
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
        [DllImport("user32.dll")]
        static extern uint GetDoubleClickTime();

        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;

        #endregion

        private Stopwatch sw = new Stopwatch();
        private SaveFile sf = new SaveFile(Path.GetDirectoryName(Application.ExecutablePath) + @"\settings.save");
        private bool downloadingFile = false;

        public Main()
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            Start();
        }

        private void Start()
        {
            label1.Text = "Checking for Updates";
            new Thread(delegate ()
            {
                try
                {
                    if (!CheckUpdate())
                    {
                        this.BeginInvoke(new MethodInvoker(delegate () { label1.Text = "Updating - Downloading Files..."; }));

                        JArray changelog = GetChangelog();
                        while (changelog.First["version"].ToString() != sf.version)
                            changelog.RemoveAt(0);

                        changelog.RemoveAt(0);

                        JObject changes = GetChanges(changelog);
                        string directory = Path.GetDirectoryName(Application.ExecutablePath) + "\\test";

                        foreach (JToken addFolder in changes["folders"]["added"])
                        {
                            if (!Directory.Exists(directory + addFolder.ToString()))
                                Directory.CreateDirectory(directory + addFolder.ToString());
                        }

                        foreach (JToken removeFolder in changes["folders"]["removed"])
                            try { Directory.Delete(directory + removeFolder.ToString(), true); } catch { }

                        SetProgress(0);
                        using (WebClient web = new WebClient())
                        {
                            web.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadFileProgressChanged);
                            web.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
                            int counter = 1;
                            foreach (JToken addedFile in changes["files"]["changed"])
                            {
                                this.BeginInvoke(new MethodInvoker(delegate () { label1.Text = "Updating - Downloading Files (" + counter + "/" + ((JArray)changes["files"]["changed"]).Count + ")"; }));
                                downloadingFile = true;
                                web.DownloadFileAsync(new Uri("http://digital-programming.de/ScriptBrowser/bin" + addedFile), directory + addedFile);
                                while (downloadingFile)
                                    Thread.Sleep(50);
                                counter++;
                            }
                        }
                        SetProgress(0);

                        this.BeginInvoke(new MethodInvoker(delegate () { label1.Text = "Cleaning Up..."; }));
                        foreach (JToken removeFile in changes["files"]["removed"])
                            try { File.Delete(directory + removeFile); } catch { }

                        sf.version = changelog.Last["version"].ToString();
                        sf.Save();
                    }
                    this.BeginInvoke(new MethodInvoker(delegate () { label1.Text = "Starting Script-Browser"; }));
                }
                catch (Exception ex) { this.BeginInvoke(new MethodInvoker(delegate () { label1.Text = "Retry"; })); Console.WriteLine(ex.StackTrace); }
                SetProgress(0);
            }).Start();
        }

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            downloadingFile = false;
        }

        private void DownloadFileProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            SetProgress(e.ProgressPercentage);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (label1.Cursor == Cursors.Hand)
                Start();
        }

        private void label1_TextChanged(object sender, EventArgs e)
        {
            if (label1.Text == "Retry")
            {
                label1.BackColor = Color.FromArgb(51, 139, 118);
                label1.Cursor = Cursors.Hand;
            }
            else
            {
                label1.BackColor = Color.FromArgb(22, 36, 45);
                label1.Cursor = Cursors.Default;
            }
        }

        private JObject GetChanges(JArray changelog)
        {
            JObject changes = new JObject
            {
                ["files"] = new JObject
                {
                    ["changed"] = new JArray(),
                    ["removed"] = new JArray()
                },
                ["folders"] = new JObject
                {
                    ["added"] = new JArray(),
                    ["removed"] = new JArray()
                }
            };

            foreach (JToken change in changelog)
            {
                foreach (JToken fileChange in change["files"]["changed"])
                {
                    ((JArray)changes["files"]["changed"]).Add(fileChange.ToString());
                    if (((JArray)changes["files"]["removed"]).Contains(fileChange.ToString()))
                        ((JArray)changes["files"]["removed"]).Remove(fileChange.ToString());
                }

                foreach (JToken fileRemoved in change["files"]["removed"])
                {
                    ((JObject)changes["files"]).Remove(fileRemoved.ToString());
                    if (((JArray)changes["files"]["changed"]).Contains(fileRemoved.ToString()))
                        ((JArray)changes["files"]["changed"]).Remove(fileRemoved.ToString());
                }

                foreach (JToken folderAdded in change["folders"]["added"])
                {
                    ((JArray)changes["folders"]["added"]).Add(folderAdded.ToString());
                    if (((JArray)changes["folders"]["removed"]).Contains(folderAdded.ToString()))
                        ((JArray)changes["folders"]["removed"]).Remove(folderAdded.ToString());
                }

                foreach (JToken folderRemoved in change["folders"]["removed"])
                {
                    ((JArray)changes["folders"]["removed"]).Remove(folderRemoved.ToString());
                    if (((JArray)changes["folders"]["added"]).Contains(folderRemoved.ToString()))
                        ((JArray)changes["folders"]["added"]).Remove(folderRemoved.ToString());
                }
            }

            return changes;
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

        #endregion

        #region Networking

        private bool CheckUpdate()
        {
            using (WebClient web = new WebClient())
                return web.DownloadString("http://digital-programming.de/ScriptBrowser/version.txt") == sf.version;
        }

        private JArray GetChangelog()
        {
            using (WebClient web = new WebClient())
                return JArray.Parse(web.DownloadString("http://digital-programming.de/ScriptBrowser/changelog.txt"));
        }

        private void ApplyUpdate(List<string> files)
        {
            using (WebClient web = new WebClient())
            {
                foreach (string file in files)
                {
                    this.BeginInvoke(new MethodInvoker(delegate () { label1.Text = "Updating - Downloading Files (" + (files.IndexOf(file) + 1) + "/" + files.Count + ")"; }));
                    web.DownloadFile("http://digital-programming.de/ScriptBrowser/bin/" + file, Path.GetDirectoryName(Application.ExecutablePath) + "\\" + file);
                }
            }
        }

        #endregion

        #region Logo animation

        private float ParametricBlend(float t)
        {
            if (t > 1)
                return 1;
            float sqt = (float)Math.Pow(t, 2);
            return sqt / (2.0f * (sqt - t) + 1.0f);
        }

        private void timerSmall_Tick(object sender, EventArgs e)
        {
            panel1.Padding = new Padding(20 + (int)(ParametricBlend(sw.ElapsedMilliseconds / 2000.0f) * 40));
            if (panel1.Padding.All >= 60)
            {
                sw.Restart();
                timerSmall.Enabled = false;
                timerBig.Enabled = true;
            }
        }

        private void timerBig_Tick(object sender, EventArgs e)
        {
            panel1.Padding = new Padding(60 - (int)(ParametricBlend(sw.ElapsedMilliseconds / 2000.0f) * 40));
            if (panel1.Padding.All <= 20)
            {
                sw.Restart();
                timerSmall.Enabled = true;
                timerBig.Enabled = false;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            sw.Start();
        }

        #endregion

        #region Progressbar

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Column == 0)
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(51, 139, 118)), e.CellBounds);
            else
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(22, 36, 45)), e.CellBounds);
        }

        private void SetProgress(int prog)
        {
            this.BeginInvoke(new MethodInvoker(delegate () 
            {
                tableLayoutPanel1.ColumnStyles[0].Width = prog;
                tableLayoutPanel1.ColumnStyles[1].Width = 100 - prog;
            }));
        }

        #endregion
    }
}
