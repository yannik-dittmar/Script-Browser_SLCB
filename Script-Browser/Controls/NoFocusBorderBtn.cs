using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser.Controls
{
    class NoFocusBorderBtn : Button
    {
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
                using (SolidBrush back = new SolidBrush(Color.FromArgb(22, 36, 45)))
                using (SolidBrush fore = new SolidBrush(ForeColor))
                using (StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                })
                {
                    e.Graphics.FillRectangle(back, e.ClipRectangle);
                    e.Graphics.DrawString(Text, Font, fore, e.ClipRectangle, sf);
                }
            }
        }
    }
}
