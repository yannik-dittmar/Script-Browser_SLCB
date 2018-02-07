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
        public Rating()
        {
            InitializeComponent();
        }

        public void SetRating(int rating)
        {
            if (rating >= 1)
                pictureBox1.Image = Properties.Resources.star;
            if (rating >= 2)
                pictureBox2.Image = Properties.Resources.star;
            if (rating >= 3)
                pictureBox3.Image = Properties.Resources.star;
            if (rating >= 4)
                pictureBox4.Image = Properties.Resources.star;
            if (rating >= 5)
                pictureBox5.Image = Properties.Resources.star;
        }
    }
}
