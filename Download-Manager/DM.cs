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

        //Beenden der Anwendung bei Klicken auf "X"
        private void labelX_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Minimieren des Fensters bei Klicken auf "_"
        private void labelMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //Bewegen des Fensters 
        //wenn die Maustaste in diesem Bereich gedrückt wird, Speichern der alten Position; Varible für Taste gedrückt true
        private void tableLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }
        //Berchnen der neuen Position wenn Maus bewegt wird
        private void tableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                //updaten des Fensters
                this.Update();
            }
        }
        //Bewegen des Festers beenden, wenn die Maustaste nicht mehr gedrückt wird, indem die Variable auf false gesetzt wird
        private void tableLayoutPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        //Knopf "next" aktivieren, wenn der Nutzer im der Textbox bis zum Ende gescrollt hat
        private void richTextBoxTerms_VScroll(object sender, EventArgs e)
        {
            if (richTextBoxTerms.ReachedBottom())
            {
                checkBoxAgree.Enabled = true;
            }
        }

        //checkbox "I agree." -> Aktivieren des Knopfes "next"
        private void checkBoxAgree_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAgree.Checked == true)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        //Knopf "cancel" -> Dialog
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //Beenden der Anwendung
                Application.Exit();
            }
        }

        //Knopf "next" -> Anzeigen des Installationsbereichs
        private void button1_Click(object sender, EventArgs e)
        {
            //Nutzungsbedingungen entfernen
            panel1.Visible = false;
            //neue Installationsseite
            Pages.Installation x = new Pages.Installation();
            this.Controls.Add(x);
            x.Size = new Size(645, 370);
            x.Location = new Point(0,52);
        }

    }
}
