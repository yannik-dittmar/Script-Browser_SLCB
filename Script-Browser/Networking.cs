using MetroFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        //Encrypt passwords
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
                CheckIp(form);

                using (WebClient web = new WebClient())
                {
                    if (web.DownloadString(storageServer + "/Script%20Browser/login.php?user=" + _username + "&pass=" + Hash(_password)).Contains("false"))
                        MetroMessageBox.Show(form, "The username or password was incorrect.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 100);
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
                if (DialogResult.Retry == MetroMessageBox.Show(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150))
                    goto tryagain;
            }
        }

        public static void SignUp(string _username, string _password, string email, Main form)
        {
            _password = Hash(_password);
            tryagain:
            try
            {
                CheckIp(form);

                using (WebClient web = new WebClient())
                {
                    string result = web.DownloadString(storageServer + "/Script%20Browser/signUp.php?username=" + _username + "&pass=" + _password + "&email=" + email);

                    if (result.Contains("username"))
                        MetroMessageBox.Show(form, "The username \"" + _username + "\" is allready registered!\nPlease select another one.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("email"))
                        MetroMessageBox.Show(form, "The email address \"" + email + "\" is allready registered!\nPlease select another one.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("blacklist"))
                        MetroMessageBox.Show(form, "The email address \"" + email + "\" is blacklisted!\nContact us under \"sl.chatbot.script.browser@gmail.com\" for more information.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("false"))
                        MetroMessageBox.Show(form, "There was an unexected sign up error.\nPlease try again later.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else
                    {
                        MetroMessageBox.Show(form, "You signed up successfully!\nA verification email has been send to your email account. Please check your inbox.", "Sign up success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 150);

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
                if (DialogResult.Retry == MetroMessageBox.Show(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150))
                    goto tryagain;
            }
        }

        public static string GetTopScripts(string type, string highest, int page, Main form)
        {
            CheckIp(form);
            using (WebClient web = new WebClient())
                return web.DownloadString(storageServer + "/Script%20Browser/getTopScripts.php?type=" + type + "&highest=" + highest + "&page=" + page);
        }

        public static string SearchScripts(string[] tags, Main form)
        {
            CheckIp(form);
            using (WebClient web = new WebClient())
                return web.DownloadString(storageServer + "/Script%20Browser/searchByTags.php?tags=" + JArray.FromObject(tags).ToString());
        }

        public static string GetScriptById(Main form, string id)
        {
            CheckIp(form);
            using (WebClient web = new WebClient())
                return web.DownloadString(storageServer + "/Script%20Browser/getScript.php?id=" + id);
        }

        public static Image DownloadImage(string path)
        {
            using (WebClient client = new WebClient())
            using (MemoryStream ms = new MemoryStream(client.DownloadData(path)))
                return Image.FromStream(ms);
        }
    }
}