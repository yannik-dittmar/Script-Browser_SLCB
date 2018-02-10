using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Download_Manager
{
    //class for checking if user scrolled to end of a richTextBox
    //credits: https://stackoverflow.com/questions/19343794/how-to-know-if-richtextbox-vertical-scrollbar-reached-the-max-value
    public static class RichTextBoxExtension
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int GetScrollInfo(IntPtr hwnd, int nBar, ref SCROLLINFO scrollInfo);

        public struct SCROLLINFO
        {
            public int cbSize;
            public int fMask;
            public int min;
            public int max;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }
        public static bool ReachedBottom(this System.Windows.Forms.RichTextBox rtb)
        {
            SCROLLINFO scrollInfo = new SCROLLINFO();
            scrollInfo.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(scrollInfo);
            //SIF_RANGE = 0x1, SIF_TRACKPOS = 0x10,  SIF_PAGE= 0x2
            scrollInfo.fMask = 0x10 | 0x1 | 0x2;
            GetScrollInfo(rtb.Handle, 1, ref scrollInfo);//nBar = 1 -> VScrollbar
            return scrollInfo.max == scrollInfo.nTrackPos + scrollInfo.nPage;
        }
    }
}
