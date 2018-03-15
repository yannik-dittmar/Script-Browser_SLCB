using MetroFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser
{
    static class Networking
    {
        public static string storageServer = "";
        public static string username = "krypto";
        public static string password = "a94a8fe5ccb19ba61c4c0873d391e987982fbbd3";
        public static List<string> scripts = new List<string>();

        //Encrypt passwords SHA1
        static string Hash(string input)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }

        //IP des Datenbanks-Server abrufen
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

        //Überprüfen ob die IP vom Datenbank-Server existiert
        public static bool CheckIp(Form form)
        {
            tryagain:
            if (storageServer == "")
            {
                if (!UpdateIp())
                {
                    if (DialogResult.Retry == MetroMessageBox.Show(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.\n\nCould not connect to mediation server!", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, 175))
                        goto tryagain;
                    else
                        return false;
                }
            }
            return true;
        }

        //Anmeldung
        public static void Login(string _username, string _password, Main form)
        {
            _password = Hash(_password); //Passwort verschlüsseln 
            tryagain:
            try
            {
                CheckIp(form); //IP überprüfen

                using (WebClient web = new WebClient())
                {
                    //Serverabfrage starten
                    string result = web.DownloadString(storageServer + "/Script%20Browser/login.php?user=" + _username + "&pass=" + _password + "&getinfo=true");

                    //Anmeldedaten sind falsch
                    if (result.Contains("false"))
                        MetroMessageBox.Show(form, "The username or password was incorrect.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 100);
                    //Anmeldedaten sind korrekt
                    else
                    {
                        //Erstellen des JObjects aus der Serverantwort
                        JObject info = JObject.Parse(result);
                        username = info["Username"].ToString();
                        password = _password;
                        form.settings1.label1.Text = "Logged in as " + username;

                        //Login-Bereich verstecken und Logout-Bereich anzeigen
                        form.settings1.animator1.Hide(form.settings1.tableLayoutPanel1);
                        form.settings1.animator1.Show(form.settings1.tableLayoutPanel2);

                        //Scripts, die dem Nutzer gehören erfassen
                        foreach (JToken i in info["Scripts"] as JArray)
                            scripts.Add(i.ToString());
                    }
                }
            }
            catch
            {
                //Unerwarteter Fehler (z.B. kein Internet)
                if (DialogResult.Retry == MetroMessageBox.Show(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150))
                    goto tryagain;
            }
        }

        //Regestrieren
        public static void SignUp(string _username, string _password, string email, Main form)
        {
            _password = Hash(_password); //Passwort verschlüsseln
            tryagain:
            try
            {
                CheckIp(form); //IP überprüfen

                using (WebClient web = new WebClient())
                {
                    //Serverabfrage starten
                    string result = web.DownloadString(storageServer + "/Script%20Browser/signUp.php?username=" + _username + "&pass=" + _password + "&email=" + email);

                    if (result.Contains("username")) //Benutzername bereits vorhanden
                        MetroMessageBox.Show(form, "The username \"" + _username + "\" is allready registered!\nPlease select another one.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("email")) //EMail bereits vorhanden
                        MetroMessageBox.Show(form, "The email address \"" + email + "\" is allready registered!\nPlease select another one.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("blacklist")) //EMail darf nicht benutzt werden
                        MetroMessageBox.Show(form, "The email address \"" + email + "\" is blacklisted!\nContact us under \"sl.chatbot.script.browser@gmail.com\" for more information.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else if (result.Contains("false")) //Unerwarteter Fehler
                        MetroMessageBox.Show(form, "There was an unexected sign up error.\nPlease try again later.", "Sign up error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150);
                    else //Regestrierung erfolgreich
                    {
                        MetroMessageBox.Show(form, "You signed up successfully!\nA verification email has been send to your email account. Please check your inbox.", "Sign up success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 150);

                        //Anmeldedaten setzen
                        username = _username;
                        password = _password;
                        form.settings1.label1.Text = "Logged in as " + username;

                        //Login-Bereich verstecken und Logout-Bereich anzeigen
                        form.settings1.animator1.Hide(form.settings1.tableLayoutPanel1);
                        form.settings1.animator1.Show(form.settings1.tableLayoutPanel2);
                    }
                }
            }
            catch
            {
                //Unerwarteter Fehler (z.B. kein Internet)
                if (DialogResult.Retry == MetroMessageBox.Show(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 150))
                    goto tryagain;
            }
        }

        //TopScripts aufrufen
        public static string GetTopScripts(string type, string highest, int page, Main form)
        {
            CheckIp(form);
            using (WebClient web = new WebClient())
                return web.DownloadString(storageServer + "/Script%20Browser/getTopScripts.php?type=" + type + "&highest=" + highest + "&page=" + page);
        }

        //Nach Scripts suchen
        public static string SearchScripts(string[] tags, Main form)
        {
            CheckIp(form);
            using (WebClient web = new WebClient())
                return web.DownloadString(storageServer + "/Script%20Browser/searchByTags.php?tags=" + JArray.FromObject(tags).ToString());
        }

        //Script-Informationen nach ID abrufen
        public static string GetScriptById(Main form, string id)
        {
            CheckIp(form);
            using (WebClient web = new WebClient())
                return web.DownloadString(storageServer + "/Script%20Browser/getScript.php?id=" + id);
        }

        //Überprüfen ob ein Update für ein Script vorliegt
        public static bool CheckForUpdate(string id, string ver)
        {
            CheckIp(null);
            using (WebClient web = new WebClient())
                return web.DownloadString(storageServer + "/Script%20Browser/checkForUpdate.php?id=" + id + "&ver=" + ver).Contains("UPDATE");
        }

        //Script hochladen
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
                    return web.PostAsync(storageServer + "/Script%20Browser/uploadScript.php?user=" + username + "&pass=" + password, data).Result.Content.ReadAsStringAsync().Result;
            }
            return "false";
        }

        //Script herunterladen
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

        //Informationen zu einem Script bekommen um ein Update durchzuführen
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

        //Update hochladen mit Datei-Änderungen
        public static string UploadUpdate(UploadScript form, string info, string path)
        {
            //IP des Datenbank-Servers überprüfen
            if (CheckIp(form))
            {
                //Ein MultipartFormDataContent erstellen
                using (MultipartFormDataContent data = new MultipartFormDataContent
                {
                    { new StringContent(info), "info" },
                    { new StreamContent(File.Open(path, FileMode.Open)), "file", "script.zip" }
                })
                
                //Abfrage starten und Ergebnis zurückgeben
                using (HttpClient web = new HttpClient())
                    return web.PostAsync(storageServer + "/Script%20Browser/uploadUpdate.php?user=" + username + "&pass=" + password, data).Result.Content.ReadAsStringAsync().Result;
            }
            return "false";
        }

        //Update hochladen ohne Datei-Änderungen
        public static string UploadUpdate(UploadScript form, string info)
        {
            //IP des Datenbank-Servers überprüfen
            if (CheckIp(form))
            {
                //Ein MultipartFormDataContent erstellen
                using (MultipartFormDataContent data = new MultipartFormDataContent
                {
                    { new StringContent(info), "info" }
                })

                //Abfrage starten und Ergebnis zurückgeben
                using (HttpClient web = new HttpClient())
                    return web.PostAsync(storageServer + "/Script%20Browser/uploadUpdate.php?user=" + username + "&pass=" + password, data).Result.Content.ReadAsStringAsync().Result;
            }
            return "false";
        }
    }
}