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

namespace Download_Manager.Pages
{
    public partial class Installation : UserControl
    {
        String pathInstallation, pathStreamlabs;

        public Installation()
        {
            InitializeComponent();
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        //FolderBrowser for selection of path for download
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                pathInstallation = folderBrowserDialog1.SelectedPath;
                textBox1.Text = pathInstallation + "\\Script-Browser";
            }
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            String urlAddress = "http://www.digital-programming.de/ScriptBrowser/setup.zip";
            String location = pathInstallation + "\\Script-Browser";
            WebClient webClient;               //webclient for downloading
            Stopwatch sw = new Stopwatch();

            if (Directory.Exists(location))
            {
                DialogResult result = MetroFramework.MetroMessageBox.Show(this, "You already installed the Script-Browser.", " ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, 100);
            }

            if (textBox1.Text != "" && !Directory.Exists(location))
            {
                

                Directory.CreateDirectory(location);


                using (webClient = new WebClient())
                {
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                    //url address
                    Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                    //start stopwatch used to calculate download speed
                    sw.Start();

                    //start download
                    try
                    {
                        webClient.DownloadFileAsync(URL, location + "\\setup.zip");
                    }
                    catch { }
                }
            }

            void ProgressChanged(object sender2, DownloadProgressChangedEventArgs e2)
            {
                //update label for downloadspeed
                labelSpeed.Text = string.Format("{0} kb/s", (e2.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString());

                //update progressbar
                metroProgressBar1.Value = e2.ProgressPercentage;
            }

            void Completed(object sender2, AsyncCompletedEventArgs e2)
            {
                sw.Reset();

                if(e2.Cancelled)
                {
                    //cancelled
                }
                //completed
                else
                {
                    Console.WriteLine("test");
                    ZipFile.ExtractToDirectory(location + "\\setup.zip", location);
                    File.Delete(location + "\\setup.zip");
                }
            }
        }

        //FolderBrowser for selection of path of Streamlabs Chatbot
        private void button2_Click(object sender, EventArgs e)
        {
            //Disable new folder button
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
