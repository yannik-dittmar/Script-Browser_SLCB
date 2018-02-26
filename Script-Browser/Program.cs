using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new UploadScript(@"D:\Streamlabs Chatbot\Twitch\Scripts\WelcomeMat\WelcomeMat_StreamlabsSystem.py"));
            //Application.Run(new UploadScript(@"C:\Users\brude\Desktop\Neuer Ordner\test_StreamlabsSystem.py"));
            Application.Run(new Main());
            //Application.Run(new Test());
        }
    }
}
