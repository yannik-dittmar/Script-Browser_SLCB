using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Markdig;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using SaveManager;
using System.Collections.Specialized;
using Script_Browser.TabPages;

namespace Script_Browser.Controls
{
    public partial class ShowScript : UserControl
    {
        bool isDrag = false;
        Control dragControl = null;
        int lastY = 0;

        int id;
        string name;
        Main form;

        public ShowScript(Main _form, string id, string name, string ver, string author, string alias, string shortDesc, string longDesc, string rating, string ratings, string downloads)
        {
            InitializeComponent();

            form = _form;

            this.name = name;
            this.id = Int32.Parse(id);

            label4.Text = name;
            label5.Text = "v" + ver;

            if (alias.Replace(" ", "") == "")
                label3.Text = "by " + author;
            else
                label3.Text = "by " + alias + " (" + author + ")";

            label1.Text = shortDesc;
            webBrowser1.DocumentText = "<html><body>" + Markdown.ToHtml(longDesc).Replace("\n", "<br>") + "</body></html>";
            rating1.SetRating((int)Math.Round(Double.Parse(rating.Replace(".", ","))), id);
            rating1.SetInformation(ratings, downloads);

            Main.sf.currentInstalled.CollectionChanged += listChanged;
            listChanged(null, null);
        }

        //Resize a Control vertically
        private void vResize_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Control c = sender as Control;
                if (e.Y >= (c.ClientRectangle.Bottom - 3) &&
                    e.Y <= (c.ClientRectangle.Bottom + 3))
                {
                    isDrag = true;
                    dragControl = c;
                    lastY = e.Y;
                }
            }
            catch { }
        }

        private void vResize_MouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            if (isDrag && dragControl != null)
            {
                if (dragControl.Height + e.Y - lastY > dragControl.MinimumSize.Height && (sender as Control) == dragControl)
                {
                    dragControl.Height += (e.Y - lastY);
                    lastY = e.Y;
                }
            }
            else if (e.Y >= (c.ClientRectangle.Bottom - 3) &&
                    e.Y <= (c.ClientRectangle.Bottom + 3))
            {
                c.Cursor = Cursors.SizeNS;
            }
            else
                c.Cursor = Cursors.Default;

        }

        private void vResize_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrag && (sender as Control) == dragControl)
            {
                dragControl = null;
                isDrag = false;
            }
        }

        private void vResize_MouseLeave(object sender, EventArgs e)
        {
            (sender as Control).Cursor = Cursors.Default;
        }

        //WebBrowser set Colors & Style
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                webBrowser1.Document.BackColor = Color.FromArgb(18, 25, 31);
                webBrowser1.Document.ForeColor = Color.White;

                webBrowser1.Document.Body.Style = "overflow:auto;";
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

        //Update button if user deletes script outside app
        private void listChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                foreach (KeyValuePair<int, string> item in Main.sf.currentInstalled)
                {
                    if (item.Key == id)
                    {
                        button3.Text = "Uninstall";
                        button3.Tag = item.Value;
                        return;
                    }
                }
                button3.Text = "Download and Install";
            }
            catch { }
        }

        //Install Script
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (button3.Text == "Uninstall")
                {
                    LocalScripts.UninstallScript(form, button3.Tag.ToString(), name);
                    return;
                }
                else
                {
                    button3.Text = "Installing..."; //TODO: Exceptions
                    button3.Refresh();

                    if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Install.zip"))
                        File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Install.zip");

                    if (Networking.DownloadScript(form, id))
                    {
                        try { Directory.Delete(Main.sf.streamlabsPath + @"Services\Scripts\" + id + "\\", true); } catch { }
                        Directory.CreateDirectory(Main.sf.streamlabsPath + @"Services\Scripts\" + id + "\\");

                        ZipFile.ExtractToDirectory(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Install.zip", Main.sf.streamlabsPath + @"Services\Scripts\" + id + "\\");
                        File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Install.zip");

                        foreach (FileInfo file in new DirectoryInfo(Main.sf.streamlabsPath + @"Services\Scripts\" + id + "\\").GetFiles())
                        {
                            if (file.Name.Contains("_StreamlabsSystem.py") || file.Name.Contains("_AnkhBotSystem.py") || file.Name.Contains("_StreamlabsParameter.py") || file.Name.Contains("_AnkhBotParameter.py"))
                            {
                                bool found = false;
                                string[] lines = File.ReadAllLines(file.FullName);
                                using (StreamWriter writer = new StreamWriter(file.FullName))
                                {
                                    for (int i = 0; i < lines.Length; i++)
                                    {
                                        writer.WriteLine(lines[i]);

                                        if (lines[i].ToLower().Contains("version") && !found)
                                        {
                                            writer.WriteLine("ScriptBrowserID = \"" + id + "\"");
                                            found = true;
                                        }
                                    }

                                    if (!found)
                                    {
                                        writer.WriteLine("");
                                        writer.WriteLine("ScriptBrowserID = \"" + id + "\"");
                                    }
                                }
                                break;
                            }
                        }
                        return;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
            try { Directory.Delete(Main.sf.streamlabsPath + @"Services\Scripts\" + id + "\\", true); } catch { }
            button3.Text = "Download and Install";
        }

        //Comment & Bug Report Tab Pages
        private void comment_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(51, 139, 118);
            button2.BackColor = Color.FromArgb(18, 25, 31);
            panelTabPage1.Visible = true;
            panelTabPage2.Visible = false;
        }

        private void bugreport_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(18, 25, 31);
            button2.BackColor = Color.FromArgb(51, 139, 118);
            panelTabPage1.Visible = false;
            panelTabPage2.Visible = true;
        }

        //Comments
        private void ShowScript_Load(object sender, EventArgs e)
        {
            comments1.LoadComments(id, form);
        }
    }
}
