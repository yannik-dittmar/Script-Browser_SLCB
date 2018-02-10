using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Net;
using Script_Browser.Controls;
using Script_Browser.Design;

namespace Script_Browser.TabPages
{
    public partial class Search : UserControl
    {
        public Main form = null;
        bool contextMenuOpen = false;

        public Search()
        {
            InitializeComponent();

            contextMenuStrip1.Renderer = new ArrowRenderer();
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
            if (e.Column.Index == 4 || e.Column.Index == 7)
            {
                e.SortResult = int.Parse(e.CellValue1.ToString()).CompareTo(int.Parse(e.CellValue2.ToString()));
                e.Handled = true;
            }
        }

        //Only row-selection when entering a cell
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[e.RowIndex].Selected = true;

                nAMEToolStripMenuItem.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
            catch { }
        }

        //Search
        private void metroTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1_Click(null, null);
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text.Trim(' ') != "")
            {
                string[] tags = metroTextBox1.Text.ToLower().Split(' ');
                if (tags.Count() > 0)
                {
                    try
                    {
                        string plainResult = Networking.SearchScripts(tags, form);
                        if (plainResult.Contains("empty"))
                        {
                            label1.Visible = true;
                            label1.Text = "No results found matching your tags.";
                        }
                        else
                        {
                            JObject result = JObject.Parse(plainResult);
                            label1.Visible = false;

                            dataGridView1.Rows.Clear();
                            foreach (KeyValuePair<string, JToken> row in result)
                            {
                                int rating = (int)Math.Round(Double.Parse(row.Value["Rating"].ToString().Replace(".", ",")));
                                string stars = "";
                                for (int i = 0; i < rating; i++)
                                    stars += "★";

                                dataGridView1.Rows.Add(row.Value["ID"], row.Value["Name"], row.Value["ShortDescription"], stars, row.Value["Downloads"], row.Value["Version"], row.Value["Username"], row.Value["Count"]);
                            }
                            dataGridView1.Sort(dataGridView1.Columns[7], ListSortDirection.Descending);
                            dataGridView1.ClearSelection();
                        }
                    }
                    catch (WebException) { MetroFramework.MetroMessageBox.Show(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error, 125); }
                    catch (Exception ex) { MetroFramework.MetroMessageBox.Show(form, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 150); Console.WriteLine(ex.StackTrace); }
                }
            }
        }

        //Load ScriptView
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                if (e.Button == MouseButtons.Left && !contextMenuOpen)
                {
                    string result = Networking.GetScriptById(form, dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    JObject script = JObject.Parse(result);

                    ShowScript ss = new ShowScript(script["ID"].ToString(), script["Name"].ToString(), script["Version"].ToString(), script["Username"].ToString(), script["ShortDescription"].ToString(), script["LongDescription"].ToString(), script["Rating"].ToString(), script["Ratings"].ToString(), script["Downloads"].ToString());
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
                }
            }
            catch (WebException) { MetroFramework.MetroMessageBox.Show(form, "There was an unexpected network error!\nPlease make sure you have an internet connection.", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error, 125); }
            catch (Exception ex) { MetroFramework.MetroMessageBox.Show(form, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 150); Console.WriteLine(ex.StackTrace); }
            contextMenuOpen = false;
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
                animatorScript.HideSync(panelScript);
                (sender as Control).Parent.Parent.Dispose();
            }
            catch { }
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
