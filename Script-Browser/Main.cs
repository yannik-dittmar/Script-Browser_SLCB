using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SaveManager;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using AnimatorNS;
using static Script_Browser.Program;
using System.Reflection;
using Script_Browser.Design;
using System.Diagnostics;

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
        private bool hide;

        public Main(JArray topScriptsData, bool hide = false, JObject login = null)
        {
            this.hide = hide;

            //Check SLCB
            if (!Set_SCB_Path.CheckSLCBPath(sf.streamlabsPath))
            {
                Protocol.AddToProtocol("Could not find valid Streamlabs Chatbot path!", Types.Warning);
                if (!Set_SCB_Path.CheckSLCBPath(Set_SCB_Path.GetSLCBPath()))
                    new Set_SCB_Path(sf.streamlabsPath).ShowDialog();
                else
                    sf.streamlabsPath = Set_SCB_Path.GetSLCBPath();
            }
            if (!Set_SCB_Path.CheckSLCBPath(sf.streamlabsPath))
                Environment.Exit(0);
            

            //Check Python
            Console.WriteLine(sf.checkPythonVersion);
            try
            {
                CheckPython.PythonResult python = CheckPython.CheckPythonInstallation();
                if ((python == CheckPython.PythonResult.Nothing || python == CheckPython.PythonResult.Wrong) && sf.checkPythonVersion)
                    new CheckPython().ShowDialog();
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
            sf.Save();

            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            
            selectedTabPage = tableLayoutPanel3;
            navbarTransitionIn.Add(tableLayoutPanel3);
            lastWinSize = Size;
            lastWinPos = Location;

            topScripts1.form = this;
            search1.form = this;
            settings1.form = this;
            localScripts1.form = this;

            topScripts1.LoadList(topScriptsData);

            if (login != null)
                Networking.Login(sf.username, sf.password, this, true, login);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            panelTab1.Visible = false;
            panelTab2.Visible = false;
            panelTab3.Visible = false;

            contextMenuStrip1.Renderer = new ArrowRenderer();

            //Update Scripts
            new Thread(delegate ()
            {
                this.BeginInvoke(new MethodInvoker(delegate () { Protocol.AddToProtocol("Started script update service", Types.Info); }));
                while (!IsDisposed)
                {
                    try
                    {
                        if (Networking.NetworkReady())
                        {
                            for (int i = 0; i < Networking.checkUpdate.Count; i++)
                            {
                                KeyValuePair<string, string> script = Networking.checkUpdate[i];
                                try
                                {
                                    this.BeginInvoke(new MethodInvoker(delegate () { Protocol.AddToProtocol("Checking for update: " + script.Value, Types.Info); }));
                                    string version = "";
                                    string[] lines = File.ReadAllLines(script.Value);
                                    foreach (string line in lines)
                                    {
                                        if (line.ToLower().Contains("version"))
                                            version = GetLineItem(line);
                                    }

                                    string result = Networking.CheckForUpdate(script.Key, version);
                                    if (result != "no" && result != "")
                                    {
                                        JObject updateInfo = JObject.Parse(result);
                                        this.BeginInvoke(new MethodInvoker(delegate () { Protocol.AddToProtocol("Updating: " + script.Value, Types.Info); }));

                                        if (Networking.DownloadScript(null, Int32.Parse(script.Key)))
                                        {
                                            string path = Path.GetDirectoryName(script.Value) + "";

                                            JObject fileChanges = JObject.Parse(updateInfo["FileChanges"].ToString());
                                            //Delete
                                            foreach (JToken delete in fileChanges["Delete"] as JArray)
                                                try { File.Delete(path + delete); } catch { }

                                            //Move
                                            foreach (JObject move in fileChanges["Move"])
                                                try { File.Move(path + move["From"], path + move["To"]); } catch { }

                                            //Copy
                                            foreach (JObject copy in fileChanges["Copy"])
                                                try { File.Move(path + copy["From"], path + copy["To"]); } catch { }

                                            try { Directory.Delete(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Update\", true); } catch { }
                                            Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Update\");

                                            if (!Directory.Exists(sf.streamlabsPath + @"Services\Scripts\" + script.Key + "\\"))
                                                Directory.CreateDirectory(sf.streamlabsPath + @"Services\Scripts\" + script.Key + "\\");

                                            ZipFile.ExtractToDirectory(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Install.zip", Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Update\");

                                            //Copy files
                                            foreach (string dirPath in Directory.GetDirectories(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Update\", "*", SearchOption.AllDirectories))
                                                Directory.CreateDirectory(dirPath.Replace(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Update\", sf.streamlabsPath + @"Services\Scripts\" + script.Key + "\\"));

                                            foreach (string newPath in Directory.GetFiles(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Update\", "*.*", SearchOption.AllDirectories))
                                            {
                                                try
                                                {
                                                    File.Copy(newPath, newPath.Replace(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Update\", sf.streamlabsPath + @"Services\Scripts\" + script.Key + "\\"), true);
                                                }
                                                catch { }
                                            }

                                            //Delete temps
                                            File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Install.zip");
                                            Directory.Delete(Path.GetDirectoryName(Application.ExecutablePath) + @"\tmp\Update\", true);

                                            //Set ID
                                            foreach (FileInfo file in new DirectoryInfo(sf.streamlabsPath + @"Services\Scripts\" + script.Key + "\\").GetFiles())
                                            {
                                                if (file.Name.Contains("_StreamlabsSystem.py") || file.Name.Contains("_AnkhBotSystem.py") || file.Name.Contains("_StreamlabsParameter.py") || file.Name.Contains("_AnkhBotParameter.py"))
                                                {
                                                    bool found = false;
                                                    lines = File.ReadAllLines(file.FullName);

                                                    foreach (string line in lines)
                                                    {
                                                        if (line.Contains("ScriptBrowserID = "))
                                                        {
                                                            found = true;
                                                            break;
                                                        }
                                                    }

                                                    if (!found)
                                                    {
                                                        using (StreamWriter writer = new StreamWriter(file.FullName))
                                                        {
                                                            for (int ii = 0; ii < lines.Length; ii++)
                                                            {
                                                                writer.WriteLine(lines[ii]);

                                                                if (lines[ii].ToLower().Contains("version") && !found)
                                                                {
                                                                    writer.WriteLine("ScriptBrowserID = \"" + script.Key + "\"");
                                                                    found = true;
                                                                }
                                                            }

                                                            if (!found)
                                                            {
                                                                writer.WriteLine("");
                                                                writer.WriteLine("ScriptBrowserID = \"" + script.Key + "\"");
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            }

                                            IAsyncResult wait = BeginInvoke(new MethodInvoker(delegate ()
                                            {
                                                Protocol.AddToProtocol("Successfully updated: " + script.Value, Types.Info);
                                                notifyIcon1.Tag = updateInfo["UpdateMessage"];
                                                notifyIcon1.ShowBalloonTip(2000, "Updated Script", updateInfo["Name"].ToString(), ToolTipIcon.Info);
                                                notifyIcon1.BalloonTipText = updateInfo["Name"].ToString();
                                            }));
                                            while (!wait.IsCompleted)
                                                Thread.Sleep(500);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    this.BeginInvoke(new MethodInvoker(delegate () { Protocol.AddToProtocol("Could not update: " + script.Value + "\n" + ex.StackTrace, Types.Error); }));
                                    Console.WriteLine(ex.StackTrace);
                                }

                                IAsyncResult wait2 = BeginInvoke(new MethodInvoker(delegate () { Networking.checkUpdate.RemoveAt(i); }));
                                while (!wait2.IsCompleted)
                                    Thread.Sleep(1000);
                                i--;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.BeginInvoke(new MethodInvoker(delegate () { Protocol.AddToProtocol("The update service has run into an error: " + ex.StackTrace, Types.Error); }));
                    }

                    Thread.Sleep(300000);
                }
            }).Start();
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
            return "UNDEF";
        }

        #region Windows API, Window Settings

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

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                if (sizeGripRectangle.Contains(hitPoint) && Size != Screen.GetWorkingArea(new Point(Cursor.Position.X, Cursor.Position.Y)).Size)
                    m.Result = new IntPtr(HTBOTTOMRIGHT);
            }
            else if (m.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                Show();
                base.WndProc(ref m);
            }
            else
                base.WndProc(ref m);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (Opacity == 0 && WindowState == FormWindowState.Minimized)
                maximize.Enabled = true;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height));
            sizeGripRectangle = new Rectangle(ClientRectangle.Width - tolerance, ClientRectangle.Height - tolerance, tolerance, tolerance);
            region.Exclude(sizeGripRectangle);
            panelMain.Region = region;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Size != Screen.GetWorkingArea(new Point(Cursor.Position.X, Cursor.Position.Y)).Size)
                ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }

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

        //Minimize App
        private void label2_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void minimized_Tick(object sender, EventArgs e)
        {
            Opacity = (Opacity - ((Double)7 / 100));
            if (Opacity == 0)
            {
                WindowState = FormWindowState.Minimized;
                minimized.Enabled = false;
            }
        }

        private void maximize_Tick(object sender, EventArgs e)
        {
            Opacity = (Opacity + ((Double)7 / 100));
            if (Opacity == 1)
                maximize.Enabled = false;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            minimized.Enabled = true;
            maximize.Enabled = false;
        }

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

        // Navbar
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

        private void tableLayoutPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int index = Int32.Parse(((Control)sender).Tag.ToString());

            if (index > selectedTabPageIndex)
            {
                Control newControl = ControlByTag(panel2, index + "");

                newControl.Size = panel2.Size;
                animatorTabPage.Show(newControl);
                

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
                animatorTabPage.Hide(oldControl);

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

        //Show Update Message
        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            try
            {
                this.Opacity = 0.5;
                new UpdateMessage(notifyIcon1.BalloonTipText, notifyIcon1.Tag.ToString()).ShowDialog();
                this.BringToFront();
            }
            catch { }
            this.Opacity = 1;
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            if (hide)
                Hide();
        }

        #region Notificon

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
            Show();
            this.Focus();
            this.Activate();
            this.BringToFront();
        }

        private void notifyIcon1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon1, null);
            }
        }

        private void tESTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
            Show();
            this.Focus();
            this.Activate();
            this.BringToFront();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sf.Save();
            try { notifyIcon1.Dispose(); } catch { }
            Environment.Exit(0);
        }

        #endregion
    }
}
