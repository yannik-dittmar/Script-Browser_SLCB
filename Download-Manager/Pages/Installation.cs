using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Download_Manager.Pages
{
    public partial class Installation : UserControl
    {
        public Installation()
        {
            InitializeComponent();
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        //FolderBrowser for selection of path for download
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        //FolderBrowser for selection of path of Streamlabs Chatbot
        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
