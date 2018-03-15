using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Microsoft.Win32;

namespace Download_Manager.Pages
{
    public partial class Installation : UserControl
    {
        String pathInstallation, pathStreamlabs;

        public Installation()
        {
            InitializeComponent();
        }

        //FolderBrowser für das Wählen eines Pfades
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            //bei Bestätigung Speichern des Pfades in Variable
            if (result == DialogResult.OK)
            {
                pathInstallation = folderBrowserDialog1.SelectedPath + "\\Script-Browser";
                textBox1.Text = pathInstallation;
            }
        }

        //Knopf "Installieren"
        private void buttonInstall_Click(object sender, EventArgs e)
        {
            //URL Adresse, Installationspfad, WebClient um Daten zu senden und zu empfangen, Stoppuhr
            String urlAddress = "http://www.digital-programming.de/ScriptBrowser/setup.zip";
            String location = pathInstallation;
            WebClient webClient;               //webclient for downloading
            Stopwatch sw = new Stopwatch();

            Boolean download = true;
            //Testen, ob der Streamlabs Chatbot installiert ist
            if (!File.Exists(pathStreamlabs + "\\Streamlabs Chatbot.exe"))
            {
                DialogResult result = MessageBox.Show("The selected path for the Streamlabs Chatbot doesn't contain it.", "Error", MessageBoxButtons.OK);
                download = false;

            }

            //Prüfen, ob der Script-Browser bereits existiert
            if (Directory.Exists(location) && File.Exists(location+"\\Script-Browser.exe"))
            {
                DialogResult result = MessageBox.Show("You already installed the Script-Browser.", "Error", MessageBoxButtons.OK);
                download = false;
            }

            //Downloaden, wenn keine Fehlermeldung
            if (download)
            {
                //Brwose Buttons deaktivieren
                button1.Enabled = false;
                button2.Enabled = false;

                //Ordner für Script-Browser erstellen
                Directory.CreateDirectory(location);

                //Downloading
                using (webClient = new WebClient())
                {
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                    //url Adresse
                    Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                    //Starten der Stoppuhr um Downloadgeschwindigkeit zu berechnen
                    sw.Start();

                    //download
                    try
                    {
                        webClient.DownloadFileAsync(URL, location + "\\setup.zip");
                    }
                    catch { }
                }
            }

            //Ladebalken aktualisieren
            void ProgressChanged(object sender2, DownloadProgressChangedEventArgs e2)
            {
                //label für Geschwindigkeit aktualisieren
                labelSpeed.Text = string.Format("{0} kb/s", (Math.Round(e2.BytesReceived / 1024d / sw.Elapsed.TotalSeconds)).ToString());

                //Ladebalken aktualisieren
                progressBar1.Value = e2.ProgressPercentage;
            }

            //download fertig
            void Completed(object sender2, AsyncCompletedEventArgs e2)
            {
                //Stoppuhr zurücksetzen
                sw.Reset();


                if(e2.Cancelled)
                {
                    //cancelled
                }
                //fertiger Download
                else
                {
                    //Button "finish" aktivieren
                    button3.Enabled = true;

                    //Zip entpacken
                    ZipFile.ExtractToDirectory(location + "\\setup.zip", location);
                    //Zip Datei löschen
                    File.Delete(location + "\\setup.zip");

                    //Prüfen, ob SB gestartet werden soll
                    //ja -> starten des SB mit Parametern Pfad und Start im Vordergrund
                    if (checkBox1.Checked)
                    {
                        Process.Start(pathInstallation + "\\Script-Browser.exe", "\"" + pathStreamlabs + "\" true");
                    }
                    //nein -> starten des SB mit Parametern Pfad und Start im Hintergrund
                    else
                    {
                        Process.Start(pathInstallation + "\\Script-Browser.exe", "\"" + pathStreamlabs + "\" false");
                    }

                    //Desktop Link?
                    if (checkBox2.Checked == true)
                    {
                        //Pfad des Desktops
                        string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                        //Erstellen eines Links
                        using (StreamWriter writer = new StreamWriter(desktop + "\\Script-Browser.url"))
                        {
                            string app = pathInstallation + "\\Script-Browser.exe";
                            writer.WriteLine("[InternetShortcut]");
                            writer.WriteLine("URL=file:///" + app);
                            writer.WriteLine("IconIndex=0");
                            string icon = app.Replace('\\', '/');
                            writer.WriteLine("IconFile=" + icon);
                            writer.Flush();
                        }
                    }

                    //Startmenüeintrag?
                    if (checkBox3.Checked)
                    {
                        //Startmenüordner Pfad
                        string startMenu = Environment.GetFolderPath(Environment.SpecialFolder.Programs) + "\\Script-Browser";
                        
                        //bei Nichtexistenz erstellen eines Ordners
                        if (!Directory.Exists(startMenu))
                        {
                            Directory.CreateDirectory(startMenu);
                        }

                        //Eintrag erstellen
                        using (StreamWriter writer2 = new StreamWriter(startMenu + ".url"))
                        {
                            writer2.WriteLine("[InternetShortcut]");
                            writer2.WriteLine("URL=file:///" + pathInstallation + "\\Script-Browser.exe");
                            writer2.WriteLine("IconIndex=0");
                            string icon = (pathInstallation + "\\Script-Browser.exe").Replace('\\', '/');
                            writer2.WriteLine("IconFile=" + icon);
                            writer2.Flush();
                        }
                    }

                    //Updater zu regestry hizufügen
                    RegistryKey key = Registry.CurrentUser.OpenSubKey("Computer\\HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    key.SetValue("Updater SB", pathInstallation + "\\Updater.exe");
                }
            }
        }

        //Button finish
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Pfad bei Nutzereingabe überschreiben
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            pathStreamlabs = textBox2.Text;
        }

        //Pfad bei Nutzereingabe überschreiben
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            pathInstallation = textBox1.Text;
        }

        //FolderBrowser für das Wählen eines Pfades für Streamlabs Chatbot
        private void button2_Click(object sender, EventArgs e)
        {
            //Verhindern, dass ein neuer Ornder erstellt wird
            this.folderBrowserDialog2.ShowNewFolderButton = false;
            DialogResult result = folderBrowserDialog2.ShowDialog();

            if (result == DialogResult.OK)
            {
                pathStreamlabs = folderBrowserDialog2.SelectedPath;
                textBox2.Text = pathStreamlabs;
            }
        }
    }
}
