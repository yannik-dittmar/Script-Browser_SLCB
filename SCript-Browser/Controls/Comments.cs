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
using System.Collections.Specialized;
using System.Diagnostics;

namespace Script_Browser.Controls
{
    public partial class Comments : UserControl
    {
        private ChromiumWebBrowser web = new ChromiumWebBrowser();
        private string htmlComment = "<div class='comment' id='{0}'><span class='user' id='{0}'>{1}</span><span class='time' id='{0}'>{2}</span>{3}</div>";
        private string htmlSubComment = "<div class='subComment' id='{0}'><span class='user' id='{0}'>{1}</span><span class='time' id='{0}'>{2}</span>{3}</div>";
        private string htmlReload = "";
        private string placeholder = "<comment> id=";

        private Main form;
        private int id;
        private bool admin;

        public Comments()
        {
            InitializeComponent();

            this.Controls.Add(web);
            web.Dock = DockStyle.Fill;
            web.AddressChanged += new EventHandler<AddressChangedEventArgs>(AddressChanged);
            web.LoadError += new EventHandler<LoadErrorEventArgs>(LoadError);
        }

        public void LoadComments(int id, Main form, bool admin = false)
        {
            web.Load(Environment.CurrentDirectory + @"\HTML\LoadingComments.html");
            this.form = form;
            this.id = id;
            this.admin = admin;

            try
            {
                JObject result = Networking.GetComments(form, id);
                JArray comments = (JArray)result["comments"];
                List<JToken> openComments = new List<JToken>(GetComToReplyId(comments, "0"));
                List<string> placeholders = new List<string>(new string[]{ placeholder + "0>" });
                string htmlcomments = placeholder + "0>";

                while (openComments.Count != 0)
                {
                    JToken com = openComments[0];
                    string delete = "";
                    if (com["Username"].ToString() == Main.sf.username || admin)
                        delete = "<span class='cancel' id='" + com["ID"] + "' style='float:right;'>X</span>";

                    string usedHtmlComment = htmlSubComment;
                    if (GetAmountOfParents(comments, com) % 2 == 0)
                        usedHtmlComment = htmlComment;

                    htmlcomments = htmlcomments.Replace(placeholder + com["ReplyID"] + ">", placeholder + com["ReplyID"] + ">" + String.Format(usedHtmlComment, com["ID"], com["Username"], com["Time"], delete + placeholder + com["ID"] + ">"));
                    placeholders.Add(placeholder + com["ID"] + ">");

                    openComments.AddRange(GetComToReplyId(comments, com["ID"].ToString()));
                    openComments.RemoveAt(0);
                }

                foreach (string p in placeholders)
                {
                    try
                    {
                        JToken com = comments.FirstOrDefault(x => placeholder + x["ID"] + ">" == p);
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
                htmlReload = htmlLoad;
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

        private void LoadError(object sender, LoadErrorEventArgs e)
        {
            if (!e.FailedUrl.Contains("CommentError.html"))
                web.Load(Environment.CurrentDirectory + @"\HTML\CommentError.html");
        }

        private void AddressChanged(object sender, AddressChangedEventArgs e)
        {
            Console.WriteLine(e.Address);
            form.BeginInvoke(new MethodInvoker(delegate () 
            {
                try
                {
                    Uri infos = new Uri(e.Address);
                    NameValueCollection vars = HttpUtility.ParseQueryString(infos.Query);

                    if (vars.AllKeys.Count() != 0 && !vars.AllKeys.Contains("refresh"))
                        web.LoadHtml(htmlReload, Environment.CurrentDirectory + @"\HTML\Comments.html");

                    if (vars.AllKeys.Contains("comment"))
                    {
                        if (!String.IsNullOrWhiteSpace(vars.Get("comment")))
                        {
                            if (Networking.SendComment(form, id, vars.Get("comment"), vars.Get("reply")))
                                LoadComments(id, form, admin);
                        }
                    }
                    else if (vars.AllKeys.Contains("delete"))
                    {
                        DialogResult result = MetroFramework.MetroMessageBox.Show(form, "Do you really want to delete the comment and all the replys to it?", "Delete Comment", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, 100);
                        if (result == DialogResult.Yes)
                        {
                            if (Networking.DeleteComment(form, vars.Get("delete")))
                                LoadComments(id, form, admin);
                            else
                                MetroFramework.MetroMessageBox.Show(form, "Could't delete your comment. Please try again later.", "Could not delete comment", MessageBoxButtons.OK, MessageBoxIcon.Error, 100);
                        }
                    }
                    else if (vars.AllKeys.Contains("refresh"))
                        LoadComments(id, form, admin);
                    else if (vars.AllKeys.Contains("discord"))
                        Process.Start("http://discord.gg/KDe7Vyu");
                }
                catch { web.Load(Environment.CurrentDirectory + @"\HTML\CommentError.html"); }
            }));
        }
    }
}