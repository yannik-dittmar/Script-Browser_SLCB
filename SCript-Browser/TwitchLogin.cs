using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Script_Browser
{
    public partial class TwitchLogin : Form
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

        private ChromiumWebBrowser web = new ChromiumWebBrowser();
        private bool loadinfinity = false;
        private Main form;

        public TwitchLogin(Main form)
        {
            InitializeComponent();
            this.form = form;

            panel1.Controls.Add(web);
            web.Dock = DockStyle.Fill;
            web.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEnd);
            web.LoadError += new EventHandler<LoadErrorEventArgs>(LoadError);
            web.AddressChanged += new EventHandler<AddressChangedEventArgs>(AddressChanged);
            web.Load(Path.GetDirectoryName(Application.ExecutablePath) + @"\HTML\LoadingTwitch.html");
        }

        private void FrameEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Url.Contains("LoadingTwitch.html") & !loadinfinity)
                web.Load("https://id.twitch.tv/oauth2/authorize?response_type=token&client_id=rutrjzb35smltearj9f57xbr31vov5&redirect_uri=http://localhost&scope=openid+user_read");
        }

        private void LoadError(object sender, LoadErrorEventArgs e)
        {
            try
            {
                if (e.FailedUrl.Contains("access_token"))
                {
                    loadinfinity = true;
                    web.Load(Path.GetDirectoryName(Application.ExecutablePath) + @"\HTML\LoadingTwitch.html");
                    Uri infos = new Uri(e.FailedUrl.Replace("#", "?"));
                    string token = HttpUtility.ParseQueryString(infos.Query).Get("access_token");
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Clear();
                        wc.Headers.Add("Accept", "application/vnd.twitchtv.v5+json");
                        wc.Headers.Add("Client-ID", "rutrjzb35smltearj9f57xbr31vov5");
                        wc.Headers.Add("Authorization", "OAuth " + token);
                        JObject account = JObject.Parse(wc.DownloadString("https://api.twitch.tv/kraken/user"));
                        if (!(bool)account["email_verified"])
                        {
                            this.DialogResult = DialogResult.No;
                            this.BeginInvoke(new MethodInvoker(delegate () { this.Dispose(); }));
                        }
                        else
                        {
                            this.BeginInvoke(new MethodInvoker(delegate () 
                            {
                                try
                                {
                                    Networking.SignUp(account["name"] + "", token, account["email"] + "", form, true, this);
                                }
                                catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
                                this.Dispose();
                            }));
                        }
                    }
                }
                else
                {
                    loadinfinity = false;
                    web.Load(Path.GetDirectoryName(Application.ExecutablePath) + @"\HTML\Error.html");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                loadinfinity = false;
                web.Load(Path.GetDirectoryName(Application.ExecutablePath) + @"\HTML\Error.html");
            }
        }

        private void AddressChanged(object sender, AddressChangedEventArgs e)
        {
            if (e.Address.Contains("discord"))
            {
                e.Browser.StopLoad();
                Process.Start("http://discord.gg/KDe7Vyu");
            }
            else if (e.Address == "https://id.twitch.tv/oauth2/authorize")
                this.BeginInvoke(new MethodInvoker(delegate () { this.Dispose(); }));
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

        private void TwitchLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            web.Dispose();
        }
    }
}
