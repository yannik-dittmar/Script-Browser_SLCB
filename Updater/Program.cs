using Microsoft.Win32;
using SaveManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SaveFile sf = new SaveFile(Path.GetDirectoryName(Application.ExecutablePath) + @"\settings.save");
            String versionWeb="";

            WebClient web = new WebClient();
            Stream stream = web.OpenRead("http://digital-programming.com/ScriptBrowser/version.txt");
            using (StreamReader reader = new StreamReader(stream))
            {
                versionWeb = reader.ReadToEnd();
            }

            if (!versionWeb.Equals(sf.version))
            {
                String urlAddress = "http://www.digital-programming.de/ScriptBrowser/setup.zip";
                String location = Application.ExecutablePath;
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);

                    //url address
                    Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                    //download
                    try
                    {
                        webClient.DownloadFileAsync(URL, location + "\\setup.zip");
                    }
                    catch { }
                }

                //download completed
                void Completed(object sender2, AsyncCompletedEventArgs e)
                {
                    if (!e.Cancelled)
                    {
                        //delete old files and folders
                        DirectoryInfo di = new DirectoryInfo("" + AppDomain.CurrentDomain.BaseDirectory);

                        foreach (FileInfo file in di.GetFiles())
                        {
                            if (file.Name != "Updater.exe")
                            {
                                try
                                {
                                    file.Delete();
                                }
                                catch { }
                            }
                        }
                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            if (dir.Name != "setup.zip")
                            {
                                try
                                {
                                    dir.Delete(true);
                                }
                                catch { }
                            }
                        }

                        //extract downloaded zip file
                        ZipFile.ExtractToDirectory(location + "\\setup.zip", location);
                        //delete zip file
                        File.Delete(location + "\\setup.zip");
                    }
                }

            
        }
    }
}
