using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Installer
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\\Script-Browser Setup.exe"))
                    File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\Script-Browser Setup.exe");
            }
            catch { }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InstallerForm());
        }
    }
}
