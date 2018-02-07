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

namespace Script_Browser.Controls
{
    public partial class ShowScript : UserControl
    {
        bool isDrag = false;
        int lastY = 0;

        public ShowScript()
        {
            InitializeComponent();
            webBrowser1.DocumentText = "<html><body>" + Markdown.ToHtml(System.IO.File.ReadAllText(@"C:\Users\Yannik\Desktop\Twitch\test.txt")) + "</body></html>";
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y >= (panel1.ClientRectangle.Bottom - 5) &&
                e.Y <= (panel1.ClientRectangle.Bottom + 5))
            {
                isDrag = true;
                lastY = e.Y;
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrag)
            {
                if (panel1.Height + e.Y - lastY > panel1.MinimumSize.Height)
                {
                    panel1.Height += (e.Y - lastY);
                    lastY = e.Y;
                }
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrag)
            {
                isDrag = false;
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.BackColor = Color.FromArgb(18, 25, 31);
            webBrowser1.Document.ForeColor = Color.White;
        }

        public void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.ToString() != "about:blank")
            {
                try { Process.Start(e.Url.ToString()); } catch { }
                e.Cancel = true;
            }
        }
    }
}
