using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Script_Browser.Controls
{
    public partial class SlideShow : UserControl
    {
        public SlideShow()
        {
            InitializeComponent();
        }

        private void DownloadImages(string[] paths)
        {

        }

        private void SmallImageResize(object sender, EventArgs e)
        {
            try
            {
                PictureBox pb = (PictureBox)sender;
                pb.Width = (int)(((double)pb.Image.Height / (double)pb.Image.Width) * (double)pb.Height);
            }
            catch { }
        }
    }
}
