using FluentFTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateLogger
{
    public partial class Form1 : Form
    {
        private string settingsFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.json";
        private string blacklistFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\blacklist.json";
        private string scanLocation = @"D:\MegaSync\Visual-Studio\Script-Browser_SLCB\Script-Browser\bin\Debug";
        private JArray changelog;
        private JArray blacklist;
        private List<string> bl = new List<string>();

        public Form1()
        {
            InitializeComponent();
            fileSystemWatcher1.Path = scanLocation;

            try
            {
                if (File.Exists(settingsFile))
                    changelog = JArray.Parse(File.ReadAllText(settingsFile));
                else
                    changelog = new JArray();
            }
            catch { changelog = new JArray(); }

            try
            {
                if (File.Exists(blacklistFile))
                    blacklist = JArray.Parse(File.ReadAllText(blacklistFile));
                else
                    blacklist = new JArray();
            }
            catch { blacklist = new JArray(); }
   
            openFileDialog1.InitialDirectory = scanLocation;
        }

        private void ReadBlacklist()
        {
            bl.Clear();
            foreach (JToken item in blacklist)
                bl.Add(item.ToString());
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            PrintLog();
        }

        private void PrintLog()
        {
            ReadBlacklist();
            string version = "1.0.0";
            foreach (JToken change in changelog)
                version = change["version"].ToString();
            textBox2.Text = version;

            JObject unstatedChanges = ScanChanges(GetLastScan());
            textBox1.Text = changelog.ToString().Replace("\\\\", "\\") 
                + Environment.NewLine 
                + Environment.NewLine 
                + "===UNSTATED CHANGES===" 
                + Environment.NewLine 
                + Environment.NewLine 
                + unstatedChanges.ToString().Replace("\\\\", "\\");

            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();

            button1.Enabled = !(
                (int)unstatedChanges["files"]["stats"]["changed"] == 0 
                && (int)unstatedChanges["files"]["stats"]["removed"] == 0
                && (int)unstatedChanges["folders"]["stats"]["added"] == 0
                && (int)unstatedChanges["folders"]["stats"]["removed"] == 0);
        }

        private string FileTime(string file)
        {
            return " | " + new FileInfo(file).LastWriteTime;
        }

        private JObject ScanChanges(JObject lastScan)
        {
            JObject result = new JObject
            {
                ["date"] = DateTime.Now.ToString("dd.MM.yy"),
                ["version"] = textBox2.Text,
                ["files"] = new JObject
                {
                    ["stats"] = new JObject
                    {
                        ["changed"] = 0,
                        ["removed"] = 0
                    },
                    ["changed"] = new JObject(),
                    ["removed"] = new JArray()
                },
                ["folders"] = new JObject
                {
                    ["stats"] = new JObject
                    {
                        ["added"] = 0,
                        ["removed"] = 0
                    },
                    ["added"] = new JArray(),
                    ["removed"] = new JArray()
                }
            };

            Tuple<Dictionary<string, string>, string[]> subData = GetSubData(scanLocation);
            string[] currentFiles = subData.Item1.Keys.ToArray();
            string[] currentFolders = subData.Item2;

            //Removed Files
            foreach (KeyValuePair<string, JToken> lastfile in (JObject)lastScan["files"])
            {
                if (!currentFiles.Contains(lastfile.Key.ToString()))
                    ((JArray)result["files"]["removed"]).Add(lastfile.Key);
            }

            //Removed Folders
            foreach (JToken lastfolder in lastScan["folders"])
            {
                if (!currentFolders.Contains(lastfolder.ToString()))
                    ((JArray)result["folders"]["removed"]).Add(lastfolder);
            }

            //Added Files
            foreach (string currentFile in currentFiles)
            {
                if (!((JObject)lastScan["files"]).ContainsKey(currentFile))
                {
                    if (!bl.Contains(currentFile))
                    {
                        bool found = false;
                        foreach (string item in bl)
                        {
                            if (currentFile.StartsWith(item))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                            result["files"]["changed"][currentFile] = subData.Item1[currentFile];
                    }
                }
                else if (lastScan["files"][currentFile].ToString() != subData.Item1[currentFile] && !bl.Contains(currentFile))
                {
                    bool found = false;
                    foreach (string item in bl)
                    {
                        if (currentFile.StartsWith(item))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        result["files"]["changed"][currentFile] = subData.Item1[currentFile];
                }
            }

            //Added Folders
            foreach (string currentFolder in currentFolders)
            {
                if (((JArray)lastScan["folders"]).FirstOrDefault(f => f.ToString() == currentFolder) == null && !bl.Contains(currentFolder))
                {
                    bool found = false;
                    foreach (string item in bl)
                    {
                        if (currentFolder.StartsWith(item))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        ((JArray)result["folders"]["added"]).Add(currentFolder);
                }
            }

            //Counters
            result["files"]["stats"]["changed"] = result["files"]["changed"].Count();
            result["files"]["stats"]["removed"] = result["files"]["removed"].Count();
            result["folders"]["stats"]["added"] = result["folders"]["added"].Count();
            result["folders"]["stats"]["removed"] = result["folders"]["removed"].Count();

            return result;
        }

        private Tuple<Dictionary<string, string>, string[]> GetSubData(string p)
        {
            Dictionary<string, string> files = new Dictionary<string, string>();
            HashSet<string> subfolders = new HashSet<string>(Directory.GetDirectories(p));
            HashSet<string> storedSubfolders = new HashSet<string>();

            string[] topFiles = Directory.GetFiles(p);
            foreach (string file in topFiles)
                files[file.Replace(p, "")] = HashSum(file);

            while (subfolders.Count != 0)
            {
                string subfolder = subfolders.First();
                string[] subfiles = Directory.GetFiles(subfolder);

                foreach (string file in subfiles)
                    files[file.Replace(p, "")] = HashSum(file);

                subfolders.Remove(subfolder);
                storedSubfolders.Add(subfolder.Replace(p, ""));

                string[] subsubfolders = Directory.GetDirectories(subfolder);
                foreach (string subsubfolder in subsubfolders)
                    subfolders.Add(subsubfolder);
            }

            return Tuple.Create(files, storedSubfolders.ToArray());
        }

        private JObject GetLastScan()
        {
            JObject lastScan = new JObject
            {
                ["files"] = new JObject(),
                ["folders"] = new JArray()
            };

            foreach (JToken change in changelog)
            {
                foreach (KeyValuePair<string, JToken> fileChange in (JObject)change["files"]["changed"])
                    lastScan["files"][fileChange.Key] = fileChange.Value;

                foreach (JToken fileRemoved in change["files"]["removed"])
                    ((JObject)lastScan["files"]).Remove(fileRemoved.ToString());

                foreach (JToken folderAdded in change["folders"]["added"])
                    ((JArray)lastScan["folders"]).Add(folderAdded);

                foreach (JToken folderRemoved in change["folders"]["removed"])
                    ((JArray)lastScan["folders"]).Remove(folderRemoved);
            }

            return lastScan;
        }

        private string HashSum(string path)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(path))
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
        }

        //Log Changes
        private void button1_Click(object sender, EventArgs e)
        {
            changelog.Add(ScanChanges(GetLastScan()));
            PrintLog();
            File.WriteAllText(settingsFile, changelog.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start(scanLocation);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Changelog
            JArray exportChangelog = new JArray(changelog);
            foreach (JToken change in exportChangelog)
            {
                JObject fileChanges = (JObject)change["files"]["changed"].DeepClone();
                change["files"]["changed"] = new JArray();
                foreach (KeyValuePair<string, JToken> fileChange in fileChanges)
                    ((JArray)change["files"]["changed"]).Add(fileChange.Key);
            }

            File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + @"\changelog.txt", exportChangelog.ToString());

            //Version
            File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + @"\version.txt", textBox2.Text);

            //Create Zip
            if (Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp"))
                Directory.Delete(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp", true);
            Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp");

            Tuple<Dictionary<string, string>, string[]> subData = GetSubData(scanLocation);
            string[] currentFiles = subData.Item1.Keys.ToArray();
            foreach (string file in currentFiles)
            {
                bool found = false;
                foreach (string item in bl)
                {
                    if (file.StartsWith(item))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    if (!Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp" + file))
                        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp" + file));
                    File.Copy(scanLocation + file, Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp" + file);
                }
            }
            ZipFile.CreateFromDirectory(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\", Path.GetDirectoryName(Application.ExecutablePath) + @"\setup.zip");
            
            //FTP Upload
            try
            {
                using (FtpClient client = new FtpClient("digital-programming.de"))
                {
                    while (true)
                    {
                        try
                        {
                            string input = Microsoft.VisualBasic.Interaction.InputBox("Password for FTP-Connection: digital-programming.de", "FTP Password", "", 0, 0);
                            client.Credentials = new System.Net.NetworkCredential("ni485822_1", input);
                            client.Connect();
                            break;
                        }
                        catch { }
                    }

                    client.UploadFiles(new[] { Path.GetDirectoryName(Application.ExecutablePath) + @"\changelog.txt", Path.GetDirectoryName(Application.ExecutablePath) + @"\version.txt", Path.GetDirectoryName(Application.ExecutablePath) + @"\setup.zip" }, "/ScriptBrowser");
                    if (client.DirectoryExists("/ScriptBrowser/bin/"))
                        client.DeleteDirectory("/ScriptBrowser/bin/");

                    List<string> dirs = new List<string>(Directory.EnumerateDirectories(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\"));
                    dirs.Add(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\");

                    foreach (string dir in dirs)
                        client.UploadFiles(Directory.GetFiles(dir), dir.Replace(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\", "/ScriptBrowser/bin/").Replace("\\", "/"));
                    MessageBox.Show(this, "Export finished!");
                }
            }
            catch (Exception ex) { MessageBox.Show(this, "Could not upload!\n" + ex.StackTrace); }

            Directory.Delete(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\", true);
            File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + @"\setup.zip");
        }

        //File Watcher
        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            PrintLog();
        }

        private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
        {
            PrintLog();
        }

        //Blacklist
        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            foreach (string file in openFileDialog1.FileNames)
                blacklist.Add(file.Replace(scanLocation, ""));
            File.WriteAllText(blacklistFile, blacklist.ToString());
            PrintLog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = scanLocation;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK && folderBrowserDialog1.SelectedPath != scanLocation)
            {
                blacklist.Add(folderBrowserDialog1.SelectedPath.Replace(scanLocation, ""));
                File.WriteAllText(blacklistFile, blacklist.ToString());
                PrintLog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            blacklist.RemoveAll();
            PrintLog();
        }
    }
}
