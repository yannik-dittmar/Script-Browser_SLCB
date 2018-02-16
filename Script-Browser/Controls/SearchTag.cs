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
    public partial class SearchTag : UserControl
    {
        public SearchTag(string name)
        {
            InitializeComponent();
            label1.Text = name;
        }

        private void noFocusBorderBtn1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
