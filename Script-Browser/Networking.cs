using MetroFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser
{
    static class Networking
    {
        public static string storageServer = "";
        //public static string username = "krypto";
        //public static string password = "a94a8fe5ccb19ba61c4c0873d391e987982fbbd3";
        public static bool twitch = false;
        public static ObservableCollection<KeyValuePair<string, string>> checkUpdate = new ObservableCollection<KeyValuePair<string, string>>();

        //Encrypt passwords
        static string Hash(string input)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }

        #region Network Settings

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

        public static bool CheckIp(Form form)
        {
            tryagain:
            if (storageServer == "" || !NetworkReady())
            {
                if (!UpdateIp())
                {
                    if (DialogResult.Retry == SMB(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.\n\nCould not connect to mediation server!", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, 4, DialogResult.Retry))
                        goto tryagain;
                    else
                        return false;
                }
            }
            return true;
        }

        public static bool NetworkReady()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(storageServer.Replace("https://", "").Replace(":443", ""), 3000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
            return false;
        }

        #endregion

        #region Account

        public static JObject Login(string _username, string _password, Main form, bool hashed = false, JObject info = null)
        {
            if (!hashed)
                _password = Hash(_password);

            tryagain:
            try
            {
                if (info == null)
                    CheckIp(form);

                using (WebClient web = new WebClient())
                {
                    string result = "";
                    if (info == null)
                        result = web.DownloadString(storageServer + "/Script%20Browser/login.php?user=" + _username + "&pass=" + _password + "&getinfo=true");
                    if (!result.Contains("Twitch") && info == null)
                    {
                        MetroMessageBox.Show(form, "The username or password was incorrect.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 100);
                        Main.sf.username = "";
                        Main.sf.password = "";
                        Main.sf.Save();
                    }
                    else
                    {
                        try
                        {
                            if (info == null)
                                info = JObject.Parse(result);
                            twitch = (bool)info["Twitch"];
                            form.settings1.label1.Text = "Logged in as " + Main.sf.username;

                            form.settings1.checkBox1.Checked = (int)info["Notifys"]["NewCom"] == 1;
                            form.settings1.checkBox2.Checked = (int)info["Notifys"]["NewReply"] == 1;
                            form.settings1.checkBox3.Checked = (int)info["Notifys"]["NewBug"] == 1;
                            form.settings1.noFocusBorderBtn8.Enabled = false;

                            if (hashed)
                            {
                                form.settings1.tableLayoutPanel1.Visible = false;
                                form.settings1.tableLayoutPanel2.Visible = true;
                                form.settings1.tableLayoutPanel11.Visible = true;
                            }
                            else
                            {
                                form.settings1.animator1.HideSync(form.settings1.tableLayoutPanel1);
                                form.settings1.animator1.Show(form.settings1.tableLayoutPanel2);
                                form.settings1.animator1.Show(form.settings1.tableLayoutPanel11);
                            }

                            form.settings1.noFocusBorderBtn6.Visible = !(bool)info["Verified"];

                            foreach (JToken i in info["Scripts"] as JArray)
                                Main.sf.accountScripts.Add(i.ToString());

                            Main.sf.username = info["Username"].ToString();
                            Main.sf.password = _password;
                            Main.sf.Save();
                        }
                        catch { return JObject.Parse(result); }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                if (DialogResult.Retry == SMB(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 2, DialogResult.Retry))
                    goto tryagain;
            }
            return null;
        }

        public static void SignUp(string _username, string _password, string email, Main form, bool twitch = false, TwitchLogin webForm = null)
        {
            if (!twitch)
                _password = Hash(_password);

            tryagain:
            try
            {
                CheckIp(form);

                using (WebClient web = new WebClient())
                {
                    string result = "";
                    if (twitch)
                    {
                        result = web.DownloadString(storageServer + "/Script%20Browser/signUpTwitch.php?token=" + _password);
                        webForm.Dispose();
                    }
                    else
                        result = web.DownloadString(storageServer + "/Script%20Browser/signUp.php?username=" + _username + "&pass=" + _password + "&email=" + email);

                    if (result.Contains("username"))
                        MetroMessageBox.Show(form, "The username \"" + _username + "\" is allready registered!\nPlease select another one.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("email"))
                        MetroMessageBox.Show(form, "The email address \"" + email + "\" is allready registered!\nPlease select another one.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("blacklist"))
                        MetroMessageBox.Show(form, "The email address \"" + email + "\" is blacklisted!\nContact us under \"sl.chatbot.script.browser@gmail.com\" for more information.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("false"))
                        MetroMessageBox.Show(form, "There was an unexected sign up error.\nPlease try again later.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("verify"))
                        MetroMessageBox.Show(form, "Your Twitch account wasn't verified yet!\nPlease verify your email address.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else
                    {
                        if (!twitch)
                            MetroMessageBox.Show(form, "You signed up successfully!\nA verification email has been send to your email account. Please check your inbox.", "Sign up success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 150);

                        Main.sf.username = _username;
                        if (twitch)
                            Main.sf.password = Hash(_password);
                        else
                            Main.sf.password = _password;
                        Networking.twitch = twitch;
                        form.settings1.label1.Text = "Logged in as " + Main.sf.username;

                        form.settings1.tableLayoutPanel7.Visible = !twitch;
                        form.settings1.tableLayoutPanel9.Visible = !twitch;
                        form.settings1.tableLayoutPanel10.Visible = !twitch;
                        form.settings1.noFocusBorderBtn6.Visible = !twitch;

                        form.settings1.animator1.Hide(form.settings1.tableLayoutPanel1);
                        form.settings1.animator1.Show(form.settings1.tableLayoutPanel2);
                        form.settings1.animator1.Show(form.settings1.tableLayoutPanel11);
                        form.settings1.noFocusBorderBtn8.Enabled = false;

                        form.settings1.checkBox1.Checked = true;
                        form.settings1.checkBox2.Checked = true;
                        form.settings1.checkBox3.Checked = true;

                        Main.sf.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                try { webForm.Dispose(); } catch { }
                if (DialogResult.Retry == SMB(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 2, DialogResult.Retry))
                    goto tryagain;
            }
        }

        public static void SendVerificationAgain(Main form)
        {
            tryagain:
            try
            {
                CheckIp(form);

                using (WebClient web = new WebClient())
                {
                    string result = web.DownloadString(storageServer + "/Script%20Browser/sendVerificationAgain.php?user=" + Main.sf.username + "&pass=" + Main.sf.password);
                    if (result.Contains("verfied"))
                    {
                        MetroMessageBox.Show(form, "Your account has already been verified!", "Could not send verification E-Mail again", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
                        form.settings1.noFocusBorderBtn6.Visible = false;
                    }
                    else if (result.Contains("time"))
                        MetroMessageBox.Show(form, "You can only request a verification E-Mail once every 5 minuets.\nPlease wait...", "Could not send verification E-Mail again", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("false"))
                        MetroMessageBox.Show(form, "Please try it later again.", "Could not send verification E-Mail again", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
                    else
                        MetroMessageBox.Show(form, "We send a new verification E-Mail to your address!\nPlease check your inbox.", "Send verification E - Mail again", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 150);
                }
            }
            catch
            {
                if (DialogResult.Retry == MetroMessageBox.Show(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150))
                    goto tryagain;
            }
        }

        public static void ChangePass(string oldPass, string newPass, Main form)
        {
            oldPass = Hash(oldPass);
            newPass = Hash(newPass);

            tryagain:
            try
            {
                CheckIp(form);

                using (WebClient web = new WebClient())
                {
                    string result = web.DownloadString(storageServer + "/Script%20Browser/changePassword.php?user=" + Main.sf.password + "&pass=" + oldPass + "&newpass=" + newPass);
                    if (result.Contains("false"))
                        MetroMessageBox.Show(form, "Please check your old password or try again later.", "Could not change password", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
                    else
                    {
                        MetroMessageBox.Show(form, "Your password has been successfully changed!", "Password changed", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 125);
                        form.settings1.materialSingleLineTextField1.Text = "";
                        form.settings1.materialSingleLineTextField2.Text = "";
                        form.settings1.materialSingleLineTextField3.Text = "";
                        Main.sf.password = newPass;
                    }
                }
            }
            catch
            {
                if (DialogResult.Retry == SMB(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 2, DialogResult.Retry))
                    goto tryagain;
            }
        }

        public static void ChangeUsername(string _username, string pass, Main form)
        {
            pass = Hash(pass);

            tryagain:
            try
            {
                CheckIp(form);

                using (WebClient web = new WebClient())
                {
                    string result = web.DownloadString(storageServer + "/Script%20Browser/changeUsername.php?user=" + Main.sf.username + "&newuser=" + _username + "&pass=" + pass);
                    if (result.Contains("false"))
                        MetroMessageBox.Show(form, "Please check your password or try again later.", "Could not change username", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
                    else if (result.Contains("username"))
                        MetroMessageBox.Show(form, "Your new username has already been taken.", "Could not change username", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
                    else
                    {
                        MetroMessageBox.Show(form, "Your username has been successfully changed!", "Username changed", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 125);
                        form.settings1.materialSingleLineTextField4.Text = "";
                        form.settings1.materialSingleLineTextField5.Text = "";
                        Main.sf.username = _username;
                        form.settings1.label1.Text = "Logged in as " + Main.sf.username;
                    }
                }
            }
            catch
            {
                if (DialogResult.Retry == SMB(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 2, DialogResult.Retry))
                    goto tryagain;
            }
        }

        public static void ChangeEMail(string email, string pass, Main form)
        {
            pass = Hash(pass);

            tryagain:
            try
            {
                CheckIp(form);

                using (WebClient web = new WebClient())
                {
                    string result = web.DownloadString(storageServer + "/Script%20Browser/changeEmail.php?user=" + Main.sf.username + "&pass=" + pass + "&email=" + email);
                    if (result.Contains("false"))
                        MetroMessageBox.Show(form, "Please check your password or try again later.", "Could not change E-Mail address", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 125);
                    else if (result.Contains("blacklist"))
                        MetroMessageBox.Show(form, "The email address \"" + email + "\" is blacklisted!\nContact us under \"sl.chatbot.script.browser@gmail.com\" for more information.", "Could not change E-Mail address", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("email"))
                        MetroMessageBox.Show(form, "The email address \"" + email + "\" is allready registered!\nPlease select another one.", "Could not change E-Mail address", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("time"))
                        MetroMessageBox.Show(form, "You can only request a verification E-Mail once every 5 minuets.\nPlease wait...", "Could not change E-Mail address", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else
                    {
                        MetroMessageBox.Show(form, "Your new E-Mail address will be activated, after you verified it.\nPlease check your inbox for more information.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 125);
                        form.settings1.materialSingleLineTextField6.Text = "";
                        form.settings1.materialSingleLineTextField7.Text = "";
                        form.settings1.noFocusBorderBtn6.Visible = true;
                    }
                }
            }
            catch
            {
                if (DialogResult.Retry == SMB(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 2, DialogResult.Retry))
                    goto tryagain;
            }
        }

        public static void NotifySettings(Main form, int newcom, int newreply, int newbug)
        {
            tryagain:
            if (CheckIp(form))
            {
                try
                {
                    using (WebClient web = new WebClient())
                    {
                        string result = web.DownloadString(storageServer + "/Script%20Browser/notifySettings.php?user=" + Main.sf.username + "&pass=" + Main.sf.password + "&newcom=" + newcom + "&newreply=" + newreply + "&newbug=" + newbug);

                        if (result.Contains("false"))
                            MetroMessageBox.Show(form, "Could't update your notification settings. Please try again later.", "Could update settings", MessageBoxButtons.OK, MessageBoxIcon.Error, 100);
                        else if (result.Contains("true"))
                            MetroMessageBox.Show(form, "Your notification settings have been updated!", "Settings updated", MessageBoxButtons.OK, MessageBoxIcon.Information, 100);
                    }
                }
                catch
                {
                    if (DialogResult.Retry == SMB(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 2, DialogResult.Retry))
                        goto tryagain;
                }
            }
        }

        #endregion

        #region Browse Scripts

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

        #endregion

        public static string CheckForUpdate(string id, string ver)
        {
            CheckIp(null);
            using (WebClient web = new WebClient())
                return web.DownloadString(storageServer + "/Script%20Browser/checkForUpdate.php?id=" + id + "&ver=" + ver);
        }

        #region Transfer Scripts

        public static string UploadScript(UploadScript form, string info, string path)
        {
            if (CheckIp(form))
            {
                using (MultipartFormDataContent data = new MultipartFormDataContent
                {
                    { new StringContent(info), "info" },
                    { new StreamContent(File.Open(path, FileMode.Open)), "file", "script.zip" }
                })
                using (HttpClient web = new HttpClient())
                    return web.PostAsync(storageServer + "/Script%20Browser/uploadScript.php?user=" + Main.sf.username + "&pass=" + Main.sf.password, data).Result.Content.ReadAsStringAsync().Result;
            }
            return "false";
        }

        public static bool DownloadScript(Main form, int id)
        {
            try
            {
                if (CheckIp(form))
                {
                    using (WebClient web = new WebClient())
                        web.DownloadFile(storageServer + "/Script%20Browser/Uploads/" + id, Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Install.zip");
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static bool IncreaseDownloads(Main form, int id)
        {
            try
            {
                if (CheckIp(form))
                {
                    using (WebClient web = new WebClient())
                        web.DownloadString(storageServer + "/Script%20Browser/increaseDownloads.php?id=" + id);
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static string GetUploadUpdateInfo(Main form, string id)
        {
            try
            {
                if (CheckIp(form))
                {
                    using (WebClient web = new WebClient())
                        return web.DownloadString(storageServer + "/Script%20Browser/getUUInfo.php?id=" + id);
                }
            }
            catch { }
            return "";
        }

        public static string UploadUpdate(UploadScript form, string info, string path)
        {
            if (CheckIp(form))
            {
                using (MultipartFormDataContent data = new MultipartFormDataContent
                {
                    { new StringContent(info), "info" },
                    { new StreamContent(File.Open(path, FileMode.Open)), "file", "script.zip" }
                })
                using (HttpClient web = new HttpClient())
                    return web.PostAsync(storageServer + "/Script%20Browser/uploadUpdate.php?user=" + Main.sf.username + "&pass=" + Main.sf.password, data).Result.Content.ReadAsStringAsync().Result;
            }
            return "false";
        }

        public static string UploadUpdate(UploadScript form, string info)
        {
            if (CheckIp(form))
            {
                using (MultipartFormDataContent data = new MultipartFormDataContent
                {
                    { new StringContent(info), "info" }
                })
                using (HttpClient web = new HttpClient())
                    return web.PostAsync(storageServer + "/Script%20Browser/uploadUpdate.php?user=" + Main.sf.username + "&pass=" + Main.sf.password, data).Result.Content.ReadAsStringAsync().Result;
            }
            return "false";
        }

        #endregion

        public static string RateScript(Main form, string id, string rating)
        {
            if (CheckIp(form))
            {
                using (WebClient web = new WebClient())
                    return web.DownloadString(storageServer + "/Script%20Browser/rateScript.php?id=" + id + "&rating=" + rating);
            }
            return "false";
        }

        #region Comments

        public static JObject GetComments(Main form, int id)
        {
            if (CheckIp(form))
            {
                using (WebClient web = new WebClient())
                    return JObject.Parse(web.DownloadString(storageServer + "/Script%20Browser/getComments.php?id=" + id + "&user=" + Main.sf.username + "&pass=" + Main.sf.password));
            }
            return null;
        }

        public static bool SendComment(Main form, int id, string comment, string reply)
        {
            if (CheckIp(form))
            {
                using (MultipartFormDataContent data = new MultipartFormDataContent
                {
                    { new StringContent(comment), "comment" },
                    { new StringContent(id + ""), "id" },
                    { new StringContent(reply), "reply" }
                })
                using (HttpClient web = new HttpClient())
                {
                    string result = web.PostAsync(storageServer + "/Script%20Browser/addComment.php?user=" + Main.sf.username + "&pass=" + Main.sf.password, data).Result.Content.ReadAsStringAsync().Result;
                    if (result.Contains("time"))
                        MetroMessageBox.Show(form, "Please wait a while befor posting a new comment.", "Don't Spam!", MessageBoxButtons.OK, MessageBoxIcon.Warning, 100);
                    else if (result.Contains("false"))
                        MetroMessageBox.Show(form, "Could't send your comment. Please try again later.", "Could not send comment", MessageBoxButtons.OK, MessageBoxIcon.Error, 100);
                    else
                        return true;
                }
            }
            return false;
        }

        public static bool DeleteComment(Main form, string id)
        {
            if (CheckIp(form))
            {
                using (WebClient web = new WebClient())
                {
                    string result = web.DownloadString(storageServer + "/Script%20Browser/deleteComment.php?id=" + id + "&user=" + Main.sf.username + "&pass=" + Main.sf.password);
                    if (result.Contains("true"))
                        return true;
                }
            }
            return false;
        }

        #endregion

        public static DialogResult SMB(Form form, string msg = "", string title = "", MessageBoxButtons btns = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information, MessageBoxDefaultButton defBtn = MessageBoxDefaultButton.Button1, int heigth = 1, DialogResult def = DialogResult.Yes)
        {
            if (form.Visible)
                return MetroMessageBox.Show(form, msg, title, btns, icon, defBtn, 80 + (heigth * 22));
            else
                return def;
        }
    }
}