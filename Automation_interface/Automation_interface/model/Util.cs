using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;

namespace Automation_interface.model
{
    class Util
    {
        public static Random rand = new Random();
        public static DateTime currentDate;
        public static List<Replacer> usedValue = new List<Replacer>();

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


        public static KeyWords getRule(string fileName, bool isVote)
        {
            KeyWords rule = new KeyWords();
            using (var rd = new CsvReader(new StreamReader(fileName), false))
            {
                while (rd.ReadNextRecord())
                {
                    if (rd.FieldCount > 1)
                    {
                        if (rd[0].Contains("question"))
                        {
                            string pattern = @"\#(.*?)\#";
                            MatchCollection matches = Regex.Matches(rd[0], pattern);
                            int serial = Int32.Parse(matches[0].Value.Substring(1, (matches[0].Value.Length - 2)));
                            rule.questions.Add(new Question(rd[1].Replace("@", ","), serial));
                        }
                        else if (rd[0].Contains("metatitle"))
                        {
                            string pattern = @"\#(.*?)\#";
                            MatchCollection matches = Regex.Matches(rd[0], pattern);
                            int serial = Int32.Parse(matches[0].Value.Substring(1, (matches[0].Value.Length - 2)));
                            rule.metaTitles.Add(new MetaTitle(rd[1].Replace("@", ","), serial));
                        }
                        else if (rd[0].Contains("answer"))
                        {
                            string pattern = @"\#(.*?)\#";
                            MatchCollection matches = Regex.Matches(rd[0], pattern);
                            int serial = Int32.Parse(matches[0].Value.Substring(1, (matches[0].Value.Length - 2)));
                            Answer ans = new Answer(rd[1].Replace("@", ","), serial);
                            
                            if (isVote)
                            {
                                try
                                {
                                    string temp = rd[2];
                                    string[] range = temp.Split(',');
                                    int low = Int32.Parse(range[0]);
                                    int high = Int32.Parse(range[1]);
                                    ans.voteCount = rand.Next(low, high + 1);
                                }
                                catch (Exception e)
                                {
                                    throw new Exception("Vote count missing");
                                }
                            }
                            rule.answers.Add(ans);
                        }
                        else
                        {
                            Replacer re = new Replacer();
                            re.tagInhtml = rd[0].Trim().ToLower();
                            re.value = rd[1].Replace("@", ",");
                            if (rd.FieldCount > 2)
                            {
                                for (int i = 2; i < rd.FieldCount; i++)
                                {
                                    string s = rd[i].Trim();
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

        public static void loadUsedKeywordValue() {
            using (var rd = new StreamReader(@"UsedKeyword/usedKeyword.csv")) {
                string[] headers = rd.ReadLine().Split(',');
                if (headers.Length > 0) {
                    while (!rd.EndOfStream)
                    {
                        string[] values = rd.ReadLine().Split(',');
                        for (int i = 0; i < headers.Length; i++) {
                            Replacer r = new Replacer();
                            r.tagInhtml = headers[i];
                            r.value = values[i];
                            usedValue.Add(r);
                        }
                        
                    }
                }
                
            }
        }
        
        public static void saveUsedKeyWordValue() {
            if (usedValue.Count > 0)
            {
                using (var sw = new StreamWriter(@"UsedKeyword/usedKeyword.csv"))
                {
                    for (var i = 0; i <= usedValue.Count; i++)
                    {
                        sw.WriteLine("Error_Flag = 'FOR_IMPORT' and location_type =   'Home' and batch_num = {0}", i);
                    }
                }
            }
            else {
                File.WriteAllText(@"UsedKeyword/usedKeyword.csv", "");
            }
            
        }

    }
}
