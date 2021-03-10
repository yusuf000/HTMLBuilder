using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Automation_interface.model
{
    class PageBuilder
    {
        private string mainHeader;
        private string headerhtmlcode;
        private string posthtmlcode;
        private string replyhtmlcode;
        private string footerhtmlcode;
        private string replybackhtml;
        private string banner;
        private string metastring;
        private string page;
        private int questionIndex;
        private string poster;
        private string countTest = "Posts: ";
        private DateTime currentTime;
        private model.KeyWords rule;
        private string metaTeg = "";
        private Dictionary<string, string> questionRule;
        private Dictionary<string, Replacer> usedReplacers;
        private List<Replacer> listOfReplacers;
        private List<Answer> answerList;
        

        public PageBuilder()
        {
            mainHeader = File.ReadAllText("HTMLs/MainHeader.txt");
            headerhtmlcode = File.ReadAllText("HTMLs/PageheaderHTMLCode.txt");
            posthtmlcode = File.ReadAllText("HTMLs/BodyHtmlCode.txt");
            replyhtmlcode = File.ReadAllText("HTMLs/post_replyhtmlcode.txt");
            footerhtmlcode = File.ReadAllText("HTMLs/footerHtmlCode.txt");
            replybackhtml = File.ReadAllText("HTMLs/postreplyback.txt");
            banner = File.ReadAllText("HTMLs/Banner.txt");
            
        }

        public string createpage(List<string> templete, model.KeyWords r, DateTime cur, bool isVote)
        {
            this.page = mainHeader;
            rule = r;
            currentTime = cur;
            if (isVote)
            {
                countTest = "Votes: ";
            }

            int questionCount = 0;
            string metaTitle = null;
            bool isWaitingForModerator = true;
            for (int i = 0; i < templete.Count; i++)
            {
                if (templete[i] == "*headerhtmlcode*")
                {
                    if (!isWaitingForModerator)
                    {
                        metaTitle = findMetatitle();
                        createHeader(metaTitle);
                    }
                } else if (templete[i] == "*posthtmlcode*")
                {
                    i++;
                    string question = "";
                    questionIndex = -1;
                    while (!templete[i].Contains("question"))
                    {
                        var rc = findByKey(templete[i]);
                        question += rc[Util.rand.Next(0, rc.Count)].value + " ";
                        i++;
                    }

                    if (!templete[i].Contains("random"))
                    {
                        string pattern = @"\#(.*?)\#";
                        //string pattern = @"a.e";
                        MatchCollection matches = Regex.Matches(templete[i], pattern);
                        questionIndex = Int32.Parse(matches[0].Value.Substring(1, (matches[0].Value.Length - 2)));
                    }
                    else
                    {
                        questionIndex = Util.rand.Next(0, rule.questions.Count);
                    }
                    questionRule = new Dictionary<string, string>();
                    usedReplacers = new Dictionary<string, Replacer>();
                    listOfReplacers = new List<Replacer>();
                    string q = findQuestion();
                    if (isWaitingForModerator)
                    {
                        metaTitle = findMetatitle();
                        createHeader(metaTitle);
                        isWaitingForModerator = false;
                    }
                    createQuestion(q, metaTitle);
                }
                else if (templete[i] == "*replyhtmlcode*")
                {
                    i++;
                    string question = "";
                    while (!templete[i].Contains("answer"))
                    {
                        var rc = findByKey(templete[i]);
                        question += rc[Util.rand.Next(0, rc.Count)].value + " ";
                        i++;
                    }
                    string q = findAnswer();
                    
                    createAnswer(q, metaTitle);
                }
                else if (templete[i].Contains("*footerhtmlcode*"))
                {
                    
                    createFooter();
                }
            }

            return page;
        }

        private void createHeader(string metaTile)
        {
            string copyHeader = headerhtmlcode;
            copyHeader = copyHeader.Replace("<*metatitle*>", metaTile);
            var moderators = findByKey("*Moderators*");
            string moderator = moderators[Util.rand.Next(0, moderators.Count)].value;
            copyHeader = copyHeader.Replace("<*Moderators*>", moderator);
            this.page = page + copyHeader;
        }

        private void createFooter()
        {
            this.page = page + banner;
        }

        private void createQuestion(string question, string metaTitle)
        {
            string copy = posthtmlcode;
            copy = copy.Replace("<*metatitle*>",  metaTitle);
            copy = copy.Replace("<*greeting*>", "");
            copy = copy.Replace("<*question#>", question);
            var posters = findByKey("*postername*");
            var pos = posters[Util.rand.Next(0, posters.Count)];
            poster = pos.value;
            copy = copy.Replace("<*postername*>", poster);
            copy = copy.Replace("<*date*>", currentTime.ToString("ddd, dd MMM yyy HH:mm:ss tt"));
            currentTime = currentTime.AddMinutes(Util.rand.Next(10, 7200));
            copy = copy.Replace("Colleges and&nbsp;<*level*>", "Level: " + clearAllTag(pos.variant[0]));
            copy = copy.Replace("<*location*>", clearAllTag(pos.variant[1]));
            copy = copy.Replace("Schools", countTest + clearAllTag(pos.variant[2]));
            this.page = page + copy;
        }

        private void createAnswer(string answer, string metatitle)
        {
            string copy = posthtmlcode;
            copy = copy.Replace("<*metatitle*>", "Re: " + metatitle);
            copy = copy.Replace("<*question#>", answer);
            copy = copy.Replace("<*greeting*>", "");
            var posters = findByKey("*repliername*");
            var pos = posters[Util.rand.Next(0, posters.Count)];
            poster = pos.value;
            copy = copy.Replace("<*postername*>", poster);
            copy = copy.Replace("<*date*>", currentTime.ToString("ddd, dd MMM yyy HH:mm:ss tt"));
            currentTime = currentTime.AddMinutes(Util.rand.Next(10, 7200));
            copy = copy.Replace("Colleges and&nbsp;<*level*>", "Level: " + clearAllTag(pos.variant[0]));
            copy = copy.Replace("<*location*>", clearAllTag(pos.variant[1]));
            copy = copy.Replace("Schools", countTest + clearAllTag(pos.variant[2]));
            this.page = page + copy;
        }

        private List<Replacer> findByKey(string key)
        {
            List<Replacer> list = new List<Replacer>();
            for (int i = 0; i < this.rule.replacers.Count; i++)
            {
                if (rule.replacers[i].tagInhtml == key.Trim().ToLower())
                {
                    list.Add(rule.replacers[i]);
                }
            }

            return list;
        }

        private string findQuestion()
        {
            if (questionIndex < 0 )
            {
                questionIndex = Util.rand.Next(0, rule.questions.Count);
            }
            Question q = rule.findQuestion(questionIndex);
            answerList = rule.FindAnswerList(questionIndex);
            return clearAllTag(q.value, true);
        }

        private string findMetatitle()
        {
            MetaTitle m = rule.FindMetaTitle(questionIndex);
            return clearAllTag(m.value);
        }

        private string findAnswer()
        {
            int i = Util.rand.Next(0, answerList.Count);
            Answer a = answerList[i];
            answerList.RemoveAt(i);
            return clearAllTag(a.value);
        }

        private string clearAllTag(string text, bool isQuestion = false)
        {
            string pattern = @"\*(.*?)\*";
            MatchCollection matches = Regex.Matches(text, pattern);
            while (matches.Count > 0)
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    if (isQuestion == false)
                    {
                        if (questionRule.ContainsKey(matches[i].Value))
                        {
                            text = text.Replace(matches[i].Value, questionRule[matches[i].Value]);
                            continue;
                        }
                    }

                    string key;
                    if (matches[i].Value.Contains("variant"))
                    {
                        key = matches[i].Value.Substring(0, matches[i].Value.IndexOf('-') + 1);
                        key = key.Replace("-", "*");
                        Replacer re = null;
                        if (usedReplacers.ContainsKey(key))
                        {
                            re = usedReplacers[key];
                        }
                        else
                        {
                            var res = findByKey(key);
                            if (res.Count < 1)
                            {
                                throw new Exception(key + " This is not present in rule file please check carefully.");
                            }
                            re = res[Util.rand.Next(0, res.Count)];
                        }
                        var value = re.variant[Util.rand.Next(0, re.variant.Count)];
                        text = text.Replace(matches[i].Value, value);
                        if (isQuestion)
                        {
                            questionRule[matches[i].Value] = value;
                        }
                    }
                    else if (matches[i].Value.Contains("other"))
                    {
                        key = matches[i].Value.Substring(0, matches[i].Value.IndexOf('-') + 1);
                        key = key.Replace("-", "*");
                        var res = findByKey(key);
                        if (res.Count < 1)
                        {
                            throw new Exception(key + " This is not present in rule file please check carefully.");
                        }

                        res = filterUsed(res);

                        if (res.Count < 1)
                        {
                            throw new Exception(key + " This is no other value present in rule file please check carefully.");
                        }
                        var rep = res[Util.rand.Next(0, res.Count)];
                        text = text.Replace(matches[i].Value, rep.value);
                        if (isQuestion)
                        {
                            questionRule[matches[i].Value] = rep.value;
                        }
                    }
                    else
                    {
                        key = matches[i].Value;
                        var re = findByKey(key);
                        if (re.Count < 1)
                        {
                            throw new Exception(key + " This is not present in rule file please check carefully.");
                        }
                        var rep = re[Util.rand.Next(0, re.Count)];
                        text = text.Replace(matches[i].Value, rep.value);
                        if (isQuestion)
                        {
                            questionRule[matches[i].Value] = rep.value;
                        }

                        usedReplacers[matches[i].Value] = rep;
                    }
                }
                matches = Regex.Matches(text, pattern);
            }

            return text;
        }

        private List<Replacer> filterUsed(List<Replacer> res)
        {
            List<Replacer> replacers = new List<Replacer>();
            for (int i = 0; i < res.Count; i++)
            {
                bool used = false;
                for (int j = 0; j < listOfReplacers.Count; j++)
                {
                    if (res[i].equal(listOfReplacers[j]))
                    {
                        used = true;
                        break;
                    }
                }

                if (!used)
                {
                    replacers.Add(res[i]);
                }
            }

            return replacers;
        }
    }
}
