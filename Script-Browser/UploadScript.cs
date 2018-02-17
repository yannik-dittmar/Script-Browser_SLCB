using Markdig;
using MaterialSkin.Controls;
using Script_Browser.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser
{
    public partial class UploadScript : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;

        int currentStep = 1;
        int currentPage = 1;
        List<string> searchTags = new List<string>();

        public UploadScript(string path)
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            metroComboBox1.SelectedIndex = 0;
            materialSingleLineTextField1.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField2.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField3.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField4.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;

            fileSystemWatcher1.Path = path;
            UpdateDgvFiles(null, null);

            CheckScriptInformation(null, null);
            SetPage(1, true);
        }

        //
        // Windows API, Window Settings
        //
        private void MoveForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void UploadScript_Load(object sender, EventArgs e)
        {
            ShapeArrow(noFocusBorderBtn1, 0);
            ShapeArrow(noFocusBorderBtn2, 1);
            ShapeArrow(noFocusBorderBtn3, 1);
            ShapeArrow(noFocusBorderBtn4, 1);
            ShapeArrow(noFocusBorderBtn5, 2);
        }

        private void ShapeArrow(Control btn, int pos)
        {
            Size b = btn.Bounds.Size;
            Point l = btn.Location;
            List<Point> pts = new List<Point>{
                new Point(0, 0),
                new Point(b.Width - 15, 0),
                new Point(b.Width, (b.Height / 2)),
                new Point(b.Width - 15, b.Height),
                new Point(0, b.Height),
                new Point(15, (b.Height / 2))
            };

            if (pos == 0)
                pts.RemoveAt(5);
            else if (pos == 2)
            {
                pts[1] = new Point(b.Width, 0);
                pts[3] = new Point(b.Width, b.Height);
                pts.RemoveAt(2);
            }

            using (GraphicsPath polygon_path = new GraphicsPath(FillMode.Winding))
            {
                polygon_path.AddPolygon(pts.ToArray());
                btn.Region = new Region(polygon_path);
                btn.Location = l;
            }
        }

        private void SetPage(int page, bool updateTable)
        {
            if (page <= currentStep && page > 0)
            {
                for (int i = 0; i < tableLayoutPanel4.Controls.Count; i++)
                {
                    tableLayoutPanel4.Controls[i].BackColor = Color.FromArgb(25, 72, 70);

                    if ((i == page - 1 && updateTable) || (i == currentPage - 1 && !updateTable))
                        tableLayoutPanel4.Controls[i].BackColor = Color.FromArgb(51, 139, 118);
                }

                if (updateTable)
                {
                    for (int i = 0; i < tableLayoutTabControl.ColumnStyles.Count; i++)
                    {
                        try
                        {
                            if (i == page - 1)
                            {
                                tableLayoutTabControl.ColumnStyles[i].SizeType = SizeType.Percent;
                                tableLayoutTabControl.ColumnStyles[i].Width = 100f;
                                if (tableLayoutTabControl.Controls[i].Tag == null)
                                    tableLayoutTabControl.Controls[i].Visible = true;
                            }
                            else
                            {
                                tableLayoutTabControl.ColumnStyles[i].SizeType = SizeType.Absolute;
                                tableLayoutTabControl.ColumnStyles[i].Width = 0;
                                if (tableLayoutTabControl.Controls[i].Tag == null)
                                    tableLayoutTabControl.Controls[i].Visible = false;
                            }
                        }
                        catch { }
                    }
                }

                if (updateTable)
                {
                    if (page != 5)
                        noFocusBorderBtn6.Text = "Next";
                    else
                        noFocusBorderBtn6.Text = "Upload";
                    noFocusBorderBtn6.Enabled = page != currentStep || page == 5;
                    noFocusBorderBtn7.Enabled = page != 1;
                    currentPage = page;

                    switch (page)
                    {
                        case 1:
                            labelHelp.Text = "The general information to your script.\nThese will be shown in the browser and written into the script file itself.";
                            break;
                        case 2:
                            labelHelp.Text = "The long description will be shown when a user clicks on your script in the browser.\nThe text supports the Markdown language! For more details look in the \"Markdown Information\" tab.";
                            break;
                        case 3:
                            labelHelp.Text = "These tags help the user to find your script over the search function.\nTry to explain the script as detailed as possible.";
                            break;
                        case 4:
                            labelHelp.Text = "Select the files that should be installed for the user.\nWe recomend to deselect the settings files.";
                            break;
                        default:
                            labelHelp.Text = "";
                            break;
                    }
                }
            }
        }

        private void EnableStep(int step)
        {
            for (int i = 0; i < tableLayoutPanel4.Controls.Count; i++)
                tableLayoutPanel4.Controls[i].Enabled = i < step;
            currentStep = step;

            noFocusBorderBtn6.Enabled = currentPage != currentStep;
            noFocusBorderBtn7.Enabled = currentPage != 1;
        }

        private void noFocusBorderBtn1_MouseClick(object sender, MouseEventArgs e)
        {
            try { SetPage(Int32.Parse((sender as Control).Tag.ToString()), true); } catch { }
        }

        private void nextPage_Click(object sender, EventArgs e)
        {
            SetPage(currentPage + 1, true);
        }

        private void previousPage_Click(object sender, EventArgs e)
        {
            SetPage(currentPage - 1, true);
        }

        //
        // Tab Script Information
        //

        private void CheckScriptInformation(object sender, EventArgs e)
        {
            List<MaterialSingleLineTextField> textfields = new List<MaterialSingleLineTextField> { materialSingleLineTextField1, materialSingleLineTextField2, materialSingleLineTextField3, materialSingleLineTextField4 };

            bool ok = true;
            foreach (MaterialSingleLineTextField textfield in textfields)
            {
                if (textfield.Text.Trim(' ').Length == 0 && textfield.Tag.ToString() != "empty")
                {
                    EnableStep(1);
                    ok = false;
                    break;
                }
            }

            if (ok && currentStep == 1)
            {
                EnableStep(2);
                CheckDescription(null, null);
            }

            SetPage(1, sender != null);
        }

        //
        // Tab Description
        //
        //TODO: Add Markdown Info

        private void SwitchBtn(Button btn, bool enabled)
        {
            if (enabled)
                btn.BackColor = Color.FromArgb(51, 139, 118);
            else
                btn.BackColor = Color.FromArgb(18, 25, 31);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SwitchBtn(button1, true);
            SwitchBtn(button2, false);
            SwitchBtn(button3, false);
            webBrowser1.Visible = false;
            panelMarkdown.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.DocumentText = "<html><body>" + Markdown.ToHtml(richTextBox1.Text) + "</body></html>";
                SwitchBtn(button1, false);
                SwitchBtn(button2, true);
                SwitchBtn(button3, false);
                webBrowser1.Visible = true;
                panelMarkdown.Visible = false;
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SwitchBtn(button1, false);
            SwitchBtn(button2, false);
            SwitchBtn(button3, true);
            panelMarkdown.Visible = true;
        }

        //WebBrowser set Colors & Style
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                webBrowser1.Document.BackColor = Color.FromArgb(18, 25, 31);
                webBrowser1.Document.ForeColor = Color.White;

                webBrowser1.Document.Body.Style = "overflow:auto;margin=3px 6px;font-family: Arial;";
            }
            catch { }
        }

        //WebBrowser navigate url to extern browser
        public void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.ToString() != "about:blank")
            {
                try { Process.Start(e.Url.ToString()); } catch { }
                e.Cancel = true;
            }
        }

        private void CheckDescription(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Trim(' ').Trim('\n').Length > 0)
            {
                EnableStep(3);
                CheckTags(null, null);
            }
            else
                EnableStep(2);
            SetPage(2, sender != null);
        }

        //
        // Tab Tags
        //

        private void AddTags(object sender, EventArgs e)
        {
            try
            {
                if (metroTextBox1.Text.Trim(' ').Length > 0)
                {
                    string[] tags = metroTextBox1.Text.ToLower().Split(' ');
                    foreach (string tag in tags)
                    {
                        if (!searchTags.Contains(tag))
                        {
                            searchTags.Add(tag);
                            SearchTag st = new SearchTag(tag);
                            flowLayoutPanelTags.Controls.Add(st);
                            flowLayoutPanelTags.Controls.SetChildIndex(st, 0);
                            st.Disposed += new EventHandler(RemoveTag);
                            st.Tag = tag;
                        }
                    }
                    metroTextBox1.Text = "";
                }
            }
            catch { }
        }

        private void metroTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                AddTags(null, null);
                e.Handled = true;
            }
        }

        private void RemoveTag(object sender, EventArgs e)
        {
            try { searchTags.Remove((sender as Control).Tag.ToString()); } catch { }
        }

        private void CheckTags(object sender, ControlEventArgs e)
        {
            if (searchTags.Count > 0)
            {
                EnableStep(4);
                CheckFiles(null, null);
            }
            else
                EnableStep(3);
            SetPage(3, sender != null);
        }

        //
        // Tab Files
        //

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

        private void dataGridView1_MouseLeave(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            dgv.ClearSelection();
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
                }
                catch { }
            }
        }

        private void CheckFiles(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (sender != null)
                    dataGridView1.Rows[e.RowIndex].Selected = true;
            }
            catch { }

            bool found = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string path = row.Cells[2].Value.ToString();
                if ((bool)(row.Cells[0] as DataGridViewCheckBoxCell).Value && path.Split('\\')[path.Split('\\').Length - 1].Contains("_StreamlabsSystem.py") || path.Split('\\')[path.Split('\\').Length - 1].Contains("_AnkhBotSystem.py"))
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                if (currentStep >= 4)
                    EnableStep(5);
            }
            else if (currentStep >= 4)
                EnableStep(4);
            SetPage(4, currentPage == 5);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Selected = true;
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void UpdateDgvFiles(object sender, FileSystemEventArgs e)
        {
            try
            {
                dataGridView1.CellValueChanged -= new DataGridViewCellEventHandler(CheckFiles);
                dataGridView1.Rows.Clear();
                foreach (string file in Directory.GetFiles(Path.GetDirectoryName(fileSystemWatcher1.Path), "*.*", SearchOption.AllDirectories))
                {
                    try
                    {
                        Icon ico = SystemIcons.Warning;
                        try
                        {
                            ico = Icon.ExtractAssociatedIcon(file);
                        }
                        catch { }
                        dataGridView1.Rows.Add(!file.Contains("settings.js") && !file.Contains("settings.json"), ico, file.Replace(fileSystemWatcher1.Path, ""));
                    }
                    catch { }
                }
                dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(CheckFiles);
            }
            catch { }
            CheckFiles(null, null);
        }

        private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
        {
            UpdateDgvFiles(null, null);
        }
    }
}
