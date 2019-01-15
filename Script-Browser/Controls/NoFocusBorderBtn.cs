using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser.Controls
{
    public class NoFocusBorderBtn : Button
    {
        public Color NotEnabledBG = Color.FromArgb(22, 36, 45);

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!Enabled)
            {
                using (SolidBrush fore = new SolidBrush(ForeColor))
                using (StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                })
                {
                    e.Graphics.Clear(NotEnabledBG);
                    e.Graphics.DrawString(Text, Font, fore, new Rectangle(0,0, this.Width, this.Height), sf);
                }
            }
        }
    }
}
