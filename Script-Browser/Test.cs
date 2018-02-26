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

            string[] lines = File.ReadAllLines(@"C:\Users\Yannik\Desktop\Neues Textdokument.txt");
            using (StreamWriter writer = new StreamWriter(@"C:\Users\Yannik\Desktop\Neues Textdokument.txt"))
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("test="))
                        writer.WriteLine(PutInValue(lines[i], "ultimaten"));
                    else
                        writer.WriteLine(lines[i]);
                }
            }
        }

        private string PutInValue(string line, string value)
        {
            List<char> charLine = line.ToList();

            bool found = false;
            for (int i = 0; i < charLine.Count; i++)
            {
                if (charLine[i] == '\"')
                {
                    if (!found)
                        found = true;
                    else
                    {
                        foreach (char c in value.ToCharArray().Reverse())
                            charLine.Insert(i, c);

                        string result = "";
                        foreach (char c in charLine)
                            result += c;
                        return result;
                    }
                }
                else if (found)
                {
                    charLine.RemoveAt(i);
                    i--;
                }
            }

            return line;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Console.WriteLine(openFileDialog1.FileName);
            using (WebClient web = new WebClient())
                Console.WriteLine("RESULT: " + Encoding.Default.GetString(web.UploadFile("http://slcbsb.duckdns.org/Script%20Browser/Scripts/fileUpload.php", "POST", openFileDialog1.FileName)));
            Console.WriteLine("FINISHED");
        }
    }
}