using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Console.Title = "Script-Browser Updater";
            Console.WriteLine("Waiting for Applications to close...");

            while (Process.GetProcessesByName("Script-Browser").Length != 0 || Process.GetProcessesByName("SLCBSB-SplashScreen").Length != 0)
                Thread.Sleep(100);

            Console.WriteLine("Moving files...");

            if (File.Exists(directory + "\\changeFiles.txt"))
            {
                string[] files = File.ReadAllLines(directory + "\\changeFiles.txt");
                for (int i = 0; i < files.Length; i++)
                {
                    try
                    {
                        if (File.Exists(Path.GetDirectoryName(directory) + files[i]))
                            File.Delete(Path.GetDirectoryName(directory) + files[i]);
                        File.Move(directory + "\\" + i, Path.GetDirectoryName(directory) + files[i]);
                        Console.WriteLine("Moved: " + files[i]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to move: " + files[i]);
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        Console.WriteLine();
                    }
                }
                File.Delete(directory + "\\changeFiles.txt");
            }

            Console.WriteLine("Removing files...");

            if (File.Exists(directory + "\\remove.txt"))
            {
                string[] files = File.ReadAllLines(directory + "\\remove.txt");
                foreach (string file in files)
                {
                    try
                    {
                        if (File.GetAttributes(Path.GetDirectoryName(directory) + file) == FileAttributes.Directory)
                            Directory.Delete(Path.GetDirectoryName(directory) + file, true);
                        else
                            File.Delete(Path.GetDirectoryName(directory) + file);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to remove: " + file);
                        Console.WriteLine();
                        Console.WriteLine(ex.StackTrace);
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine("Finished!");
        }
    }
}
