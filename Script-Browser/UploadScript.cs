using Markdig;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        public UploadScript()
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            materialSingleLineTextField1.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField2.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField3.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField4.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;

            CheckScriptInformation(null, null);
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

        private void SetPage(int page)
        {
            if (page <= currentStep && page > 0)
            {
                for (int i = 0; i < tableLayoutPanel4.Controls.Count; i++)
                {
                    tableLayoutPanel4.Controls[i].BackColor = Color.FromArgb(25, 72, 70);

                    if (i == page - 1)
                        tableLayoutPanel4.Controls[i].BackColor = Color.FromArgb(51, 139, 118);
                }

                for (int i = 0; i < tableLayoutTabControl.ColumnStyles.Count; i++)
                {
                    try
                    {
                        if (i == page - 1)
                        {
                            tableLayoutTabControl.ColumnStyles[i].SizeType = SizeType.Percent;
                            tableLayoutTabControl.ColumnStyles[i].Width = 100f;
                            if (tableLayoutTabControl.Controls[i].GetType().ToString() != "System.Windows.Forms.FlowLayoutPanel")
                                tableLayoutTabControl.Controls[i].Visible = true;
                        }
                        else
                        {
                            tableLayoutTabControl.ColumnStyles[i].SizeType = SizeType.Absolute;
                            tableLayoutTabControl.ColumnStyles[i].Width = 0;
                            if (tableLayoutTabControl.Controls[i].GetType().ToString() != "System.Windows.Forms.FlowLayoutPanel")
                                tableLayoutTabControl.Controls[i].Visible = false;
                        }
                    }
                    catch { }
                }

                noFocusBorderBtn6.Enabled = page != currentStep;
                noFocusBorderBtn7.Enabled = page != 1;
                currentPage = page;
            }
        }

        private void EnableStep(int step)
        {
            for (int i = 0; i < tableLayoutPanel4.Controls.Count; i++)
                tableLayoutPanel4.Controls[i].Enabled = i < step;
            currentStep = step;
        }

        private void noFocusBorderBtn1_MouseClick(object sender, MouseEventArgs e)
        {
            try { SetPage(Int32.Parse((sender as Control).Tag.ToString())); } catch { }
        }

        private void nextPage_Click(object sender, EventArgs e)
        {
            SetPage(currentPage + 1);
        }

        private void previousPage_Click(object sender, EventArgs e)
        {
            SetPage(currentPage - 1);
        }

        //
        // Tab Script Information
        //

        private void CheckScriptInformation(object sender, EventArgs e)
        {
            List<MaterialSingleLineTextField> textfields = new List<MaterialSingleLineTextField> { materialSingleLineTextField1, materialSingleLineTextField2, materialSingleLineTextField3, materialSingleLineTextField4 };

            foreach (MaterialSingleLineTextField textfield in textfields)
            {
                if (textfield.Text.Trim(' ').Length == 0 && textfield.Tag.ToString() != "empty")
                {
                    EnableStep(1);
                    break;
                }
                else if (currentStep == 1)
                    EnableStep(2);
            }
            SetPage(1);
        }

        //
        // Tab Description
        //

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
            }
            catch { }
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
    }
}
