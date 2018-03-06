using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser
{
    public partial class FileChangesForUpdate : Form
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

        Types type;
        JObject output;

        public FileChangesForUpdate(Types _type, JObject _output)
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            materialSingleLineTextField2.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSingleLineTextField1.SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;

            type = _type;
            output = _output; 

            if (type == Types.Delete)
            {
                label5.Visible = false;
                materialSingleLineTextField2.Visible = false;
                label4.Text = "Path for file to delete:";
                materialSingleLineTextField1.Hint = "test.txt";
            }
            else if (type == Types.MoveOrCopy)
            {
                label4.Text = "Path for source file:";
                label5.Text = "Path for destination file:";
                materialSingleLineTextField1.Hint = "test.txt";
                materialSingleLineTextField2.Hint = "test2.txt";
            }
        }

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

        public enum Types
        {
            Delete,
            MoveOrCopy
        }

        private void noFocusBorderBtn6_Click(object sender, EventArgs e)
        {
            if (type == Types.Delete && materialSingleLineTextField1.Text.Trim(' ') != "")
                output["Value"] = materialSingleLineTextField1.Text;
            else if (type != Types.Delete && materialSingleLineTextField1.Text.Trim(' ') != "" && materialSingleLineTextField2.Text.Trim(' ') != "" && materialSingleLineTextField1.Text != materialSingleLineTextField2.Text)
            {
                output = new JObject
                {
                    ["From"] = materialSingleLineTextField1.Text,
                    ["To"] = materialSingleLineTextField2.Text
                };
            }
            this.Dispose();
        }
    }
}
