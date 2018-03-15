using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MaterialSkin;
using MaterialSkin.Controls;
using MaterialSkin.Animations;
using System.Runtime.InteropServices;
using SaveManager;
using System.IO;

namespace Script_Browser
{
    public partial class Main : Form
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
        int tolerance = 20;
        const int WM_NCHITTEST = 132;
        const int HTBOTTOMRIGHT = 17;
        Rectangle sizeGripRectangle;

        #endregion

        List<TableLayoutPanel> navbarTransitionIn = new List<TableLayoutPanel>();
        List<TableLayoutPanel> navbarTransitionOut = new List<TableLayoutPanel>();
        TableLayoutPanel selectedTabPage = null;
        int selectedTabPageIndex = 0;

        Size lastWinSize;
        Point lastWinPos;

        public static SaveFile sf = new SaveFile(Path.GetDirectoryName(Application.ExecutablePath) + @"\settings.save");

        public Main()
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20)); //Fensterecken abrunden
            
            //Fenster Standardwerte setzen 
            selectedTabPage = tableLayoutPanel3;
            navbarTransitionIn.Add(tableLayoutPanel3);
            lastWinSize = Size;
            lastWinPos = Location;

            //Tabseiten das Fenster übergeben
            topScripts1.form = this;
            search1.form = this;
            settings1.form = this;
            localScripts1.form = this;
        }

        //Erste Tabseite aufrufen und Scripts laden
        private void Main_Load(object sender, EventArgs e)
        {
            panelTab1.Visible = false;
            panelTab2.Visible = false;
            panelTab3.Visible = false;
            topScripts1.button3_Click(null, null);
        }

        #region Windows API, Window Settings

        //Fenster minimierbar machen
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;
                var cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                return cp;
            }
        }

        //Größe des Fensters verstellen
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint) && Size != Screen.GetWorkingArea(new Point(Cursor.Position.X, Cursor.Position.Y)).Size)
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        //Fenster wieder herstellen nachdem es minimiert wurde
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (Opacity == 0 && WindowState == FormWindowState.Minimized)
                maximize.Enabled = true;
        }

        //Nach Größenänderung Ecken und Bereiche anpassen
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height));
            sizeGripRectangle = new Rectangle(ClientRectangle.Width - tolerance, ClientRectangle.Height - tolerance, tolerance, tolerance);
            region.Exclude(sizeGripRectangle);
            panelMain.Region = region;
            Invalidate();
        }

        //HTBOTTOMRIGHT zeichnen (unten rechts)
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Size != Screen.GetWorkingArea(new Point(Cursor.Position.X, Cursor.Position.Y)).Size)
                ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }

        //Fenster über den Bildschirm bewegen
        private void MoveForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Size == Screen.GetWorkingArea(new Point(Cursor.Position.X, Cursor.Position.Y)).Size)
                {
                    Size = lastWinSize;
                    Left = Cursor.Position.X - Width / 2;
                }

                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);

                if (Cursor.Position.Y <= Screen.GetWorkingArea(new Point(Cursor.Position.X, Cursor.Position.Y)).Y + 20)
                    label3_Click(null, null);
                else
                    lastWinPos = Location;
            }
        }

        //Close App
        private void label2_Click(object sender, EventArgs e)
        {
            sf.Save();
            try { notifyIcon1.Dispose(); } catch { }
            Environment.Exit(0);
        }

        //Fenster langsam transparent machen um es dann zu minimieren
        private void minimized_Tick(object sender, EventArgs e)
        {
            Opacity = (Opacity - ((Double)7 / 100));
            if (Opacity == 0)
            {
                WindowState = FormWindowState.Minimized;
                minimized.Enabled = false;
            }
        }

        //Fenster langsam nicht transparent machen um es dann zu zeigen
        private void maximize_Tick(object sender, EventArgs e)
        {
            Opacity = (Opacity + ((Double)7 / 100));
            if (Opacity == 1)
                maximize.Enabled = false;
        }

        //Fenster auf Knopfdruck minimieren
        private void label4_Click(object sender, EventArgs e)
        {
            minimized.Enabled = true;
            maximize.Enabled = false;
        }

        //Fenster maximieren oder wieder normale Größe herstellen
        private void label3_Click(object sender, EventArgs e)
        {
            Screen screen = Screen.FromControl(this);
            if (Size == screen.WorkingArea.Size && Location == new Point(screen.Bounds.Left, screen.Bounds.Top))
            {
                Size = lastWinSize;
                Location = lastWinPos;
            }
            else
            {
                lastWinSize = Size;
                if (sender != null)
                    lastWinPos = Location;
                Size = Screen.GetWorkingArea(new Point(Cursor.Position.X, Cursor.Position.Y)).Size;
                Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
                Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width + 1, Height + 1, 0, 0));
            }
        }

        //Fensterecken richtig anpasse, wenn im maximierten Modus
        private void Main_Resize(object sender, EventArgs e)
        {
            Screen screen = Screen.FromControl(this);
            if (Size == screen.WorkingArea.Size && Location == new Point(screen.Bounds.Left, screen.Bounds.Top))
                Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width + 1, Height + 1, 0, 0));
            else
                Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        // Save settings on dispose
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            sf.Save();
        }

        #endregion

        #region  Animations

        //Farbe der Elemente der unteren Navigationsleiste ändern
        //Nutzer hovert über diesem Element
        private void NavTransitionIn_Tick(object sender, EventArgs e)
        {
            for (int i=0; i < navbarTransitionIn.Count; i++)
            {
                TableLayoutPanel tlp = navbarTransitionIn[i];
                Color c = tlp.BackColor;
                int r = c.R + 2;
                if (r > 25)
                    r = 25;
                int g = c.G + 2;
                if (g > 72)
                    g = 72;
                int b = c.B + 2;
                if (b > 70)
                    b = 70;
                tlp.BackColor = Color.FromArgb(r, g, b);
                if (r == 25 && g == 72 && b == 70)
                    navbarTransitionIn.Remove(tlp);
            }
            Color c2 = selectedTabPage.BackColor;
            if ((c2.R != 25 || c2.G != 72 || c2.B != 70) && !navbarTransitionIn.Contains(selectedTabPage))
                navbarTransitionIn.Add(selectedTabPage);
        }

        //Nutzer ist nicht mehr auf dem Element -> stanard Hintergrundfarbe wiederherstellen
        private void NavTransitionOut_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < navbarTransitionOut.Count; i++)
            {
                TableLayoutPanel tlp = navbarTransitionOut[i];
                Color c = tlp.BackColor;
                int r = c.R - 2;
                if (r < 18)
                    r = 18;
                int g = c.G - 2;
                if (g < 25)
                    g = 25;
                int b = c.B - 2;
                if (b < 31)
                    b = 31;
                tlp.BackColor = Color.FromArgb(r, g, b);
                if (r == 18 && g == 25 && b == 31)
                    navbarTransitionOut.Remove(tlp);
            }

            NavTransitionOut.Enabled = navbarTransitionOut.Count != 0;
        }

        //Mauzeiger betritt ein Auswahl-Element
        private void tableLayoutPanel_MouseEnter(object sender, EventArgs e)
        {
            if (sender.GetType().ToString() != "System.Windows.Forms.TableLayoutPanel")
            {
                if (!navbarTransitionIn.Contains((TableLayoutPanel)((Control)sender).Parent))
                    navbarTransitionIn.Add((TableLayoutPanel)((Control)sender).Parent);
                navbarTransitionOut.Remove((TableLayoutPanel)((Control)sender).Parent);
            }
            else
            {
                if (!navbarTransitionIn.Contains(sender as TableLayoutPanel))
                    navbarTransitionIn.Add(sender as TableLayoutPanel);
                navbarTransitionOut.Remove(sender as TableLayoutPanel);
            }
            
            NavTransitionIn.Enabled = true;
        }

        //Mauszeiger verlässt ein Auswahl-Element
        private void tableLayoutPanel_MouseLeave(object sender, EventArgs e)
        {
            if (selectedTabPage != sender)
            {
                if (sender.GetType().ToString() != "System.Windows.Forms.TableLayoutPanel")
                {
                    if (!navbarTransitionOut.Contains((TableLayoutPanel)((Control)sender).Parent))
                        navbarTransitionOut.Add((TableLayoutPanel)((Control)sender).Parent);
                    navbarTransitionIn.Remove((TableLayoutPanel)((Control)sender).Parent);
                }
                else
                {
                    if (!navbarTransitionOut.Contains(sender as TableLayoutPanel))
                        navbarTransitionOut.Add(sender as TableLayoutPanel);
                    navbarTransitionIn.Remove(sender as TableLayoutPanel);
                }

                NavTransitionOut.Enabled = true;
            }
        }

        //Nutzer hat auf eine Auswahl in der Navigationsleiste geklickt
        private void tableLayoutPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int index = Int32.Parse(((Control)sender).Tag.ToString()); //Herausfinden, welche Tabseite aufgerufen werden soll

            if (index > selectedTabPageIndex)
            {
                Control newControl = ControlByTag(panel2, index + "");

                newControl.Size = panel2.Size;
                animatorTabPage.Show(newControl); //Neue Seite überdeck die Andere mit einer Animation
                
                //Navbar Item Hintergrundfarbe auf dauerhaft setzen
                foreach (Control c in tableLayoutPanel2.Controls)
                {
                    if (c.Tag.ToString() == index + "")
                    {
                        if (!navbarTransitionOut.Contains(selectedTabPage))
                            navbarTransitionOut.Add(selectedTabPage);
                        navbarTransitionIn.Remove(selectedTabPage);
                        NavTransitionOut.Enabled = true;

                        selectedTabPage = c as TableLayoutPanel;
                        break;
                    }
                }

                selectedTabPageIndex = index;
            }
            else if (index < selectedTabPageIndex)
            {
                foreach (Control c in panel2.Controls)
                {
                    try
                    {
                        if (c.Tag.ToString() != index + "" && c.Tag.ToString() != selectedTabPageIndex + "")
                            c.Visible = false;
                    }
                    catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
                }
                Control oldControl = ControlByTag(panel2, selectedTabPageIndex + "");
                Control newControl = ControlByTag(panel2, index + "");
                newControl.Visible = true;

                oldControl.Size = panel2.Size;
                animatorTabPage.Hide(oldControl); //Alte Seite verschwindet und neue Seite kommt zum vorschein

                //Navbar Item Hintergrundfarbe auf dauerhaft setzen
                foreach (Control c in tableLayoutPanel2.Controls)
                {
                    if (c.Tag.ToString() == index + "")
                    {
                        if (!navbarTransitionOut.Contains(selectedTabPage))
                            navbarTransitionOut.Add(selectedTabPage);
                        navbarTransitionIn.Remove(selectedTabPage);
                        NavTransitionOut.Enabled = true;

                        selectedTabPage = c as TableLayoutPanel;
                        break;
                    }
                }

                selectedTabPageIndex = index;
            }
        }

        //Ein Element nach seinem Tag erfassen, nur in einem bestimmten 
        private Control ControlByTag(Control parent, string tag)
        {
            foreach (Control c in parent.Controls)
            {
                try
                {
                    if (c.Tag.ToString() == tag)
                        return c;
                }
                catch { }
            }
            return null;
        }

        #endregion
    }
}
