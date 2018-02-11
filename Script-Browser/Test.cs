using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            openFileDialog1.ShowDialog();
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