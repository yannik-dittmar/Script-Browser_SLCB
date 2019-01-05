using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Script_Browser
{
    public partial class Protocol : Form
    {
        public static List<string[]> protocol = new List<string[]>();

        public Protocol()
        {
            InitializeComponent();

            foreach (string[] entry in protocol)
                richTextBox1.AppendText("[" + entry[0] + "]: " + entry[1] + "\n", Color.FromArgb(Int32.Parse(entry[2]), Int32.Parse(entry[3]), Int32.Parse(entry[4])));
            richTextBox1.ScrollToCaret();
        }

        public static void AddToProtocol(string msg, Types type)
        {
            protocol.Add(new string[] { DateTime.Now.ToString("HH:mm:ss"), msg.Replace("\n", "\n                        "), type.ToString() });
            Save();
        }

        public static void Save()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (string[] entry in protocol)
                    lines.Add("[" + entry[0] + "|" + entry[2].Substring(0, 1) + "]: " + entry[1]);
                File.WriteAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\log.txt", lines);
            }
            catch { }
        }
    }

    public enum Types
    {
        Info,
        Warning,
        Error
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
