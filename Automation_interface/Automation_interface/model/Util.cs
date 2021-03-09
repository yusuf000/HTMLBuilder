﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Automation_interface.model
{
    class Util
    {
        public  static Random rand = new Random();

        public static string StripTags(string html)
        {
            html = WebUtility.HtmlDecode(html);
            html = Regex.Replace(html, "<script[^>]*>([\\w\\W]*?)</script>", " ");
            html = Regex.Replace(html, "<style[^>]*>([\\w\\W]*?)</style>", " ");
            html = Regex.Replace(html, "<!--([\\w\\W]*?)-->", " ");
            html = Regex.Replace(html, "<([\\w\\W]*?)>", " ");
            html = Regex.Replace(html, "<.*?>", " ");
            html = html.Replace("&amp;", "&");
            html = html.Replace(Environment.NewLine, " ");
            html = html.Replace("\n", " ").Replace("\r\n", " ").Replace("\r", " ");
            html = html.Replace("\\t", " ");
            html = Regex.Replace(html, "\\s+", " ");
            html = html.Trim();
            return html;
        }

        public static string PageHeader(string matatitle, string moderator)
        {
            string htmlhedercode = "";
            StreamReader inputReder = new StreamReader(string.Concat(System.Windows.Forms.Application.StartupPath, "\\PageheaderHTMLCode.txt"));
            string dataread = "";
            dataread = inputReder.ReadLine();
            inputReder.Close();
            htmlhedercode = dataread.Replace("<*metatitle*>", matatitle);
            return htmlhedercode.Replace("<*Moderators*>", moderator);
        }


        public static Rule getRule(string fileName)
        {
            Rule rule = new Rule();
            using (var rd = new StreamReader(fileName))
            {
                while (!rd.EndOfStream)
                {
                    List<string> list = new List<string>();
                    var splits = rd.ReadLine().Split(',');
                    if (splits.Length > 1)
                    {
                        if (splits[0].Contains("question"))
                        {
                            string pattern = @"\#(.*?)\#";
                            MatchCollection matches = Regex.Matches(splits[0], pattern);
                            int serial = Int32.Parse(matches[0].Value.Substring(1, (matches[0].Value.Length - 2)));
                            rule.questions.Add(new Question(splits[1], serial));
                        }
                        else if (splits[0].Contains("metatitle"))
                        {
                            string pattern = @"\#(.*?)\#";
                            MatchCollection matches = Regex.Matches(splits[0], pattern);
                            int serial = Int32.Parse(matches[0].Value.Substring(1, (matches[0].Value.Length - 2)));
                            rule.metaTitles.Add(new MetaTitle(splits[1], serial));
                        }
                        else if (splits[0].Contains("answer"))
                        {
                            string pattern = @"\#(.*?)\#";
                            MatchCollection matches = Regex.Matches(splits[0], pattern);
                            int serial = Int32.Parse(matches[0].Value.Substring(1, (matches[0].Value.Length - 2)));
                            rule.answers.Add(new Answer(splits[1], serial));
                        }
                        else
                        {
                            Replacer re = new Replacer();
                            re.tagInhtml = splits[0].Trim().ToLower();
                            re.value = splits[1].Trim();
                            if (splits.Length > 2)
                            {
                                for (int i = 2; i < splits.Length; i++)
                                {
                                    string s = splits[i].Trim();
                                    if (s.Length > 0)
                                        re.variant.Add(s);
                                }
                            }
                            rule.replacers.Add(re);
                        }

                        
                    }

                    
                }
            }
            return rule;
        }
    }
}