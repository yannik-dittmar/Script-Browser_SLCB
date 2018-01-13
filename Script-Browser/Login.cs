using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser
{
    public partial class Login : Form
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

        public Login()
        {
            InitializeComponent();
            var msm = MaterialSkin.MaterialSkinManager.Instance;
            materialSingleLineTextField1.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void MoveForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        static string Hash(string input)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }

        private void label3_MouseClick(object sender, MouseEventArgs e)
        {
            try { Process.Start("http://digital-programming.de"); } catch { }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (label4.Text == "Login")
            {
                tableLayoutPanel2.RowStyles[1] = new RowStyle(SizeType.AutoSize);
                tableLayoutPanel2.RowStyles[2] = new RowStyle(SizeType.Absolute, 0);

                label4.Text = "Sign up";
                roundedEdgesButton1.Text = "Login";
                label5.Visible = true;
            }
            else
            {
                tableLayoutPanel2.RowStyles[1] = new RowStyle(SizeType.Absolute, 0);
                tableLayoutPanel2.RowStyles[2] = new RowStyle(SizeType.AutoSize);

                label4.Text = "Login";
                roundedEdgesButton1.Text = "Sign up";
                label5.Visible = false;
            }
        }

        private void timerProgressbar_Tick(object sender, EventArgs e)
        {
            if (progressBarEx1.Value < Int32.Parse(progressBarEx1.Tag.ToString()))
                progressBarEx1.Value += 1;
            else if (progressBarEx1.Value > Int32.Parse(progressBarEx1.Tag.ToString()))
                progressBarEx1.Value -= 1;
        }
    }
}
