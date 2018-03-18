using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Script_Browser.Controls
{
    public partial class Rating : UserControl
    {
        double rating = 0;
        string id = "";
        public Main form;
        string downloads = "";

        public Rating()
        {
            InitializeComponent();
        }

        public void SetRating(double rating, string _id)
        {
            id = _id;
            try
            {
                if (Main.sf.ratedScripts.Contains(id))
                {
                    pictureBox1.Cursor = Cursors.No;
                    pictureBox2.Cursor = Cursors.No;
                    pictureBox3.Cursor = Cursors.No;
                    pictureBox4.Cursor = Cursors.No;
                    pictureBox5.Cursor = Cursors.No;
                }

                DrawStar(pictureBox1, rating, 1);
                DrawStar(pictureBox2, rating, 2);
                DrawStar(pictureBox3, rating, 3);
                DrawStar(pictureBox4, rating, 4);
                DrawStar(pictureBox5, rating, 5);

                this.rating = rating;
            }
            catch { }
        }

        private void DrawStar(PictureBox pb, double rating, int index)
        {
            if (pb.Image != null)
                pb.Image.Dispose();

            if (rating >= index)
                pb.Image = Properties.Resources.star;
            else
            {
                if (rating <= index - 1)
                    pb.Image = null;
                else
                {
                    Bitmap bmp = (Bitmap)Properties.Resources.star.Clone();
                    double split = rating - (int)rating;

                    int x = (int)(bmp.Width * split);
                    for (; x < bmp.Width; x++)
                    {
                        for (int y = 0; y < bmp.Height; y++)
                            bmp.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                    }

                    pb.Image = bmp;
                }
            }
        }

        public void SetInformation(string ratings, string downloads)
        {
            this.downloads = downloads;
            label1.Text = ratings + " Ratings and " + downloads + " Downloads";
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                if ((sender as Control).Cursor != Cursors.No)
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
            }
            catch { }
        }

        private void tableLayoutPanel1_MouseLeave(object sender, EventArgs e)
        {
            SetRating(rating, id);
        }

        //Rate
        private void RateScript_Click(object sender, EventArgs e)
        {
            try
            {
                if ((sender as Control).Cursor != Cursors.No)
                {
                    string result = Networking.RateScript(form, id, (sender as Control).Tag.ToString());
                    if (!result.Contains("false"))
                    {
                        JObject newRating = JObject.Parse(result);

                        Main.sf.ratedScripts.Add(id);
                        SetRating(Double.Parse(newRating["Rating"].ToString().Replace(".", ",")), id);
                        SetInformation(newRating["Ratings"].ToString(), downloads);
                    }
                }
            }
            catch { }
        }
    }
}
