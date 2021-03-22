using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Automation_interface.model
{
    class PageBuilder
    {
        private string[] templateHeader;
        private string[] ruleTemplate;
        private Markup markup;
        public OutputCSV outputCsv;

        private string mainHeader;
        private string headerhtmlcode;
        private string posthtmlcode;
        private string posthtmlcodeVotes;
        private string replyhtmlcode;
        private string footerhtmlcode;
        private string replybackhtml;
        private string quoteHtml;
        private string banner;

        private int answerCount = 0;
        private string page;
        private string fileName;
        private int questionIndex;
        private string currentQuestion;
        private Replacer questionState = null;
        private Replacer answerState = null;
        private Replacer questioner = null;
        private Replacer replier = null;
        private string poster;
        private bool canReplierreplyback = false;
        private bool isvote;
        private model.KeyWords rule;
        private int percentage;
        private string prevReply;
        private string prevAnswer;
        private Dictionary<string, string> questionRule;
        private Dictionary<string, Replacer> usedReplacers;
        private List<Replacer> listOfReplacers;
        private List<Replacer> usedInAnswer = new List<Replacer>();
        private List<Answer> answerList;
        private int questionCount = 0;
        private string pageUrl;
        private string urlWithouHost;
        
        

        public PageBuilder()
        {
            mainHeader = File.ReadAllText("HTMLs/MainHeader.txt");
            headerhtmlcode = File.ReadAllText("HTMLs/PageheaderHTMLCode.txt");
            posthtmlcode = File.ReadAllText("HTMLs/BodyHtmlCode.txt");
            posthtmlcodeVotes = File.ReadAllText("HTMLs/BodyHtmlCode_votes.txt");
            replyhtmlcode = File.ReadAllText("HTMLs/post_replyhtmlcode.txt");
            footerhtmlcode = File.ReadAllText("HTMLs/footerHtmlCode.txt");
            replybackhtml = File.ReadAllText("HTMLs/postreplyback.txt");
            banner = File.ReadAllText("HTMLs/Banner.txt");
            pageUrl = File.ReadAllText("HTMLs/pagination_URL.txt");
            quoteHtml = File.ReadAllText("HTMLs/quoteHtml.txt");
            markup = new Markup();
            outputCsv = new OutputCSV();
        }

        public string createpage(List<string> templete, model.KeyWords r, bool isVote, string[] headers, int per)
        {
            this.page = mainHeader;
            this.templateHeader = headers;
            this.ruleTemplate = templete.ToArray();
            this.percentage = per;
            rule = r;
            isvote = isVote;
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
                    questionCount++;
                    string question = "";
                    while (!templete[i].Contains("question"))
                    {
                        
                        if (templete[i].Contains("%"))
                        {
                            if (isYes())
                            {
                                string key = templete[i].Substring(1, templete[i].Length - 2);
                                var res = findByKey(key);
                                question += clearAllTag(res[Util.rand.Next(0, res.Count)].value + " ", true);
                            }
                            i++;
                            continue;
                        }
                        var rc = findByKey(templete[i]);
                        question += clearAllTag(rc[Util.rand.Next(0, rc.Count)].value + " ", true);
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
                        if (rule.questions.Count == 0)
                        {
                            throw new Exception("Does not have enough question");
                        }
                        questionIndex = Util.rand.Next(0, rule.questions.Count);
                        questionIndex = rule.questions[questionIndex].serialNumber;
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
                                    question += clearAllTag(res[Util.rand.Next(0, res.Count)].value + " ", true);
                                }
                                continue;
                            }
                            var rc = findByKey(templete[i]);
                            question += " " + clearAllTag(rc[Util.rand.Next(0, rc.Count)].value, true) ;
                        }

                    }
                    createQuestion(question, metaTitle);
                }
                else if (templete[i] == "*replyhtmlcode*")
                {
                    bool isQuote = false;
                    usedInAnswer = new List<Replacer>();
                    i++;
                    answerState = null;
                    string answer = "";
                    while (!templete[i].Contains("answer"))
                    {
                        if (templete[i].Contains("quote"))
                        {
                            if (templete[i].Contains("%"))
                            {
                                if (isYes())
                                {
                                    isQuote = true;
                                }
                            }
                            else
                            {
                                isQuote = true;
                            }
                            i++;
                            continue;
                        }
                        else if (templete[i].Contains("%"))
                        {
                            if (isYes())
                            {
                                string key = templete[i].Substring(1, templete[i].Length - 2);
                                var res = findByKey(key);
                                answer += clearAllTag(res[Util.rand.Next(0, res.Count)].value + " ");
                            }
                            i++;
                            continue;
                        }
                        var rc = findByKey(templete[i]);
                        answer += clearAllTag(rc[Util.rand.Next(0, rc.Count)].value + " ");
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
                                    answer += clearAllTag(res[Util.rand.Next(0, res.Count)].value + " ");
                                }
                                continue;
                            }
                            var rc = findByKey(templete[i]);
                            answer += " " + clearAllTag(rc[Util.rand.Next(0, rc.Count)].value) ;
                        }

                    }
                    createAnswer(answer, metaTitle, votes, isQuote);
                    answerCount++;
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
                        canReplierreplyback = true;
                    }
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
            answerCount = 0;
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
            createUrl(metaTile);
            if(questionCount == 1)
                markup.addTile(metaTile);
        }

        private void createFooter()
        {
            
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
            string date = Util.currentDate.ToString("ddd, dd MMM yyy HH:mm:ss tt");
            copy = copy.Replace("<*date*>", date);
            Util.currentDate = Util.currentDate.AddMinutes(Util.rand.Next(10, 7200));
            pos.variant[0] = clearAllTag(pos.variant[0]);
            pos.variant[1] = clearAllTag(pos.variant[1]);
            pos.variant[2] = clearAllTag(pos.variant[2]);
            copy = copy.Replace("Colleges and&nbsp;<*level*>", "Level: " + pos.variant[0]);
            copy = copy.Replace("<*location*>", pos.variant[1]);
            copy = copy.Replace("Schools", "Posts: " + Util.posterPostCount[poster]++ );
            copy = copy.Replace("Votes", "");
            questioner = pos;
            currentQuestion = question;
            if (questionCount == 1)
            {
                markup.addQuestion(question, date, poster);
                setOutputCsvAttribute(metaTitle, question);
            }
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

        private void createAnswer(string answer, string metatitle, int votes, bool isQuote = false)
        {
            string copy;
            if (isvote)
            {
                copy = posthtmlcodeVotes;
            }
            else
            {
                copy = posthtmlcode;
            }

            if (isQuote)
            {
                answer = quoteHtml.Replace("*posterName*", questioner.value).Replace("*question*", currentQuestion) + "<br><br><br>" + answer ;
            }

            copy = copy.Replace("<*metatitle*>", "Re: " + metatitle);
            copy = copy.Replace("<*question#>", answer);
            copy = copy.Replace("<*greeting*>", "");
            var posters = findByKey("*repliername*");
            var pos = findPosterOrReplier(posters);
            poster = pos.value;
            copy = copy.Replace("<*postername*>", poster);
            string date = Util.currentDate.ToString("ddd, dd MMM yyy HH:mm:ss tt");
            copy = copy.Replace("<*date*>", date);
            Util.currentDate = Util.currentDate.AddMinutes(Util.rand.Next(10, 7200));
            pos.variant[0] = clearAllTag(pos.variant[0]);
            pos.variant[1] = clearAllTag(pos.variant[1]);
            pos.variant[2] = clearAllTag(pos.variant[2]);
            copy = copy.Replace("Colleges and&nbsp;<*level*>", "Level: " + pos.variant[0]);
            copy = copy.Replace("<*location*>", pos.variant[1]);
            copy = copy.Replace("Schools", "Posts: " + Util.posterPostCount[poster]++);
            if (isvote)
            {
                copy = copy.Replace("*voteCount*", "" + votes);
            }
            else
            {
                copy = copy.Replace("Votes", "");
            }

            replier = pos;
            prevAnswer = answer;
            if (questionCount == 1)
            {
                if (isvote)
                {
                    if (answerCount == 0)
                    {
                        markup.addAcceptedAnswer(answer, date, poster, votes, pageUrl);
                    }
                    else
                    {
                        markup.addSuggestedAnswer(answer, date, poster, votes, pageUrl);
                    }
                }
                else
                {
                    markup.addSuggestedAnswer(answer, date, poster, 0, pageUrl);
                }
            }

            

            this.page = page + copy;
        }

        private void createReplyBack(string reply, string metatitle, string question, Replacer poster, Replacer questioner)
        {
            string copy = replybackhtml;
            copy = copy.Replace("<*metatitle*>", metatitle);
            copy = copy.Replace("<*question#>", question);
            copy = copy.Replace("<*posterreplyback*>", reply);
            copy = copy.Replace("<*postername1*>", questioner.value);
            copy = copy.Replace("<*postername*>", poster.value);
            copy = copy.Replace("<*date*>", Util.currentDate.ToString("ddd, dd MMM yyy HH:mm:ss tt"));
            Util.currentDate = Util.currentDate.AddMinutes(Util.rand.Next(10, 7200));
            copy = copy.Replace("<*level*>",  poster.variant[0]);
            copy = copy.Replace("<*location*>", poster.variant[1]);
            copy = copy.Replace("<*Postcount*>", "" + Util.posterPostCount[poster.value]++);
            
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

            if (list.Count == 0) {
                if (retriveUsedKeyword(key) > 0)
                {
                    return findByKey(key);
                }
                throw new Exception("*"+ key +"* is not present in keyword csv");
            }

            return list;
        }

        private string findQuestion()
        {
            if (rule.questions.Count == 0)
            {
                throw  new Exception("Does not have enough question");
            }
            Question q = rule.findQuestion(questionIndex);
            answerList = rule.FindAnswerList(questionIndex);
            return clearAllTag(q.value, true);
        }

        private string findMetatitle()
        {
            MetaTitle m = rule.FindMetaTitle(questionIndex);
            return clearAllTag(m.value, true);
        }

        private string findAnswer(ref int votes)
        {
            

            if (answerList != null && answerList.Count > 0)
            {
                Answer a;
                if (answerCount == 0 && this.isvote)
                {
                    var temp = rule.FindAnswerAcceptedList(questionIndex);
                    if(temp.Count == 0)
                        throw new Exception("Does not have accepted answer for question " + questionIndex);
                    a = temp[Util.rand.Next(0, temp.Count)];
                }
                else
                {
                   int i = Util.rand.Next(0, answerList.Count);
                   a = answerList[i];
                   answerList.RemoveAt(i);
                }
                
                votes = a.voteCount;
                
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
                Dictionary<string, bool> isUsedEarly = new Dictionary<string, bool>();
                for (int i = 0; i < matches.Count; i++)
                {
                    if (isUsedEarly.ContainsKey(matches[i].Value))
                    {
                        continue;
                    }

                    if (questionRule.ContainsKey(matches[i].Value))
                    {
                        text = text.Replace(matches[i].Value, questionRule[matches[i].Value]);
                        continue;
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
                            retriveUsedKeyword(key);
                            res = filterUsed(res);
                            if (res.Count < 1)
                            {

                                throw new Exception(key + " This is no other value present in rule file please check carefully.");
                            }

                        }
                        var rep = res[Util.rand.Next(0, res.Count)];
                        text = text.Replace(matches[i].Value, rep.value);
                        if (isQuestion)
                        {
                            questionRule[matches[i].Value] = rep.value;
                        }
                        else
                        {
                            usedInAnswer.Add(rep);
                        }
                        listOfReplacers.Add(rep);
                        if (rep.isOnlyOncesUse)
                        {
                            rule.replacers.Remove(rep);
                            Util.usedValue.Add(rep);
                            Util.usedForThisPage.Add(rep);
                        }
                    }
                    else
                    {
                        key = matches[i].Value;
                        var re = findByKey(key);
                        if (re.Count < 1)
                        {
                            if (retriveUsedKeyword(key) > 0)
                            {
                                re = findByKey(key);
                            }
                            else
                            {
                                throw new Exception(key + " This is not present in rule file please check carefully.");
                            }
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
                        if (rep.isOnlyOncesUse)
                        {
                            rule.replacers.Remove(rep);
                            Util.usedValue.Add(rep);
                            Util.usedForThisPage.Add(rep);
                        }
                    }

                    isUsedEarly[matches[i].Value] = true;
                }
                matches = Regex.Matches(text, pattern);
            }

            return text;
        }

        private int retriveUsedKeyword(string tag)
        {
            int count = 0;
            var uniqueList = Replacer.findUniqueList(Util.usedForThisPage);
            for (int i = 0; i < uniqueList.Count; i++)
            {
                if (tag == uniqueList[i].tagInhtml)
                {
                    count++;
                    rule.replacers.Add(uniqueList[i]);
                }
            }

            Util.usedForThisPage = uniqueList;
            return count;
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
                for (int j = 0; j < usedInAnswer.Count; j++)
                {
                    if (res[i].equal(usedInAnswer[j]))
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

        public void addPaginator(string paginator)
        {
            this.page += paginator;
        }

        public string getPagHtml()
        {
            return  markup.getMarkup() + this.page + "</body></html>";
        }

        public string getUrl()
        {
            return this.pageUrl;
        }

        private void createUrl(string metatitle)
        {
            metatitle = metatitle.Replace("-", "");
            string url = metatitle.Replace(" ", "-")+ "-" + Util.rand.Next(10000, 1000000);
            url = url.Replace("@", "");
            url = url.Replace("?", "");
            url = url.Replace(".", "");
            url = url.Replace("'", "");
            url = url.Replace(",", "");
            url = url.Replace("--", "-");
            url = url.Replace("!", "");
            string temp = File.ReadAllText("HTMLs/paginationremove.txt");
            string[] chars = temp.Split(',');
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i].Length > 0)
                {
                    url = url.Replace(chars[i], "");
                }
                
            }
            urlWithouHost = url;
            this.pageUrl += url;

        }

        private void setOutputCsvAttribute(string metaTile, string question)
        {
            
            
            string titleKeyword = findFromRule("Title");
            string title;
            if (titleKeyword == "*metatitle*")
            {
                title = metaTile;
            }
            else
            {
                var res = findByKey(titleKeyword);
                title = clearAllTag(res[Util.rand.Next(0, res.Count)].value);
            }
            string metaKeywords = removeMeta(removeHtmlTag(question).Split(' '));
            string metaDescription = "";
            string[] questionWord = question.Split(' ');
            if (questionWord.Length > 30)
            {
                for (int i = 0; i < 30; i++)
                {
                    metaDescription += questionWord[i] + " ";
                }
            }
            else
            {
                metaDescription = question;
            }
            outputCsv.setAllAtribute(Util.systemName,"", title, metaKeywords, metaDescription,metaTile, urlWithouHost);
        }

        private string removeMeta(string[] metawords)
        {
            int len = metawords.Length > 30 ? 30 : metawords.Length;
            List<string> newMeta = new List<string>();
            string[] removeList = File.ReadAllText(@"HTMLs/metalist.txt").Split(',');
            for (int i = 0; i < len; i++)
            {
                bool found = false;
                for (int j = 0; j < removeList.Length; j++)
                {
                    if (metawords[i] == removeList[j])
                    {
                        found = true;
                        break;
                    }
                }

                if (!found && metawords[i].Length > 0)
                {
                    newMeta.Add(metawords[i]);
                }
            }

            string s = "";
            int iter = 0;
            for (; iter < newMeta.Count - 1; iter++)
            {
                s += newMeta[iter] + ", ";
            }

            s += newMeta[iter];
            return s;
        }

        private string removeHtmlTag(string x)
        {
            string s = x;
            string pattern = @"\<(.*?)\>";
            //string pattern = @"a.e";
            MatchCollection matches = Regex.Matches(x, pattern);
            for (int i = 0; i < matches.Count; i++)
            {
                s.Replace(matches[i].Value, "");
            }
            return s;
        }

        private string findFromRule(string name)
        {
            string s = "";
            for (int i = 0; i < templateHeader.Length; i++)
            {
                if (name == templateHeader[i])
                {
                    s = ruleTemplate[i];
                    break;
                }
            }
            return s;
        }

       

        public string getUrlWithoutHost()
        {
            return urlWithouHost;
        }

        public string getMarkup()
        {
            return markup.getMarkup();
        }

        public void addBanner()
        {
            this.page = page + banner;
        }

        public void addLocation(string fName)
        {
            this.fileName = fName;
        }

        public string getFileName()
        {
            return fileName;
        }
    }
}
