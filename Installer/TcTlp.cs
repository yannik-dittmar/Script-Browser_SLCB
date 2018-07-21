using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Installer
{
    class TcTlp : TableLayoutPanel
    {
        public void Tab(int tab)
        {
            if (tab < this.ColumnCount)
            {
                foreach (ColumnStyle cs in this.ColumnStyles)
                    cs.Width = 0;
                this.ColumnStyles[tab].Width = 100;
            }
        }
    }
}
