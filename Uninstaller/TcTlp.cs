using Script_Browser.Controls;
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
        public int currentTab = 0;
        public int allowed = 1;

        public void Tab(int tab, NoFocusBorderBtn back = null, NoFocusBorderBtn next = null)
        {
            if (tab < this.ColumnCount && tab >= 0)
            {
                foreach (ColumnStyle cs in this.ColumnStyles)
                    cs.Width = 0;
                this.ColumnStyles[tab].Width = 100;

                foreach (Control c in this.Controls)
                    c.Visible = false;
                this.Controls[tab].Visible = true;

                if (back != null && next != null)
                {
                    if (tab == 4)
                        next.Text = "Install";
                    else
                        next.Text = "Next";
                    next.Enabled = allowed != tab || tab == 4;
                    back.Enabled = tab != 0;
                }

                currentTab = tab;
            }
        }

        public void NextTab(NoFocusBorderBtn back, NoFocusBorderBtn next)
        {
            Tab(currentTab + 1, back, next);
        }

        public void PrevTab(NoFocusBorderBtn back, NoFocusBorderBtn next)
        {
            Tab(currentTab - 1, back, next);
        }
    }
}
