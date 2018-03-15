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
        #region DLL-Methodes & Variables

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

        #endregion

        string path = "";
        int currentStep = 5;
        int currentPage = 1;
        List<string> searchTags = new List<string>();
        bool uploaded = false;
        JObject uuInfo;
        JObject fileChanges = new JObject { ["Delete"] = new JArray(), ["Move"] = new JArray(), ["Copy"] = new JArray() };

        public UploadScript(string path, JObject _uuInfo)
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            //Style für Textfelder setzen
            materialSingleLineTextField1.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField2.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField3.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField4.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;

            try
            {
                //Information über Script aus Script-Datei lesen
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

            //Pfad globalisieren und in Elemente einfügen
            this.path = path;
            label3.Text = Path.GetDirectoryName(path) + "\\";
            fileSystemWatcher1.Path = Path.GetDirectoryName(path) + "\\";
            UpdateDgvFiles(null, null); //Dateien im Scriptordner erfassen

            //Bestimmen des Script Typs
            if (path.Split('\\')[path.Split('\\').Length - 1].Contains("_StreamlabsParameter.py") || path.Split('\\')[path.Split('\\').Length - 1].Contains("_AnkhBotParameter.py"))
                metroComboBox1.SelectedIndex = 1;
            else
                metroComboBox1.SelectedIndex = 0;

            //Überprüfen ob bereits Informationen zum Script mitgegeben wurden, wenn ja handelt es sich um ein Update
            uuInfo = _uuInfo;
            if (uuInfo != null)
            {
                this.Text = "Upload Update";
                label1.Text = "Script Browser - Upload Update";

                richTextBox1.Text = uuInfo["LongDescription"].ToString(); //Lange Beschreibung setzen

                //Tags hinzufügen
                string tags = "";
                foreach (JToken tag in uuInfo["Tags"] as JArray)
                    tags += tag + " ";
                metroTextBox1.Text = tags;
                AddTags(null, null);

                //Zeige bestimmte Bereiche, die nur für das Updaten von Dateien gedacht sind
                flowLayoutPanel1.Visible = true;
                tableLayoutPanel11.Visible = true;
            }
        }

        //Ein string zwischen zwei " zurückgeben
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

        #region Windows API, Window Settings

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
            //Buttons wie Pfeile formen (für Navbar oben)
            ShapeArrow(noFocusBorderBtn1, 0);
            ShapeArrow(noFocusBorderBtn2, 1);
            ShapeArrow(noFocusBorderBtn3, 1);
            ShapeArrow(noFocusBorderBtn4, 1);
            ShapeArrow(noFocusBorderBtn5, 2);

            //Alle Fenster laden und wieder zurück zum ersten gehen
            currentStep = 5;
            SetPage(1, true);
            SetPage(2, true);
            SetPage(3, true);
            SetPage(4, true);
            SetPage(5, true);
            SetPage(1, true);
            CheckScriptInformation(null, null);
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

        #endregion

        //Neue Tabseite setzen
        private void SetPage(int page, bool updateTable)
        {
            if (page <= currentStep && page > 0)
            {
                //Navbar Farben updaten
                for (int i = 0; i < tableLayoutPanel4.Controls.Count; i++)
                {
                    tableLayoutPanel4.Controls[i].BackColor = Color.FromArgb(25, 72, 70);

                    if ((i == page - 1 && updateTable) || (i == currentPage - 1 && !updateTable))
                        tableLayoutPanel4.Controls[i].BackColor = Color.FromArgb(51, 139, 118);
                }

                //Andere Tabseiten verstecken, nur derzeitige anzeigen
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
                    //Button unten rechts aktualisieren
                    if (uploaded)
                        noFocusBorderBtn6.Text = "Finish";
                    else if (page != 5)
                        noFocusBorderBtn6.Text = "Next";
                    else
                        noFocusBorderBtn6.Text = "Upload";
                    noFocusBorderBtn6.Enabled = page != currentStep || (page == 5 && !uploaded);
                    noFocusBorderBtn7.Enabled = page != 1;
                    currentPage = page;

                    //Info Text zu jeweiliger Seite aktualisieren
                    if (uuInfo == null)
                    {
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
                    else
                    {
                        switch (page)
                        {
                            case 1:
                                labelHelp.Text = "Please notice if you don't change the version, you only change the information of your script.\nFiles will not be updated.";
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
        }

        //Festlegen bis wohin der Nutzer maximal gehen kann
        private void EnableStep(int step)
        {
            for (int i = 0; i < tableLayoutPanel4.Controls.Count; i++)
                tableLayoutPanel4.Controls[i].Enabled = i < step;
            currentStep = step;

            noFocusBorderBtn6.Enabled = currentPage != currentStep;
            noFocusBorderBtn7.Enabled = currentPage != 1;
        }

        //Tabseiten auf Knopfdruck von Navbar wechseln
        private void noFocusBorderBtn1_MouseClick(object sender, MouseEventArgs e)
        {
            try { SetPage(Int32.Parse((sender as Control).Tag.ToString()), true); } catch { }
        }

        //Nächste Seite anzeigen bzw. Script-Upload starten
        private void nextPage_Click(object sender, EventArgs e)
        {
            if (noFocusBorderBtn6.Text == "Upload" && !uploaded)
                Upload(null, null);
            else if (noFocusBorderBtn6.Text == "Finish")
                this.Dispose();
            else
                SetPage(currentPage + 1, true);
        }

        //Vorherige Seite anzeigen
        private void previousPage_Click(object sender, EventArgs e)
        {
            SetPage(currentPage - 1, true);
        }

        #region Tab: Script Information

        //Überprüfen ob Eingabefelder Text beinhalten
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
                if (textfield.Tag.ToString() == "version" && uuInfo != null)
                    label11.Visible = textfield.Text == uuInfo["Version"].ToString();
            }

            //Festlegen ob die weiteren Einstellungen für den Nutzer freigegeben werden
            if (ok && currentStep == 1)
            {
                EnableStep(2);
                CheckDescription(null, null);
            }

            SetPage(1, sender != null);
        }

        #endregion

        #region Tab: Description
        //TODO: Add Markdown Info

        //Farbe der Buttons ändern
        private void SwitchBtn(Button btn, bool enabled)
        {
            if (enabled)
                btn.BackColor = Color.FromArgb(51, 139, 118);
            else
                btn.BackColor = Color.FromArgb(18, 25, 31);
        }

        //Texteingabe anzeigen
        private void button1_Click(object sender, EventArgs e)
        {
            SwitchBtn(button1, true);
            SwitchBtn(button2, false);
            SwitchBtn(button3, false);
            webBrowser1.Visible = false;
            panelMarkdown.Visible = false;
        }

        //Vorschau des Textes anzeigen
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.DocumentText = "<html><body>" + Markdown.ToHtml(richTextBox1.Text).Replace("\n", "<br>") + "</body></html>"; //WebBrowser mit Text von Nutzer laden
                SwitchBtn(button1, false);
                SwitchBtn(button2, true);
                SwitchBtn(button3, false);
                webBrowser1.Visible = true;
                panelMarkdown.Visible = false;
            }
            catch { }
        }

        //Markdown Informationen anzeigen
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

        //Überprüfen ob Nutzer eine Beschreibung hinzugefügt hat
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

        #endregion

        #region Tab: Tags

        //Einen Tag hinzufügen und anzeigen
        private void AddTags(object sender, EventArgs e)
        {
            try
            {
                if (metroTextBox1.Text.Trim(' ').Length > 0)
                {
                    string[] tags = metroTextBox1.Text.ToLower().Split(' ');
                    foreach (string tag in tags)
                    {
                        if (!searchTags.Contains(tag) && tag.Length > 0 && searchTags.Count < 45)
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

        //Tag hinzufügen, wenn Nutzer EINGABE drückt
        private void metroTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                AddTags(null, null);
                e.Handled = true;
            }
        }

        //Tag entfernen
        private void RemoveTag(object sender, EventArgs e)
        {
            try { searchTags.Remove((sender as Control).Tag.ToString()); } catch { }
        }

        //Überprüfen ob Tags existieren
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

        #endregion

        #region Tab: Files

        //Zeilen des DataGridViews mit unterschiedlichen Hintergrundfarben zeichnen 
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

        //Zeile aufheben, wenn Maus nicht mehr im DataGridView
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

        //Überprüfen ob die Haupt-Scriptdatei existiert und ausgewählt ist
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

        //Bei einmaligen klick auf eine Zelle diese bereits bearbeiten
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Selected = true;
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        //Datei Änderungen löschen, wenn Nutzer das Häkchen entfernt
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!(bool)dataGridView1.Rows[e.RowIndex].Cells[0].Value && dataGridView1.Rows[e.RowIndex].Cells[3].Value + "" != "")
                {
                    JObject info = JObject.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value + "");
                    if (info["Type"].ToString() == "Delete")
                    {
                        for (int i = 0; i < (fileChanges["Delete"] as JArray).Count; i++)
                        {
                            if ((fileChanges["Delete"] as JArray)[i].ToString() == info["Value"].ToString())
                            {
                                (fileChanges["Delete"] as JArray).RemoveAt(i);
                                break;
                            }
                        }
                    }
                    else if (info["Type"].ToString() == "Move")
                    {
                        for (int i = 0; i < (fileChanges["Move"] as JArray).Count; i++)
                        {
                            JToken token = ((fileChanges["Move"] as JArray)[i] as JToken);
                            if (token["From"].ToString() == info["From"].ToString() && token["To"].ToString() == info["To"].ToString())
                            {
                                (fileChanges["Move"] as JArray).RemoveAt(i);
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < (fileChanges["Copy"] as JArray).Count; i++)
                        {
                            JToken token = ((fileChanges["Copy"] as JArray)[i] as JToken);
                            if (token["From"].ToString() == info["From"].ToString() && token["To"].ToString() == info["To"].ToString())
                            {
                                (fileChanges["Copy"] as JArray).RemoveAt(i);
                                break;
                            }
                        }
                    }

                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
            }
            catch { }
            CheckFiles(null, null);
        }

        //Es wurden Änderungen extern an den Dateien vorgenommen -> aktualisieren der Anzeige
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
                            ico = Icon.ExtractAssociatedIcon(file); //Datei bezogenes Icon extrahieren
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

        //Nutzer fügt einen Lösch-Befehl hinzu (Nur bei Updaten)
        private void noFocusBorderBtn10_Click(object sender, EventArgs e)
        {
            JObject output = new JObject();
            this.Opacity = 0.5;
            new FileChangesForUpdate(FileChangesForUpdate.Types.Delete, output).ShowDialog();
            this.Opacity = 1;

            if (output.Count > 0 && !(fileChanges["Delete"] as JArray).Contains(output["Value"]))
            {
                (fileChanges["Delete"] as JArray).Add(output["Value"]);
                dataGridView1.Rows.Add(true, Properties.Resources.delete_file, output["Value"], new JObject() { ["Type"] = "Delete", ["Value"] = output["Value"] }.ToString());
            }
        }

        //Nutzer fügt einen Move-Befehl hinzu (Nur bei Updaten)
        private void noFocusBorderBtn11_Click(object sender, EventArgs e)
        {
            JObject output = new JObject();
            this.Opacity = 0.5;
            new FileChangesForUpdate(FileChangesForUpdate.Types.MoveOrCopy, output).ShowDialog();
            this.Opacity = 1;

            if (output.Count > 0 && !(fileChanges["Move"] as JArray).Contains(output))
            {
                (fileChanges["Move"] as JArray).Add(output);
                dataGridView1.Rows.Add(true, Properties.Resources.move_file, output["From"] + " ▶ " + output["To"], new JObject() { ["Type"] = "Move", ["From"] = output["From"], ["To"] = output["To"] }.ToString());
            }
        }

        //Nutzer fügt einen Copy-Befehl hinzu (Nur bei Updaten)
        private void noFocusBorderBtn12_Click(object sender, EventArgs e)
        {
            JObject output = new JObject();
            this.Opacity = 0.5;
            new FileChangesForUpdate(FileChangesForUpdate.Types.MoveOrCopy, output).ShowDialog();
            this.Opacity = 1;

            if (output.Count > 0 && !(fileChanges["Copy"] as JArray).Contains(output))
            {
                (fileChanges["Copy"] as JArray).Add(output);
                dataGridView1.Rows.Add(true, Properties.Resources.copy_file, output["From"] + " ▶ " + output["To"], new JObject() { ["Type"] = "Copy", ["From"] = output["From"], ["To"] = output["To"] }.ToString());
            }
        }

        #endregion

        #region Tab: Upload

        //Script hochladen
        private void Upload(object sender, EventArgs e)
        {
            try
            {
                //Nutzer oberfläche aktualsieren
                noFocusBorderBtn8.Visible = false;
                label10.Text = "Uploading your script... This could take some seconds!";
                label10.Refresh();

                //Pfad des Zielverzeichnisses zur temporären Speicherung festlegen
                string path = Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Script\";
                PrepareFile();

                //Verbleibende Tags hinzufügen, Script-Name und Alias
                string[] add = new string[] { materialSingleLineTextField1.Text, materialSingleLineTextField4.Text };
                AddToListNotExists(add);

                //Alle Informationen über das Script in einem JObject Speichern
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

                //Script versuchen hochzuladen
                string result = Networking.UploadScript(this, info.ToString(), Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip");

                if (result.Contains("verify")) //Anmeldedaten waren falsch oder Benutzerkonto ist noch nicht verifiziert
                    MetroMessageBox.Show(this, "Your email address has not been verified yet.\nPlease check your inbox or contact us over ", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 150); //TODO: Add Email
                else if (result.Contains("enough")) //Benutzer hat bereits die maximale Anzahl an Scripts erreicht und kann keine neuen mehr hochladen
                    MetroMessageBox.Show(this, "You have reached the maximum amount of scripts for a single user!\nDelete some to upload new ones.", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 150); //TODO: Add delete script from cloud
                else if (result.Contains("true")) //Script wurde erfolgreich Hochgeladen
                {
                    label10.Text = "Your script has been successfully uploaded and published!";
                    uploaded = true;
                    Networking.scripts.Add(result.Replace("true", "")); //Script zu eigenen Scripts hinzufügen
                    noFocusBorderBtn6.Text = "Finish";
                    noFocusBorderBtn8.Enabled = false; 
                    File.Delete(Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip"); //Zip-Archiv löschen

                    //Lokale Scriptdatei mit der neuen ScriptID speichern
                    string[] lines = File.ReadAllLines(this.path);
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
                    return; //Methode beenden
                }

                File.Delete(Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip"); //Zip-Archiv löschen
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
            //Unbekannter Fehler
            label10.Text = "There was an error while uploading your script :/";
            noFocusBorderBtn8.Visible = true;
        }

        //Update zu einem Script hochladen
        private void UploadUpdate(object sender, EventArgs e)
        {
            try
            {
                noFocusBorderBtn9.Text = "Uploading...";
                noFocusBorderBtn9.Refresh();

                //Pfad des Zielverzeichnisses zur temporären Speicherung festlegen
                string path = Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Script\";

                //Datei nur bereitstellen, wenn die Versionsnummer geändert wurde
                if (materialSingleLineTextField3.Text != uuInfo["Version"].ToString())
                    PrepareFile();

                //Verbleibende Tags hinzufügen, Script-Name und Alias
                string[] add = new string[] { materialSingleLineTextField1.Text, materialSingleLineTextField4.Text };
                AddToListNotExists(add);

                //Alle Informationen über das Script in einem JObject Speichern
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
                info["FileChanges"] = fileChanges;
                info["Message"] = richTextBox2.Text;
                info["ID"] = uuInfo["ID"];

                //Script versuchen hochzuladen
                string result = "";
                if (materialSingleLineTextField3.Text != uuInfo["Version"].ToString())
                    result = Networking.UploadUpdate(this, info.ToString(), Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip");
                else
                    result = Networking.UploadUpdate(this, info.ToString());

                //Ergebnis untersuchen
                if (result.Contains("verify"))
                    MetroMessageBox.Show(this, "Your email address has not been verified yet.\nPlease check your inbox or contact us over ", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 150); //TODO: Add Email
                else if (result.Contains("enough"))
                    MetroMessageBox.Show(this, "You have reached the maximum amount of scripts for a single user!\nDelete some to upload new ones.", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 150); //TODO: Add delete script from cloud
                else if (result.Contains("true"))
                {
                    noFocusBorderBtn9.Text = "Success";
                    noFocusBorderBtn9.Enabled = false;
                    uploaded = true;
                    noFocusBorderBtn6.Text = "Finish";
                    noFocusBorderBtn8.Enabled = false;

                    try { File.Delete(Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip"); } catch { }
                    return;
                }

                try { File.Delete(Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip"); } catch { }
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
            noFocusBorderBtn9.Text = "Update Script";
        }

        //Dateien für den Upload vorbereiten
        private void PrepareFile()
        {
            //Verzeichnis im Installationsordner erstellen
            string path = Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Script\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //Falls Datein oder Order in diesem Verzeichnis vorhanden sind sollen diese gelöscht werden
            foreach (string file in Directory.GetFiles(path))
                File.Delete(file);
            foreach (string dir in Directory.GetDirectories(path))
                Directory.Delete(dir, true);
            if (File.Exists(Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip"))
                File.Delete(Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip");

            //Haupt-Scriptdatei mit denen vom Nutzer eingebeneen Informationen ändern
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

            //Jede vom Benutzer ausgewählte Datei in das temporäre Verzeichnis im Installitionsordner kopieren
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if ((bool)(row.Cells[0] as DataGridViewCheckBoxCell).Value && row.Cells[3].Value + "" == "")
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path + row.Cells[2].Value.ToString()));
                    File.Copy(Path.GetDirectoryName(this.path) + "\\" + row.Cells[2].Value.ToString(), path + row.Cells[2].Value.ToString(), true);
                }
            }

            //Eine Zip-Datei aus allen Elementen im temporäre Verzeichnis erstellen
            ZipFile.CreateFromDirectory(path, Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip");

            //Dateien wieder entfernen, sodass nur das Zip-Archiv übrig bleibt
            foreach (string file in Directory.GetFiles(path))
                File.Delete(file);
            foreach (string dir in Directory.GetDirectories(path))
                Directory.Delete(dir, true);

            //Überprüfen der Größe des Zip-Archives (maximal 30MB)
            long size = new FileInfo(Path.GetDirectoryName(Path.GetDirectoryName(path)) + "\\script.zip").Length;
            if (size >= 31457280)
            {
                MetroMessageBox.Show(this, "The file you are trying to upload has a size of " + ((size / 1024f) / 1024f) + "MB.\n But only files up to 30MB are allowed.", "File is too large", MessageBoxButtons.OK, 150);
                throw new Exception();
            }
        }

        //Werte richtig in eine Zeile aus einer Text-Datei einsetzen
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

        //Nicht existierende Tags zur Liste hinzufügen
        private void AddToListNotExists(string[] add)
        {
            foreach (string item in add)
            {
                string[] tags = item.Split(' ');
                foreach (string tag in tags)
                {
                    if (!searchTags.Contains(tag.ToLower()) && tag != "" && searchTags.Count < 49)
                        searchTags.Add(tag.ToLower());
                }
            }
        }

        #endregion
    }
}