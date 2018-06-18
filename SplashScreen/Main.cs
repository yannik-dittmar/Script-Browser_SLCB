using Newtonsoft.Json.Linq;
using SaveManager;
using System;
using System.Collections.Generic;
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

        #endregion

        private Stopwatch sw = new Stopwatch();
        private SaveFile sf = new SaveFile(Path.GetDirectoryName(Application.ExecutablePath) + @"\settings.save");

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
                        while (changelog[0]["Version"].ToString() != sf.version)
                            changelog.RemoveAt(0);

                        changelog.RemoveAt(0);

                        List<string> files = new List<string>();
                        foreach (JToken change in changelog)
                        {
                            foreach (JToken file in (JArray)change["Files"])
                            {
                                if (!files.Contains(file.ToString()))
                                    files.Add(file.ToString());
                            }
                        }


                    }
                }
                catch { this.BeginInvoke(new MethodInvoker(delegate () { label1.Text = "Retry"; })); }
            }).Start();
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
    }
}
