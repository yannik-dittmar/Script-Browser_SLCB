using Markdig;
using MaterialSkin.Controls;
using MetroFramework;
using Newtonsoft.Json.Linq;
using Script_Browser.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

        string path = "";
        int currentStep = 1;
        int currentPage = 1;
        List<string> searchTags = new List<string>();
        bool uploaded = false;

        public UploadScript(string path)
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            materialSingleLineTextField1.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField2.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField3.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField4.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;

            try
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    if (line.ToLower().Contains("scriptname") && materialSingleLineTextField1.Text == "")
                        materialSingleLineTextField1.Text = GetLineItem(line);
                    else if (line.ToLower().Contains("description") && materialSingleLineTextField2.Text == "")
                        materialSingleLineTextField2.Text = GetLineItem(line);
                    else if (line.ToLower().Contains("version") && materialSingleLineTextField3.Text == "")
                        materialSingleLineTextField3.Text = GetLineItem(line);
                    else if (line.ToLower().Contains("creator") && materialSingleLineTextField4.Text == "" && GetLineItem(line) != Networking.username)
                        materialSingleLineTextField4.Text = GetLineItem(line);
                }
            }
            catch { }

            this.path = path;
            label3.Text = Path.GetDirectoryName(path) + "\\";
            fileSystemWatcher1.Path = Path.GetDirectoryName(path) + "\\";
            UpdateDgvFiles(null, null);

            if (path.Split('\\')[path.Split('\\').Length - 1].Contains("_StreamlabsParameter.py") || path.Split('\\')[path.Split('\\').Length - 1].Contains("_AnkhBotParameter.py"))
                metroComboBox1.SelectedIndex = 1;
            else
                metroComboBox1.SelectedIndex = 0;

            CheckScriptInformation(null, null);
            SetPage(1, true);
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
            return "";
        }

        private void label3_Click(object sender, EventArgs e)
        {
            try { Process.Start(Path.GetDirectoryName(path)); } catch { }
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
                    if (uploaded)
                        noFocusBorderBtn6.Text = "Finish";
                    else if (page != 5)
                        noFocusBorderBtn6.Text = "Next";
                    else
                        noFocusBorderBtn6.Text = "Upload";
                    noFocusBorderBtn6.Enabled = page != currentStep || (page == 5 && !uploaded);
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
            if (noFocusBorderBtn6.Text == "Upload" && !uploaded)
                Upload(null, null);
            else if (noFocusBorderBtn6.Text == "Finish")
                this.Dispose();
            else
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
            SetPage(4, currentPage == 4);
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

        //
        // Tab Upload
        //

        private void Upload(object sender, EventArgs e)
        {
            try
            {
                noFocusBorderBtn8.Visible = false;
                label10.Text = "Uploading your script... This could take some seconds!";
                label10.Refresh();

                //Copy files to temp dir
                string path = Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Script\";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                foreach (string file in Directory.GetFiles(path))
                    File.Delete(file);
                foreach (string dir in Directory.GetDirectories(path))
                    Directory.Delete(dir, true);
                if (File.Exists(Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip"))
                    File.Delete(Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip");

                string[] lines = File.ReadAllLines(this.path);
                using (StreamWriter writer = new StreamWriter(this.path))
                {
                    bool s = false, d = false, v = false, c = false;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].ToLower().Contains("scriptname") && !s)
                        {
                            writer.WriteLine(PutInValue(lines[i], materialSingleLineTextField1.Text));
                            s = true;
                        }
                        else if (lines[i].ToLower().Contains("description") && !d)
                        { 
                            writer.WriteLine(PutInValue(lines[i], materialSingleLineTextField2.Text));
                            d = true;
                        }
                        else if (lines[i].ToLower().Contains("version") && !v)
                        {
                            writer.WriteLine(PutInValue(lines[i], materialSingleLineTextField3.Text));
                            v = true;
                        }
                        else if (lines[i].ToLower().Contains("creator") && !c)
                        { 
                            writer.WriteLine(PutInValue(lines[i], materialSingleLineTextField4.Text));
                            c = true;
                        }
                        else
                            writer.WriteLine(lines[i]);
                    }
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if ((bool)(row.Cells[0] as DataGridViewCheckBoxCell).Value)
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path + row.Cells[2].Value.ToString()));
                        File.Copy(Path.GetDirectoryName(this.path) + "\\" + row.Cells[2].Value.ToString(), path + row.Cells[2].Value.ToString(), true);
                    }
                }

                ZipFile.CreateFromDirectory(path, Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip");

                foreach (string file in Directory.GetFiles(path))
                    File.Delete(file);
                foreach (string dir in Directory.GetDirectories(path))
                    Directory.Delete(dir, true);

                long size = new FileInfo(Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip").Length;
                if (size >= 31457280)
                {
                    MetroMessageBox.Show(this, "The file you are trying to upload has a size of " + ((size / 1024f) / 1024f) + "MB.\n But only files up to 30MB are allowed.", "File is too large", MessageBoxButtons.OK, 150);
                    return;
                }

                string[] add = new string[] { materialSingleLineTextField1.Text, materialSingleLineTextField4.Text };
                AddToListNotExists(add);

                JObject info = new JObject();
                info["Name"] = materialSingleLineTextField1.Text;
                info["ShortDescription"] = materialSingleLineTextField2.Text;
                info["Version"] = materialSingleLineTextField3.Text;
                info["Alias"] = materialSingleLineTextField4.Text;
                if (metroComboBox1.SelectedIndex == 0)
                    info["Type"] = "Command";
                else
                    info["Type"] = "Parameter";
                info["LongDescription"] = richTextBox1.Text;
                info["Tags"] = new JArray(searchTags.ToArray());

                string result = Networking.UploadScript(this, info.ToString(), Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip");

                if (result.Contains("verify"))
                    MetroMessageBox.Show(this, "Your email address has not been verified yet.\nPlease check your inbox or contact us over ", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 150); //TODO: Add Email
                else if (result.Contains("enough"))
                    MetroMessageBox.Show(this, "You have reached the maximum amount of scripts for a single user!\nDelete some to upload new ones.", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 150); //TODO: Add delete script from cloud
                else if (result.Contains("true"))
                {
                    label10.Text = "Your script has been successfully uploaded and published!";
                    uploaded = true;
                    Networking.scripts.Add(result.Replace("true", ""));
                    noFocusBorderBtn6.Text = "Finish";
                    File.Delete(Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip");

                    //Update local script file
                    lines = File.ReadAllLines(this.path);
                    using (StreamWriter writer = new StreamWriter(this.path))
                    {
                        bool found = false;
                        for (int i = 0; i < lines.Length; i++)
                        {
                            writer.WriteLine(lines[i]);
                            if (lines[i].ToLower().Contains("version") && !found)
                            {
                                writer.WriteLine("ScriptBrowserID = \"" + result.Replace("true", "") + "\"");
                                found = true;
                            }  
                        }

                        if (!found)
                        {
                            writer.WriteLine("");
                            writer.WriteLine("ScriptBrowserID = \"" + result.Replace("true", "") + "\"");
                        }
                    }
                    return;
                }

                File.Delete(Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip");
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
            label10.Text = "There was an error while uploading your script :/";
            noFocusBorderBtn8.Visible = true;
        }

        private string PutInValue(string line, string value)
        {
            List<char> charLine = line.ToList();

            bool found = false;
            for (int i = 0; i < charLine.Count; i++)
            {
                if (charLine[i] == '\"')
                {
                    if (!found)
                        found = true;
                    else
                    {
                        foreach (char c in value.ToCharArray().Reverse())
                            charLine.Insert(i, c);

                        string result = "";
                        foreach (char c in charLine)
                            result += c;
                        return result;
                    }
                }
                else if (found)
                {
                    charLine.RemoveAt(i);
                    i--;
                }
            }

            return line;
        }

        private void AddToListNotExists(string[] add)
        {
            foreach (string item in add)
            {
                string[] tags = item.Split(' ');
                foreach (string tag in tags)
                {
                    if (!searchTags.Contains(tag.ToLower()) && tag != "")
                        searchTags.Add(tag);
                }
            }
        }
    }
}
