using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser.Controls
{
    public partial class Rating : UserControl
    {
        int rating = 0;

        public Rating()
        {
            InitializeComponent();
        }

        public void SetRating(int rating)
        {
            try
            {
                if (rating >= 1)
                    pictureBox1.Image = Properties.Resources.star;
                else
                    pictureBox1.Image = null;
                if (rating >= 2)
                    pictureBox2.Image = Properties.Resources.star;
                else
                    pictureBox2.Image = null;
                if (rating >= 3)
                    pictureBox3.Image = Properties.Resources.star;
                else
                    pictureBox3.Image = null;
                if (rating >= 4)
                    pictureBox4.Image = Properties.Resources.star;
                else
                    pictureBox4.Image = null;
                if (rating >= 5)
                    pictureBox5.Image = Properties.Resources.star;
                else
                    pictureBox5.Image = null;

                this.rating = rating;
            }
            catch { }
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                int rating = Int32.Parse((sender as Control).Tag.ToString());
                if (rating >= 1)
                    pictureBox1.Image = Properties.Resources.star;
                if (rating >= 2)
                    pictureBox2.Image = Properties.Resources.star;
                else
                    pictureBox2.Image = null;
                if (rating >= 3)
                    pictureBox3.Image = Properties.Resources.star;
                else
                    pictureBox3.Image = null;
                if (rating >= 4)
                    pictureBox4.Image = Properties.Resources.star;
                else
                    pictureBox4.Image = null;
                if (rating >= 5)
                    pictureBox5.Image = Properties.Resources.star;
                else
                    pictureBox5.Image = null;
            }
            catch { }
        }

        private void tableLayoutPanel1_MouseLeave(object sender, EventArgs e)
        {
            SetRating(rating);
        }
    }
}
