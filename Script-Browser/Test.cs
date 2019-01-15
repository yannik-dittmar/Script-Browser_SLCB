using CefSharp;
using CefSharp.WinForms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

            MessageBox.Show(Path.GetDirectoryName(Application.ExecutablePath));
            MessageBox.Show(Application.ExecutablePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\Users\KryptoPC\Desktop\Test\test.exe");
        }
    }
}