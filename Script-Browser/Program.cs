using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SaveManager;

namespace Script_Browser
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if (args.Count() == 2)
                {
                    SaveFile sf = new SaveFile(Path.GetDirectoryName(Application.ExecutablePath) + @"\settings.save");
                    sf.streamlabsPath = args[0];
                    sf.Save();

                    if (args[1] != "true")
                        return;
                }
            }
            catch { }
            //TODO: Add crash report: https://github.com/ravibpatel/CrashReporter.NET
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
            //Application.Run(new Test());
        }
    }
}
