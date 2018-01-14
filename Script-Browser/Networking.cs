using MetroFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser
{
    static class Networking
    {
        public static string storageServer = "";
        public static string username = "";
        public static string password = "";

        static string Hash(string input)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }

        public static bool UpdateIp()
        {
            try
            {
                using (WebClient web = new WebClient())
                    storageServer = web.DownloadString("http://www.digital-programming.com/School-Assist/getIP.php");
                return true;
            }
            catch { return false; }
        }

        public static void CheckIp(Main form)
        {
            tryagain:
            if (storageServer == "")
            {
                if (!UpdateIp())
                {
                    if (DialogResult.Retry == MetroMessageBox.Show(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.\n\nCould not connect to mediation server!", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, 175))
                        goto tryagain;
                }
            }
        }

        public static void Login(string _username, string _password, Main form)
        {
            tryagain:
            try
            {
                form.settings1.progressBarEx1.Tag = "0";
                form.settings1.progressBarEx1.Value = 0;
                form.settings1.progressBarEx1.Visible = true;
                CheckIp(form);
                form.settings1.progressBarEx1.Tag = "50";

                using (WebClient web = new WebClient())
                {
                    if (web.DownloadString(storageServer + "/Script%20Browser/login.php?user=" + _username + "&pass=" + Hash(_password)).Contains("false"))
                        MetroMessageBox.Show(form, "The username or password was incorrect.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, 100);
                    else
                    {
                        username = _username;
                        password = _password;
                        form.settings1.label1.Text = "Logged in as " + username;

                        form.settings1.animator1.Hide(form.settings1.tableLayoutPanel1);
                        form.settings1.animator1.Show(form.settings1.tableLayoutPanel2);
                    }
                }
            }
            catch
            {
                if (DialogResult.Retry == MetroMessageBox.Show(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, 150))
                    goto tryagain;
            }
            form.settings1.progressBarEx1.Tag = "0";
        }

        public static string GetTopScripts(string type, string highest, int page, Main form)
        {
            CheckIp(form);
            using (WebClient web = new WebClient())
                return web.DownloadString(storageServer + "/Script%20Browser/getTopScripts.php?type=" + type + "&highest=" + highest + "&page=" + page);
        }
    }
}
