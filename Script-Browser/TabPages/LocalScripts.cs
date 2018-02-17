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
using SaveManager;
using System.Threading;

namespace Script_Browser.TabPages
{
    public partial class LocalScripts : UserControl
    {
        public Main form;

        public LocalScripts()
        {
            InitializeComponent();
        }

        private void LocalScripts_Load(object sender, EventArgs e)
        {
            UpdateList(@"D:\Streamlabs Chatbot\");
            //UpdateList(@"C:\Users\18diyann\Desktop\Test Ordner\");
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
                    string scriptFile = "UNDEF";
                    string type = "UNDEF";
                    string name = dir.Split('\\')[dir.Split('\\').Length - 1];
                    string description = "UNDEF";
                    string author = "UNDEF";
                    string version = "UNDEF";
                    int id = 0;

                    foreach (string file in files)
                    {
                        if (file.Split('\\')[file.Split('\\').Length - 1].Contains("_StreamlabsSystem.py") || file.Split('\\')[file.Split('\\').Length - 1].Contains("_AnkhBotSystem.py"))
                            type = "Command";
                        else if (file.Split('\\')[file.Split('\\').Length - 1].Contains("_StreamlabsParameter.py") || file.Split('\\')[file.Split('\\').Length - 1].Contains("_AnkhBotParameter.py"))
                            type = "Parameter";

                        if (type != "UNDEF")
                        {
                            scriptFile = file;

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
                                else if (line.ToLower().Contains("scriptbrowserid"))
                                    id = Int32.Parse(GetLineItem(line));
                            }

                            break;
                        }
                    }

                    if (scriptFile != "UNDEF")
                    {
                        if (id == 0)
                            dataGridView1.Rows.Add(name, description, type, version, author, scriptFile);
                        else
                            dataGridView3.Rows.Add(name, description, type, version, author, scriptFile, id);
                    }
                }

                if (dataGridView1.RowCount == 0)
                {
                    label1.Visible = false;
                    dataGridView1.Visible = false;
                    tableLayoutPanel1.RowStyles[1].SizeType = SizeType.AutoSize;
                }
                else
                {
                    label1.Visible = true;
                    dataGridView1.Visible = true;
                    tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Percent;
                    tableLayoutPanel1.RowStyles[1].Height = 50f;
                }

                if (dataGridView3.RowCount == 0)
                {
                    label2.Visible = false;
                    dataGridView3.Visible = false;
                    tableLayoutPanel1.RowStyles[3].SizeType = SizeType.AutoSize;
                }
                else
                {
                    label2.Visible = true;
                    dataGridView3.Visible = true;
                    tableLayoutPanel1.RowStyles[3].SizeType = SizeType.Percent;
                    tableLayoutPanel1.RowStyles[3].Height = 50;
                }

                label4.Visible = dataGridView1.RowCount == 0 && dataGridView3.RowCount == 0;
                dataGridView1.ClearSelection();
                dataGridView3.ClearSelection();
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
            try
            {
                DataGridView dgv = sender as DataGridView;
                if (e.RowIndex % 2 == 0)
                {
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(22, 36, 45);
                    dgv.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(32, 53, 66);
                }
                else
                {
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(18, 31, 39);
                    dgv.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 55, 69);
                }
            }
            catch { }
        }

        //Only row-selection when entering a cell
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridView dgv = sender as DataGridView;
                    dgv.ClearSelection();
                    dgv.Rows[e.RowIndex].Selected = true;

                    //nAMEToolStripMenuItem.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                }
                catch { }
            }
        }

        private void dataGridView1_MouseLeave(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            dgv.ClearSelection();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridView dgv = sender as DataGridView;
                    dgv.Rows[e.RowIndex].Selected = true;

                    button5.Visible = false;
                    button1.Visible = dgv.Tag.ToString().Contains("upload");
                    if (dgv.Tag.ToString().Contains("update"))
                    {
                        button3.Visible = !Networking.scripts.Contains(dgv.Rows[e.RowIndex].Cells[6].Value.ToString());
                        button4.Visible = !Networking.scripts.Contains(dgv.Rows[e.RowIndex].Cells[6].Value.ToString());
                        button5.Visible = Networking.scripts.Contains(dgv.Rows[e.RowIndex].Cells[6].Value.ToString());
                    }
                    else
                    {
                        button3.Visible = false;
                        button4.Visible = false;
                        button5.Visible = false;
                    }

                    label3.Text = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();

                    tableLayoutPanel2.Visible = true;
                }
                catch { }
            }
            else
                tableLayoutPanel2.Visible = false;
        }

        private void hideFooter_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView dgv = sender as DataGridView;
                if (dgv.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.None)
                    tableLayoutPanel2.Visible = false;
            }
            catch { tableLayoutPanel2.Visible = false; }
        }

        //Upload
        private void button1_Click(object sender, EventArgs e)
        {
            form.Opacity = 0.5;
            //new UploadScript().ShowDialog();
            form.Opacity = 1;
        }
    }
}
