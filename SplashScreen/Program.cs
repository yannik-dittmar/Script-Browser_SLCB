using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SplashScreen
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>

        [STAThread]
        static void Main(string[] args)
        {
            //string directory = Path.GetDirectoryName(Application.ExecutablePath) + "\\Update";
            //if (!Directory.Exists(directory))
            //    Directory.CreateDirectory(directory);

            //if (File.Exists(directory + "\\changeFiles.txt"))
            //{
            //    string[] files = File.ReadAllLines(directory + "\\changeFiles.txt");
            //    for (int i = 0; i < files.Length; i++)
            //        try { File.Move(directory + "\\" + i, Path.GetDirectoryName(Application.ExecutablePath) + "\\test" + files[i]); } catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
            //    File.Delete(directory + "\\changeFiles.txt");
            //}

            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance();
                return;
            }

            //Check if Script-Browser exists
            try
            {
                Process[] pros = Process.GetProcessesByName("Script-Browser");
                if (pros.Count() != 0)
                {
                    string processGuid = GetAssemblyGuid(Assembly.LoadFrom(pros[0].MainModule.FileName));
                    WinApi.PostMessage(
                        (IntPtr)WinApi.HWND_BROADCAST,
                        WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", processGuid),
                        IntPtr.Zero,
                        IntPtr.Zero);
                    return;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
                Application.Run(new Main());
            else
                Application.Run(new Main(true));

            SingleInstance.Stop();
        }

        private static string GetAssemblyGuid(Assembly assembly)
        {
            object[] customAttribs = assembly.GetCustomAttributes(typeof(GuidAttribute), false);
            if (customAttribs.Length < 1)
                return null;

            return ((GuidAttribute)(customAttribs.GetValue(0))).Value.ToString();
        }
    }

    #region SingleInstance

    static public class SingleInstance
    {
        public static readonly int WM_SHOWFIRSTINSTANCE =
            WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);
        static Mutex mutex;

        static public bool Start()
        {
            bool onlyInstance = false;
            string mutexName = String.Format("Local\\{0}", ProgramInfo.AssemblyGuid);

            mutex = new Mutex(true, mutexName, out onlyInstance);

            return onlyInstance;
        }

        static public void ShowFirstInstance()
        {
            WinApi.PostMessage(
                (IntPtr)WinApi.HWND_BROADCAST,
                WM_SHOWFIRSTINSTANCE,
                IntPtr.Zero,
                IntPtr.Zero);
        }

        static public void Stop()
        {
            mutex.ReleaseMutex();
        }
    }

    static public class WinApi
    {
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        public static int RegisterWindowMessage(string format, params object[] args)
        {
            string message = String.Format(format, args);
            return RegisterWindowMessage(message);
        }

        public const int HWND_BROADCAST = 0xffff;
        public const int SW_SHOWNORMAL = 1;

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImportAttribute("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ShowToFront(IntPtr window)
        {
            ShowWindow(window, SW_SHOWNORMAL);
            SetForegroundWindow(window);
        }
    }

    static public class ProgramInfo
    {
        static public string AssemblyGuid
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
                if (attributes.Length == 0)
                {
                    return String.Empty;
                }
                return ((GuidAttribute)attributes[0]).Value;
            }
        }
        static public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
            }
        }
    }

    #endregion
}
