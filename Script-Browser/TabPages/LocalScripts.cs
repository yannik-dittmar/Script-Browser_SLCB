using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Script_Browser.TabPages
{
    public partial class LocalScripts : UserControl
    {
        public LocalScripts()
        {
            InitializeComponent();
            UpdateList(@"D:\Streamlabs Chatbot\");
        }

        public void UpdateList(string path)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path + @"Twitch\Scripts\");

                dataGridView1.Rows.Clear();
                foreach (string dir in dirs)
                {
                    string[] files = Directory.GetFiles(dir);
                    string type = "UNDEF";
                    string name = dir.Split('\\')[dir.Split('\\').Length - 1];
                    string description = "UNDEF";
                    string author = "UNDEF";
                    string version = "UNDEF";

                    foreach (string file in files)
                    {
                        if (file.Split('\\')[file.Split('\\').Length - 1].Contains("_StreamlabsSystem.py") || file.Split('\\')[file.Split('\\').Length - 1].Contains("_AnkhBotSystem.py"))
                            type = "Command";
                        else if (file.Split('\\')[file.Split('\\').Length - 1].Contains("_StreamlabsParameter.py") || file.Split('\\')[file.Split('\\').Length - 1].Contains("_AnkhBotParameter.py"))
                            type = "Parameter";

                        if (type != "UNDEF")
                        {
                            string[] lines = File.ReadAllLines(file);
                            foreach (string line in lines)
                            {
                                if (line.ToLower().Contains("scriptname") && name == "UNDEF")
                                    name = GetLineItem(line);
                                else if (line.ToLower().Contains("description") && description == "UNDEF")
                                    description = GetLineItem(line);
                                else if (line.ToLower().Contains("creator") && author == "UNDEF")
                                    author = GetLineItem(line);
                                else if (line.ToLower().Contains("version") && version == "UNDEF")
                                    version = GetLineItem(line);
                            }

                            break;
                        }
                    }

                    dataGridView1.Rows.Add(-1, name, description, type, version, author, dir);
                }
            }
            catch { }
        }

        public static string GetLineItem(string line)
        {
            try
            {
                if (line.IndexOf('"') != -1)
                {
                    string result = line.Substring(line.IndexOf('"') + 1);
                    result = result.Substring(0, result.IndexOf('"'));
                    return result;
                }
            }
            catch { }
            return "UNDEF";
        }

        //Change colors for the rows to get pattern
        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex % 2 == 0)
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(22, 36, 45);
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(32, 53, 66);
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(18, 31, 39);
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 55, 69);
            }
        }

        //Only row-selection when entering a cell
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[e.RowIndex].Selected = true;

                //nAMEToolStripMenuItem.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
            catch { }
        }
    }
}
