using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateLogger
{
    public partial class Form1 : Form
    {
        private string settingsFile = @"";
        private string scanLocation = @"D:\MegaSync\Visual-Studio\Script-Browser_SLCB\Script-Browser\bin\Debug";
        private JArray Changelog; 

        public Form1()
        {
            InitializeComponent();

            try
            {
                if (File.Exists(settingsFile))
                    Changelog = JArray.Parse(File.ReadAllText(settingsFile));
                else
                    Changelog = new JArray();
            }
            catch { Changelog = new JArray(); }

            JObject lastScan = new JObject();
            lastScan["files"] = new JArray();
            lastScan["folders"] = new JArray();

            textBox1.Text = ScanChanges(lastScan).ToString().Replace("\\\\", "\\");
            textBox1.Select(0, 0);
        }

        private JObject ScanChanges(JObject lastScan)
        {
            JObject result = new JObject
            {
                ["date"] = DateTime.Now.ToString("dd.MM.yy"),
                ["files"] = new JObject
                {
                    ["stats"] = new JObject
                    {
                        ["changed"] = 0,
                        ["removed"] = 0
                    },
                    ["changed"] = new JArray(),
                    ["removed"] = new JArray()
                },
                ["folders"] = new JObject
                {
                    ["stats"] = new JObject
                    {
                        ["changed"] = 0,
                        ["removed"] = 0
                    },
                    ["changed"] = new JArray(),
                    ["removed"] = new JArray()
                }
            };

            string[][] subData = GetSubData(scanLocation);
            string[] currentFiles = subData[0];
            string[] currentFolders = subData[1];

            //Removed Files
            foreach (JToken lastfile in lastScan["files"])
            {
                if (!currentFiles.Contains(lastfile.ToString()))
                    ((JArray)result["files"]["removed"]).Add(lastfile);
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
                if (!((JArray)lastScan["files"]).Contains(currentFile))
                    ((JArray)result["files"]["changed"]).Add(currentFile);
            }

            //Added Folders
            foreach (string currentFolder in currentFolders)
            {
                if (!((JArray)lastScan["folders"]).Contains(currentFolder))
                    ((JArray)result["folders"]["changed"]).Add(currentFolder);
            }

            //Counters
            result["files"]["stats"]["changed"] = result["files"]["changed"].Count();
            result["files"]["stats"]["removed"] = result["files"]["removed"].Count();
            result["folders"]["stats"]["changed"] = result["folders"]["changed"].Count();
            result["folders"]["stats"]["removed"] = result["folders"]["removed"].Count();

            return result;
        }

        private string[][] GetSubData(string p)
        {
            HashSet<string> files = new HashSet<string>();
            HashSet<string> subfolders = new HashSet<string>(Directory.GetDirectories(p));
            HashSet<string> storedSubfolders = new HashSet<string>();

            string[] topFiles = Directory.GetFiles(p);
            foreach (string file in topFiles)
                files.Add(file.Replace(p, ""));

            while (subfolders.Count != 0)
            {
                string subfolder = subfolders.First();
                string[] subfiles = Directory.GetFiles(subfolder);

                foreach (string file in subfiles)
                    files.Add(file.Replace(p, ""));

                subfolders.Remove(subfolder);
                storedSubfolders.Add(subfolder.Replace(p, ""));

                string[] subsubfolders = Directory.GetDirectories(subfolder);
                foreach (string subsubfolder in subsubfolders)
                    subfolders.Add(subsubfolder);
            }

            return new string[][] { files.ToArray(), storedSubfolders.ToArray()};
        }
    }
}
