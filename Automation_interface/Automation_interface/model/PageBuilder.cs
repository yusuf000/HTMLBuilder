using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Automation_interface.model
{
    class PageBuilder
    {
        private string[] templateHeader;
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
        private Replacer questionState = null;
        private Replacer answerState = null;
        private Replacer questioner = null;
        private Replacer replier = null;
        private string poster;
        private bool canReplierreplyback = false;
        private bool isvote;
        private model.KeyWords rule;
        private string metaTeg = "";
        private int percentage;
        private string prevReply;
        private string prevAnswer;
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

        public string createpage(List<string> templete, model.KeyWords r, bool isVote, string[] headers, int per)
        {
            this.page = mainHeader;
            this.templateHeader = headers;
            this.percentage = per;
            rule = r;
            isvote = isVote;
            int questionCount = 0;
            string metaTitle = null;
            bool isWaitingForModerator = true;
            for (int i = 0; i < templete.Count; i++)
            {
                if (templete[i] == "*headerhtmlcode*" && headers[i] == "Page Header")
                {
                    if (!isWaitingForModerator)
                    {
                        metaTitle = findMetatitle();
                        createHeader(metaTitle);
                    }
                } else if (templete[i] == "*posthtmlcode*" && headers[i] == "Poster Header")
                {
                    initializeNewQuestion();
                    i++;
                    string question = "";
                    while (!templete[i].Contains("question"))
                    {
                        
                        if (templete[i].Contains("%"))
                        {
                            if (isYes())
                            {
                                string key = templete[i].Substring(1, templete[i].Length - 2);
                                var res = findByKey(key);
                                question += res[Util.rand.Next(0, res.Count)].value + " ";
                            }
                            i++;
                            continue;
                        }
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
                    string q = findQuestion();
                    if (isWaitingForModerator)
                    {
                        metaTitle = findMetatitle();
                        createHeader(metaTitle);
                        isWaitingForModerator = false;
                    }
                    else
                    {
                        metaTitle = findMetatitle();
                    }
                    question += q;
                    while (headers[i + 1] == "Poster")
                    {
                        i++;
                        if (!templete[i].Contains("*metatitle*"))
                        {
                            if (templete[i].Contains("%"))
                            {
                                if (isYes())
                                {
                                    string key = templete[i].Substring(1, templete[i].Length - 2);
                                    var res = findByKey(key);
                                    question += res[Util.rand.Next(0, res.Count)].value + " ";
                                }
                                continue;
                            }
                            var rc = findByKey(templete[i]);
                            question += " " + rc[Util.rand.Next(0, rc.Count)].value ;
                        }

                    }
                    createQuestion(question, metaTitle);
                }
                else if (templete[i] == "*replyhtmlcode*")
                {
                    i++;
                    answerState = null;
                    string answer = "";
                    while (!templete[i].Contains("answer"))
                    {
                        if (templete[i].Contains("%"))
                        {
                            if (isYes())
                            {
                                string key = templete[i].Substring(1, templete[i].Length - 2);
                                var res = findByKey(key);
                                answer += res[Util.rand.Next(0, res.Count)].value + " ";
                            }
                            i++;
                            continue;
                        }
                        var rc = findByKey(templete[i]);
                        answer += rc[Util.rand.Next(0, rc.Count)].value + " ";
                        i++;
                    }

                    int votes = 0;
                    string a = findAnswer(ref votes);
                    answer += " " + a;
                    while (headers[i + 1] == "Reply")
                    {
                        i++;
                        if (!templete[i].Contains("*metatitle*"))
                        {
                            if (templete[i].Contains("%"))
                            {
                                if (isYes())
                                {
                                    string key = templete[i].Substring(1, templete[i].Length - 2);
                                    var res = findByKey(key);
                                    answer += res[Util.rand.Next(0, res.Count)].value + " ";
                                }
                                continue;
                            }
                            var rc = findByKey(templete[i]);
                            answer += " " + rc[Util.rand.Next(0, rc.Count)].value ;
                        }

                    }
                    createAnswer(answer, metaTitle, votes);
                }
                else if (templete[i] == "*posterreplyback*")
                {
                    bool doReply = false;
                    if (headers[i].Contains("%"))
                    {
                        if (isYes())
                        {
                            doReply = true;
                        }
                    }
                    else
                    {
                        doReply = true;
                    }
                    if (doReply)
                    {
                        var replies = findByKey("*posterreplyback*");
                        var re = replies[Util.rand.Next(0, replies.Count)];
                        prevReply = clearAllTag(re.value.Replace("*repliername*", replier.value));
                        createReplyBack(prevReply, metaTitle, prevAnswer ,questioner, replier);
                    }

                    canReplierreplyback = true;
                }
                else if (templete[i] == "*replierreplyback*")
                {
                    bool doReply = false;
                    if (headers[i].Contains("%"))
                    {
                        if (isYes())
                        {
                            doReply = true;
                        }
                    }
                    else
                    {
                        doReply = true;
                    }

                    if (doReply && canReplierreplyback)
                    {
                        var replies = findByKey("*replierreplyback*");
                        var re = replies[Util.rand.Next(0, replies.Count)];
                        createReplyBack(clearAllTag(re.value.Replace("*postername*", replier.value)), metaTitle, prevReply, replier, questioner);
                    }

                    canReplierreplyback = false;

                }
                else if (templete[i].Contains("*footerhtmlcode*"))
                {
                    
                    createFooter();
                }
            }

            return page;
        }
        private void initializeNewQuestion()
        {
            questionState = null;
            questionIndex = -1;
            questionRule = new Dictionary<string, string>();
            usedReplacers = new Dictionary<string, Replacer>();
            listOfReplacers = new List<Replacer>();
            questioner = null;
            replier = null;
            canReplierreplyback = false;
            prevReply = null;
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
            var pos = findPosterOrReplier(posters);
            questioner = pos;
            poster = pos.value;
            copy = copy.Replace("<*postername*>", poster);
            copy = copy.Replace("<*date*>", Util.currentDate.ToString("ddd, dd MMM yyy HH:mm:ss tt"));
            Util.currentDate = Util.currentDate.AddMinutes(Util.rand.Next(10, 7200));
            pos.variant[0] = clearAllTag(pos.variant[0]);
            pos.variant[1] = clearAllTag(pos.variant[1]);
            pos.variant[2] = clearAllTag(pos.variant[2]);
            copy = copy.Replace("Colleges and&nbsp;<*level*>", "Level: " + pos.variant[0]);
            copy = copy.Replace("<*location*>", pos.variant[1]);
            copy = copy.Replace("Schools", "Posts: " + pos.variant[2]);
            copy = copy.Replace("Votes", "");
            questioner = pos;
            this.page = page + copy;
        }

        private Replacer findPosterOrReplier(List<Replacer> list)
        {
            string state = null;
            if (questionState != null)
            {
                state = questionState.value;
            } else if (answerState != null)
            {
                state = answerState.value;
            }

            if (state != null)
            {
                List<Replacer> replacers = new List<Replacer>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].variant[1].ToLower() == state.ToLower())
                    {
                        replacers.Add(list[i]);
                    }
                }
                if (replacers.Count > 0)
                {
                    return replacers[Util.rand.Next(0, replacers.Count)];
                }
                throw new Exception("Same " + state + " state poster or replier not found");
            }
            else
            {
                return list[Util.rand.Next(0, list.Count)];
            }
        }

        private void createAnswer(string answer, string metatitle, int votes)
        {
            string copy = posthtmlcode;
            copy = copy.Replace("<*metatitle*>", "Re: " + metatitle);
            copy = copy.Replace("<*question#>", answer);
            copy = copy.Replace("<*greeting*>", "");
            var posters = findByKey("*repliername*");
            var pos = findPosterOrReplier(posters);
            poster = pos.value;
            copy = copy.Replace("<*postername*>", poster);
            copy = copy.Replace("<*date*>", Util.currentDate.ToString("ddd, dd MMM yyy HH:mm:ss tt"));
            Util.currentDate = Util.currentDate.AddMinutes(Util.rand.Next(10, 7200));
            pos.variant[0] = clearAllTag(pos.variant[0]);
            pos.variant[1] = clearAllTag(pos.variant[1]);
            pos.variant[2] = clearAllTag(pos.variant[2]);
            copy = copy.Replace("Colleges and&nbsp;<*level*>", "Level: " + pos.variant[0]);
            copy = copy.Replace("<*location*>", pos.variant[1]);
            copy = copy.Replace("Schools", "Posts: " + pos.variant[2]);
            if (isvote)
            {
                copy = copy.Replace("Votes", "Votes: " + votes);
            }
            else
            {
                copy = copy.Replace("Votes", "");
            }

            replier = pos;
            prevAnswer = answer;
            this.page = page + copy;
        }

        private void createReplyBack(string reply, string metatitle, string question, Replacer poster, Replacer questioner)
        {
            string copy = replybackhtml;
            copy = copy.Replace("<*metatitle*>", "Re: " + metatitle);
            copy = copy.Replace("<*question#>", question);
            copy = copy.Replace("<*posterreplyback*>", reply);
            copy = copy.Replace("<*postername1*>", questioner.value);
            copy = copy.Replace("<*postername*>", poster.value);
            copy = copy.Replace("<*date*>", Util.currentDate.ToString("ddd, dd MMM yyy HH:mm:ss tt"));
            Util.currentDate = Util.currentDate.AddMinutes(Util.rand.Next(10, 7200));
            copy = copy.Replace("<*level*>",  poster.variant[0]);
            copy = copy.Replace("<*location*>", poster.variant[1]);
            copy = copy.Replace("<*Postcount*>", poster.variant[2]);
            
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

            if (list.Count == 0)
            {
                throw new Exception("*"+ key +"* is not present or have less in count");
            }

            return list;
        }

        private string findQuestion()
        {
            if (rule.questions.Count == 0)
            {
                throw  new Exception("Does not have enough question");
            }
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

        private string findAnswer(ref int votes)
        {
            if (answerList != null && answerList.Count > 0)
            {
                int i = Util.rand.Next(0, answerList.Count);
                Answer a = answerList[i];
                votes = a.voteCount;
                answerList.RemoveAt(i);
                return clearAllTag(a.value);
            }
            else
            {
                throw new Exception("Does not have enough answer for question " + questionIndex);
            }
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
                            if (key.Contains("state"))
                            {
                                questionState = rep;
                            }
                        }
                        else
                        {
                            if (key.Contains("state"))
                            {
                                answerState = rep;
                            }
                        }

                        usedReplacers[matches[i].Value] = rep;
                        listOfReplacers.Add(rep);
                        //rule.replacers.Remove(rep);
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

        private bool isYes()
        {
            int randomNumber = Util.rand.Next(0, 100);

            if (randomNumber < percentage)
            {
                return true;
            }

            return false;
        }
    }
}
