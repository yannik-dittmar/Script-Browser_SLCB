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

        //FolderBrowser for selection of path for download
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                pathInstallation = folderBrowserDialog1.SelectedPath + "\\Script-Browser";
                textBox1.Text = pathInstallation;
            }
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {

            String urlAddress = "http://www.digital-programming.de/ScriptBrowser/setup.zip";
            String location = pathInstallation;
            WebClient webClient;               //webclient for downloading
            Stopwatch sw = new Stopwatch();

            Boolean download = true;
            //test if chatbot folder is correct
            if (!File.Exists(pathStreamlabs + "\\Streamlabs Chatbot.exe"))
            {
                DialogResult result = MessageBox.Show("The selected path for the Streamlabs Chatbot doesn't contain it.", "Error", MessageBoxButtons.OK);
                //DialogResult result2 = MetroFramework.MetroMessageBox.Show(this, "The selected path for the Streamlabs Chatbot doesn't contain it.", " ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, 100);
                download = false;

            }

            //check if Script-Browser already exists -> error
            if (Directory.Exists(location))
            {
                DialogResult result = MessageBox.Show("You already installed the Script-Browser.", "Error", MessageBoxButtons.OK);
                download = false;
            }

            if (download)
            {
                //disable brwose buttons
                button1.Enabled = false;
                button2.Enabled = false;

                //create directory for Script-Browser
                Directory.CreateDirectory(location);

                //Downloading
                using (webClient = new WebClient())
                {
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                    //url address
                    Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                    //start stopwatch used to calculate download speed
                    sw.Start();

                    //download
                    try
                    {
                        webClient.DownloadFileAsync(URL, location + "\\setup.zip");
                    }
                    catch { }
                }
            }

            //updating ui while downloading
            void ProgressChanged(object sender2, DownloadProgressChangedEventArgs e2)
            {
                //update label for downloadspeed
                labelSpeed.Text = string.Format("{0} kb/s", (Math.Round(e2.BytesReceived / 1024d / sw.Elapsed.TotalSeconds)).ToString());

                //update progressbar
                progressBar1.Value = e2.ProgressPercentage;
            }

            //download completed
            void Completed(object sender2, AsyncCompletedEventArgs e2)
            {
                //reset stopwatch
                sw.Reset();


                if(e2.Cancelled)
                {
                    //cancelled
                }
                //completed
                else
                {
                    //enable button for finishing process 
                    button3.Enabled = true;
                    Console.WriteLine("test");

                    //extract downloaded zip file
                    ZipFile.ExtractToDirectory(location + "\\setup.zip", location);
                    //delete zip file
                    File.Delete(location + "\\setup.zip");

                    //start SB?
                    if (checkBox1.Checked)
                    {
                        Process.Start(pathInstallation + "\\Script-Browser.exe", "\"" + pathStreamlabs + "\" true");
                    }
                    else
                    {
                        Process.Start(pathInstallation + "\\Script-Browser.exe", "\"" + pathStreamlabs + "\" false");
                    }

                    //check if user wants to create desktop link
                    if (checkBox2.Checked == true)
                    {
                        string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                        //create desktop shortcut
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

                    if (checkBox3.Checked)
                    {
                        //create start menu folder
                        string startMenu = Environment.GetFolderPath(Environment.SpecialFolder.Programs) + "\\Script-Browser";
                        
                        if (!Directory.Exists(startMenu))
                        {
                            Directory.CreateDirectory(startMenu);
                        }


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
                }
            }
        }

        //finish installation
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //change path for installation if user changes text of textbox
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            pathStreamlabs = textBox2.Text;
        }

        //change path of Streamlabs folder if user changes text of textbox
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            pathInstallation = textBox1.Text;
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
