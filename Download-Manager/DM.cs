using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Download_Manager
{
    public partial class DM : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        public DM()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //Scließen des Fensters bei klicken aus "X"
        private void labelX_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        //Minimieren des Fensters bei klicken auf "_"
        private void labelMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //Bewegen des Fensters 
        //Speichern der ursprünglichen Position bei Mausklick
        private void tableLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }
        //Berechnen der neuen Position während Maus bewegt wird
        private void tableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }
        //Beenden des Bewegens, wenn Taste nicht mehr gedrückt wird
        private void tableLayoutPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
