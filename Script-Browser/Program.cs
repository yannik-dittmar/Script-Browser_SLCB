using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
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
            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance();
                return;
            }

            //Enable unverified HTTPS for own server
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate{ return true; });

            //TODO: Add crash report: https://github.com/ravibpatel/CrashReporter.NET
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.AssemblyResolve += Resolver;
            LoadApp(args);

            SingleInstance.Stop();
        }

        #region CefSharp

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void LoadApp(string[] args)
        {
            var settings = new CefSettings();

            // Set BrowserSubProcessPath based on app bitness at runtime
            settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                   Environment.Is64BitProcess ? "x64" : "x86",
                                                   "CefSharp.BrowserSubprocess.exe");

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

            if (args.Length == 0)
                Application.Run(new Main());
            else
                Application.Run(new Main(true));
            //Application.Run(new Test());
        }

        // Will attempt to load missing assembly from either x86 or x64 subdir
        private static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                return File.Exists(archSpecificPath)
                           ? Assembly.LoadFile(archSpecificPath)
                           : null;
            }

            return null;
        }

        #endregion

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
}
