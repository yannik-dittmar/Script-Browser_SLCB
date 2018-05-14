using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.WinForms;
using CefSharp;
using System.Web;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Script_Browser.Controls
{
    public partial class Comments : UserControl
    {
        private ChromiumWebBrowser web = new ChromiumWebBrowser();
        private string htmlComment = "<div class='comment' id='{0}'><span class='user' id='{0}'>{1}</span><span class='time' id='{0}'>{2}</span>{3}</div>";
        private string htmlSubComment = "<div class='subComment' id='{0}'><span class='user' id='{0}'>{1}</span><span class='time' id='{0}'>{2}</span>{3}</div>";
        private string placeholder = "<comment> id=";

        public Comments()
        {
            InitializeComponent();

            this.Controls.Add(web);
            web.Dock = DockStyle.Fill;
            web.AddressChanged += new EventHandler<AddressChangedEventArgs>(AddressChanged);
        }

        public void LoadComments(int id, Form form)
        {
            web.Load(Environment.CurrentDirectory + @"\HTML\LoadingComments.html");

            try
            {
                JObject result = Networking.GetComments(id, form);
                JArray comments = (JArray)result["comments"];
                List<JToken> openComments = new List<JToken>(GetComToReplyId(comments, "0"));
                List<string> placeholders = new List<string>(new string[]{ placeholder + "0" });
                string htmlcomments = "";
                //bool readOnly = !(bool)result["Verified"];

                htmlcomments += placeholder + "0";

                while (openComments.Count != 0)
                {
                    JToken com = openComments[0];
                    string delete = "";
                    if (com["Username"].ToString() == Main.sf.username)
                        delete = "<span class='cancel' id='" + com["ID"] + "' style='float:right;'>X</span>";

                    string usedHtmlComment = htmlSubComment;
                    if (GetAmountOfParents(comments, com) % 2 == 0)
                        usedHtmlComment = htmlComment;

                    htmlcomments = htmlcomments.Replace(placeholder + com["ReplyID"], placeholder + com["ReplyID"] + String.Format(usedHtmlComment, com["ID"], com["Username"], com["Time"], delete + placeholder + com["ID"]));
                    placeholders.Add(placeholder + com["ID"]);

                    openComments.AddRange(GetComToReplyId(comments, com["ID"].ToString()));
                    openComments.RemoveAt(0);
                }

                foreach (string p in placeholders)
                {
                    try
                    {
                        JToken com = comments.FirstOrDefault(x => placeholder + x["ID"] == p);
                        htmlcomments = htmlcomments.Replace(p, "<p id='" + com["ID"] + "'>" + com["Comment"] + "</p>");
                    }
                    catch { htmlcomments = htmlcomments.Replace(p, ""); }
                }

                string htmlLoad = File.ReadAllText(Environment.CurrentDirectory + @"\HTML\Comments.html").Replace("<comment INPUT>", htmlcomments);

                if (!(bool)result["Verified"])
                {
                    htmlLoad = htmlLoad.Replace("<textarea name='comment' id='comment' placeholder='Your comment...'", "<textarea name='comment' id='comment' placeholder='Login with a verified account to write comments.' disabled");
                }

                web.LoadHtml(htmlLoad, Environment.CurrentDirectory + @"\HTML\Comments.html");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                web.Load(Environment.CurrentDirectory + @"\HTML\CommentError.html");
            }
        }

        private int GetAmountOfParents(JArray comments, JToken com)
        {
            int counter = 0;

            for (; com["ReplyID"].ToString() != "0"; counter++)
                com = comments.FirstOrDefault(x => x["ID"].ToString() == com["ReplyID"].ToString());

            return counter;
        }

        private List<JToken> GetComToReplyId(JArray comments, string id)
        {
            List<JToken> result = new List<JToken>();

            foreach (JToken item in comments)
            {
                if (item["ReplyID"].ToString() == id)
                    result.Add(item);
            }

            return result;
        }

        private void AddressChanged(object sender, AddressChangedEventArgs e)
        {
            Console.WriteLine(e.Address);
        }
    }
}