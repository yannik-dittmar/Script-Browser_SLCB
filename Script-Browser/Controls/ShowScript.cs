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

        public ShowScript(Main _form, string id, string name, string ver, string author, string shortDesc, string longDesc, string rating, string ratings, string downloads)
        {
            InitializeComponent();

            form = _form;

            this.name = name;
            this.id = Int32.Parse(id);

            label4.Text = name;
            label5.Text = "v" + ver;
            label3.Text = "by " + author;
            label1.Text = shortDesc;
            webBrowser1.DocumentText = "<html><body>" + Markdown.ToHtml(longDesc) + "</body></html>";
            rating1.SetRating((int)Math.Round(Double.Parse(rating.Replace(".", ","))));
            rating1.SetInformation(ratings, downloads);
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

        //Install Script
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                button3.Text = "Installing...";
                button3.Refresh();

                if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Install.zip"))
                    File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Install.zip");

                if (Networking.DownloadScript(form, id))
                {
                    try { Directory.Delete(Main.sf.streamlabsPath + @"Services\Scripts\" + name + "\\", true); } catch { }
                    Directory.CreateDirectory(Main.sf.streamlabsPath + @"Services\Scripts\" + name + "\\");

                    ZipFile.ExtractToDirectory(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Install.zip", Main.sf.streamlabsPath +  @"Services\Scripts\" + name + "\\");
                    File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Install.zip");

                    button3.Text = "Uninstall";
                    return;
                }
            }
            catch { }
            button3.Text = "Download and Install";
        }
    }
}
