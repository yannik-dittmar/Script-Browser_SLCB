using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser.Design
{
    public class ColorTableMenu : ProfessionalColorTable
    {
        public ColorTableMenu()
        {
            base.UseSystemColors = false;
        }
        
        Color color = Color.FromArgb(18, 25, 31);
        Color color2 = Color.FromArgb(51, 139, 118);
        Color color3 = Color.FromArgb(51, 139, 118);

        public override Color MenuItemSelected
        {
            get { return color3; }
        }

        public override Color MenuItemBorder
        {
            get { return color; }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return color; }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return color; }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return color; }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return color; }
        }

        public override Color MenuBorder
        {
            get { return Color.FromArgb(105, 136, 165); }
        }

        public override Color ToolStripDropDownBackground
        {
            get { return color; }
        }
        public override Color ToolStripBorder
        {
            get { return color; }
        }
        public override Color RaftingContainerGradientBegin
        {
            get { return color; }
        }
        public override Color RaftingContainerGradientEnd
        {
            get { return color; }
        }
        public override Color ToolStripGradientBegin
        {
            get { return color; }
        }
        public override Color ToolStripGradientEnd
        {
            get { return color; }
        }
        public override Color MenuStripGradientBegin
        {
            get { return color; }
        }
        public override Color MenuStripGradientEnd
        {
            get { return color; }
        }
        public override Color SeparatorDark
        {
            get { return Color.White; }
        }
        public override Color SeparatorLight
        {
            get { return Color.White; }
        }
        public override Color GripDark
        {
            get { return color; }
        }
        public override Color GripLight
        {
            get { return color; }
        }
        public override Color CheckBackground
        {
            get { return color; }
        }
        public override Color ButtonSelectedBorder
        {
            get { return color; }
        }
        public override Color ToolStripContentPanelGradientBegin
        {
            get { return color; }
        }
        public override Color ToolStripContentPanelGradientEnd
        {
            get { return color; }
        }
        public override Color ToolStripPanelGradientBegin
        {
            get { return color; }
        }
        public override Color ToolStripPanelGradientEnd
        {
            get { return color; }
        }
        public override Color StatusStripGradientBegin
        {
            get { return color; }
        }
        public override Color StatusStripGradientEnd
        {
            get { return color; }
        }
        public override Color ImageMarginGradientBegin
        {
            get { return color; }
        }
        public override Color ImageMarginGradientEnd
        {
            get { return color; }
        }
        public override Color ImageMarginRevealedGradientBegin
        {
            get { return color; }
        }
        public override Color ImageMarginRevealedGradientEnd
        {
            get { return color; }
        }
        public override Color ImageMarginGradientMiddle
        {
            get { return color; }
        }
        public override Color MenuItemPressedGradientMiddle
        {
            get { return color; }
        }
        public override Color ToolStripGradientMiddle
        {
            get { return color; }
        }
    }
}
