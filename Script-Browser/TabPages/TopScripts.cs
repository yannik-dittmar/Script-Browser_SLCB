using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Net;
using Script_Browser.Design;
using Script_Browser.Controls;
using MetroFramework;

namespace Script_Browser.TabPages
{
    public partial class TopScripts : UserControl
    {
        public Main form = null;
        int page = 1;
        bool contextMenuOpen = false;

        public TopScripts()
        {
            InitializeComponent();

            metroComboBox1.SelectedIndex = 0;
            metroComboBox2.SelectedIndex = 0;

            contextMenuStrip1.Renderer = new ArrowRenderer();
        }

        //Clear DataGridView selection
        private void TopScripts_Load(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
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

        //Sort numerically
        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 4)
            {
                e.SortResult = int.Parse(e.CellValue1.ToString()).CompareTo(int.Parse(e.CellValue2.ToString()));
                e.Handled = true;
            }
        }

        //Only row-selection when entering a cell
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                try
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;

                    nAMEToolStripMenuItem.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                }
                catch { dataGridView1.ClearSelection(); }
            }
            else
                dataGridView1.ClearSelection();
        }

        //Refresh & Download data from server
        private void button3_Click(object sender, EventArgs e)
        {
            LoadList();
        }

        public void LoadList(JArray result = null)
        {
            try
            {
                if (result == null)
                    result = JArray.Parse(Networking.GetTopScripts(metroComboBox2.Text, metroComboBox1.Text, page, form));

                if (result.Count == 0 && page > 1)
                    page--;
                else
                {
                    button2.Enabled = true;
                    dataGridView1.Rows.Clear();
                    foreach (JObject row in result)
                    {
                        int rating = (int)Math.Round(Double.Parse(row["Rating"].ToString().Replace(".", ",")));
                        string stars = "";
                        for (int i = 0; i < rating; i++)
                            stars += "★";

                        if (row["Alias"].ToString().Replace(" ", "") == "")
                            dataGridView1.Rows.Add(row["ID"], row["Name"], row["ShortDescription"], stars, row["Downloads"], row["Version"], row["Username"]);
                        else
                            dataGridView1.Rows.Add(row["ID"], row["Name"], row["ShortDescription"], stars, row["Downloads"], row["Version"], row["Alias"]);
                    }
                }

                button2.Enabled = result.Count == 30;

                button1.Enabled = page > 1;

                label2.Text = "Page " + page;
                if (metroComboBox1.Text == "Rating")
                    dataGridView1.Sort(dataGridView1.Columns[3], ListSortDirection.Descending);
                else
                    dataGridView1.Sort(dataGridView1.Columns[4], ListSortDirection.Descending);
                dataGridView1.ClearSelection();
            }
            catch { Networking.SMB(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 2); }
        }

        //Load ScriptView
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    form.UseWaitCursor = true;
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    if (e.Button == MouseButtons.Left && !contextMenuOpen)
                    {
                        string result = Networking.GetScriptById(form, dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                        JObject script = JObject.Parse(result);

                        ShowScript ss = new ShowScript(form, script["ID"].ToString(), script["Name"].ToString(), script["Version"].ToString(), script["Username"].ToString(), script["Alias"].ToString(), script["ShortDescription"].ToString(), script["LongDescription"].ToString(), script["Rating"].ToString(), script["Ratings"].ToString(), script["Downloads"].ToString());
                        ss.pictureBox1.MouseClick += new MouseEventHandler(unloadScript);
                        ss.Dock = DockStyle.Fill;
                        panelScript.Size = this.Size;
                        panelScript.Controls.Add(ss);
                        ss.Size = panelScript.Size;

                        int slidecoeff = -1 * (int)(this.Width * 0.002);
                        if (slidecoeff >= 0)
                            slidecoeff = -1;

                        animatorScript.DefaultAnimation.SlideCoeff = new PointF(slidecoeff, 0);
                        animatorScript.ShowSync(panelScript);
                        panelBrowse.Visible = false;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); Networking.SMB(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 2); }
            contextMenuOpen = false;
            form.UseWaitCursor = false;
        }

        //Unload ScriptView
        public void unloadScript(object sender, MouseEventArgs e)
        {
            try
            {
                int slidecoeff = -1 * (int)(this.Width * 0.002);
                if (slidecoeff >= 0)
                    slidecoeff = -1;

                animatorScript.DefaultAnimation.SlideCoeff = new PointF(slidecoeff, 0);
                panelBrowse.Visible = true;
                animatorScript.HideSync(panelScript);
                (sender as Control).Parent.Parent.Dispose();
            }
            catch { }
        }

        //Switch Pages
        private void button1_Click(object sender, EventArgs e)
        {
            page--;
            button1.Enabled = page > 1;
            button3_Click(null, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            page++;
            button3_Click(null, null);
        }

        //Refresh page after changing parameters
        private void metroComboBox_TextChanged(object sender, EventArgs e)
        {
            page = 1;
            button3_Click(null, null);
        }

        //Load ScriptView over contextMenu
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuOpen = false;
            dataGridView1_CellMouseClick(null, new DataGridViewCellMouseEventArgs(0, dataGridView1.SelectedRows[0].Index, 0, 0, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0)));
        }

        //Hide contextMenu when no row selected
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = dataGridView1.SelectedRows.Count == 0;
        }

        //Set contextMenu state
        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            contextMenuOpen = true;
        }
    }
}
