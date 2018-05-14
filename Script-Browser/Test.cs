using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();

            //string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            //using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            //{
            //    foreach (string subkey_name in key.GetSubKeyNames())
            //    {
            //        using (RegistryKey subkey = key.OpenSubKey(subkey_name))
            //        {
            //            try
            //            {
            //                if (subkey.GetValue("DisplayName") != null)
            //                { 
            //                    if (subkey.GetValue("DisplayName").ToString().Contains("Streamlabs Chatbot"))
            //                        Console.WriteLine(subkey.GetValue("InstallLocation"));
            //                }
            //            }
            //            catch { }
            //        }
            //    }
            //}
        }

        private void comments1_Load(object sender, EventArgs e)
        {
            comments1.LoadComments(1, this);
        }
    }
}