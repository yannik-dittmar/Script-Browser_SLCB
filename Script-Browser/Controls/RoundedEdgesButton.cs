using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser.Controls
{
    class RoundedEdgesButton : Button
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle Rect = new Rectangle(0, 0, this.Width, this.Height);
            GraphicsPath GraphPath = new GraphicsPath();
            GraphPath.AddArc(Rect.X, Rect.Y, 30, 30, 180, 90);
            GraphPath.AddArc(Rect.X + Rect.Width - 30, Rect.Y, 30, 30, 270, 90);
            GraphPath.AddArc(Rect.X + Rect.Width - 30, Rect.Y + Rect.Height - 30, 30, 30, 0, 90);
            GraphPath.AddArc(Rect.X, Rect.Y + Rect.Height - 30, 30, 30, 90, 90);
            this.Region = new Region(GraphPath);
        }
    }
}
