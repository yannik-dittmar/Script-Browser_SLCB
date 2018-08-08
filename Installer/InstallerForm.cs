using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Script_Browser.Controls;

namespace Installer
{
    public partial class InstallerForm : Form
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

        public InstallerForm()
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        #region Windows API, Window Settings

        private void label2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MoveForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void InstallerForm_Load(object sender, EventArgs e)
        {
            ShapeArrow(noFocusBorderBtn1, 0);
            ShapeArrow(noFocusBorderBtn2, 1);
            ShapeArrow(noFocusBorderBtn3, 1);
            ShapeArrow(noFocusBorderBtn4, 1);
            ShapeArrow(noFocusBorderBtn5, 2);

            textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + "\\SLCB Script-Browser\\";
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

        #region TabControls

        private void noFocusBorderBtnNext_Click(object sender, EventArgs e)
        {
            tcTlp2.NextTab(noFocusBorderBtnBack, noFocusBorderBtnNext);
            UpdateNavbar();
        }

        private void noFocusBorderBtnBack_Click(object sender, EventArgs e)
        {
            tcTlp2.PrevTab(noFocusBorderBtnBack, noFocusBorderBtnNext);
            UpdateNavbar();
        }

        private void Navbar_Click(object sender, EventArgs e)
        {
            tcTlp2.Tab(Int32.Parse((sender as Control).Tag.ToString()), noFocusBorderBtnBack, noFocusBorderBtnNext);
            UpdateNavbar();
        }

        private void UpdateNavbar()
        {
            List<NoFocusBorderBtn> navButtons = new List<NoFocusBorderBtn>
            {
                noFocusBorderBtn1,
                noFocusBorderBtn2,
                noFocusBorderBtn3,
                noFocusBorderBtn4,
                noFocusBorderBtn5
            };

            foreach (NoFocusBorderBtn btn in navButtons)
            {
                if (Int32.Parse(btn.Tag.ToString()) == tcTlp2.currentTab)
                    btn.BackColor = Color.FromArgb(51, 139, 118);
                else
                    btn.BackColor = Color.FromArgb(25, 72, 70);
            }
        }

        private void Check(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                tcTlp2.allowed = 2;
                noFocusBorderBtn3.Enabled = true;
                noFocusBorderBtn4.Enabled = checkBox2.Checked;
                noFocusBorderBtn5.Enabled = checkBox2.Checked;
                if (checkBox2.Checked)
                    tcTlp2.allowed = 4;
            }
            else
            {
                tcTlp2.allowed = 1;
                noFocusBorderBtn3.Enabled = false;
                noFocusBorderBtn4.Enabled = false;
                noFocusBorderBtn5.Enabled = false;
            }

            tcTlp2.Tab(tcTlp2.currentTab, noFocusBorderBtnBack, noFocusBorderBtnNext);
            UpdateNavbar();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (folderBrowserDialog1.SelectedPath.ToCharArray()[folderBrowserDialog1.SelectedPath.ToCharArray().Length - 1] == '\\')
                    textBox1.Text = folderBrowserDialog1.SelectedPath + "SLCB Script-Browser\\";
                else
                    textBox1.Text = folderBrowserDialog1.SelectedPath + "\\SLCB Script-Browser\\";
            }
        }
    }
}
