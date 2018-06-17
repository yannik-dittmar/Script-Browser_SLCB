using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SplashScreen
{
    public partial class Main : Form
    {
        private Stopwatch sw = new Stopwatch();

        public Main()
        {
            InitializeComponent();
        }

        private float ParametricBlend(float t)
        {
            if (t > 1)
                return 1;
            float sqt = (float)Math.Pow(t, 2);
            return sqt / (2.0f * (sqt - t) + 1.0f);
        }

        private void timerSmall_Tick(object sender, EventArgs e)
        {
            panel1.Padding = new Padding(20 + (int)(ParametricBlend(sw.ElapsedMilliseconds / 2000.0f) * 40));
            if (panel1.Padding.All >= 60)
            {
                sw.Restart();
                timerSmall.Enabled = false;
                timerBig.Enabled = true;
            }
        }

        private void timerBig_Tick(object sender, EventArgs e)
        {
            panel1.Padding = new Padding(60 - (int)(ParametricBlend(sw.ElapsedMilliseconds / 2000.0f) * 40));
            if (panel1.Padding.All <= 20)
            {
                sw.Restart();
                timerSmall.Enabled = true;
                timerBig.Enabled = false;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            sw.Start();
        }
    }
}
