using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using CsvHelper;
using OfficeOpenXml;
using CsvReader = LumenWorks.Framework.IO.Csv.CsvReader;

namespace Automation_interface.model
{
    class Util
    {
        public static Random rand = new Random();
        public static DateTime currentDate;
        public static List<Replacer> usedValue = new List<Replacer>();
        public static List<Replacer> usedForThisPage = new List<Replacer>();
        public static List<PageBuilder> pages = new List<PageBuilder>();
        public static List<PageBuilder> allPagesOfProgram = new List<PageBuilder>();
        public static Dictionary<string, int> posterPostCount = new Dictionary<string, int>();
        public static string systemName = "";
        public static bool continuousOutputCSV = false;

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
            FileInfo intialInfo = new FileInfo(fileName);
            var excel = new ExcelPackage(intialInfo);
            foreach (ExcelWorksheet workSheet in excel.Workbook.Worksheets)
            {
                var start = workSheet.Dimension.Start;
                var end = workSheet.Dimension.End;
                for (int row = start.Row; row <= end.Row; row++)
                {
                    var rd = getRow(workSheet, start, end, row);
                    
                    
                    if (rd.Count() > 1)
                    {
                        if (rd[0].Contains("question#"))
                        {
                            string pattern = @"\#(.*?)\#";
                            MatchCollection matches = Regex.Matches(rd[0], pattern);
                            int serial = Int32.Parse(matches[0].Value.Substring(1, (matches[0].Value.Length - 2)));
                            rule.questions.Add(new Question(rd[1].Replace("^", ","), serial));
                        }
                        else if (rd[0].Contains("metatitle"))
                        {
                            string pattern = @"\#(.*?)\#";
                            MatchCollection matches = Regex.Matches(rd[0], pattern);
                            int serial = Int32.Parse(matches[0].Value.Substring(1, (matches[0].Value.Length - 2)));
                            rule.metaTitles.Add(new MetaTitle(rd[1].Replace("^", ","), serial));
                        }
                        else if (rd[0].Contains("answer#"))
                        {
                            string pattern = @"\#(.*?)\#";
                            MatchCollection matches = Regex.Matches(rd[0], pattern);
                            bool accepted = false;
                            string val = matches[0].Value;
                            if (matches[0].Value.Contains("$"))
                            {
                                accepted = isVote;
                                val = matches[0].Value.Replace("$", "");
                            }

                            int serial = Int32.Parse(val.Substring(1, (val.Length - 2)));
                            Answer ans = new Answer(rd[1].Replace("^", ","), serial, accepted);
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
                            if (re.tagInhtml.Contains("!"))
                            {
                                re.tagInhtml = re.tagInhtml.Substring(1, re.tagInhtml.Length - 2);
                                re.isOnlyOncesUse = true;
                            }
                            re.value = rd[1].Replace("^", ",");
                            if (rd.Length > 2)
                            {
                                for (int i = 2; i < rd.Length; i++)
                                {
                                    if (rd[i] == null)
                                        continue;
                                    string s = rd[i].Trim();
                                    if (s.Length > 0)
                                        re.variant.Add(s);
                                }
                            }
                            if (isAvailableKeyWordValue(re))
                            {
                                rule.replacers.Add(re);
                            }
                            else
                            {
                                usedForThisPage.Add(re);
                            }

                            if (re.tagInhtml.Contains("*repliername*") || re.tagInhtml.Contains("*postername*"))
                            {
                                if (!posterPostCount.ContainsKey(re.value))
                                {
                                    posterPostCount[re.value] = Int32.Parse(re.variant[2]);
                                }
                            }
                        }
                        
                    }
                }
                
            }
             
        
            
            
            
            /*using (var rd = new CsvReader(new StreamReader(fileName), false))
            {
                while (rd.ReadNextRecord())
                {
                    if (rd.FieldCount > 1)
                    {
                        if (rd[0].Contains("question#"))
                        {
                            string pattern = @"\#(.*?)\#";
                            MatchCollection matches = Regex.Matches(rd[0], pattern);
                            int serial = Int32.Parse(matches[0].Value.Substring(1, (matches[0].Value.Length - 2)));
                            rule.questions.Add(new Question(rd[1].Replace("^", ","), serial));
                        }
                        else if (rd[0].Contains("metatitle"))
                        {
                            string pattern = @"\#(.*?)\#";
                            MatchCollection matches = Regex.Matches(rd[0], pattern);
                            int serial = Int32.Parse(matches[0].Value.Substring(1, (matches[0].Value.Length - 2)));
                            rule.metaTitles.Add(new MetaTitle(rd[1].Replace("^", ","), serial));
                        }
                        else if (rd[0].Contains("answer#"))
                        {
                            string pattern = @"\#(.*?)\#";
                            MatchCollection matches = Regex.Matches(rd[0], pattern);
                            bool accepted = false;
                            string val = matches[0].Value;
                            if (matches[0].Value.Contains("$"))
                            {
                                accepted = isVote;
                                val = matches[0].Value.Replace("$", "");
                            }

                            int serial = Int32.Parse(val.Substring(1, (val.Length - 2)));
                            Answer ans = new Answer(rd[1].Replace("^", ","), serial, accepted);
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
                            if (re.tagInhtml.Contains("!"))
                            {
                                re.tagInhtml = re.tagInhtml.Substring(1, re.tagInhtml.Length - 2);
                                re.isOnlyOncesUse = true;
                            }
                            re.value = rd[1].Replace("^", ",");
                            if (rd.FieldCount > 2)
                            {
                                for (int i = 2; i < rd.FieldCount; i++)
                                {
                                    string s = rd[i].Trim();
                                    if (s.Length > 0)
                                        re.variant.Add(s);
                                }
                            }
                            if (isAvailableKeyWordValue(re))
                            {
                                rule.replacers.Add(re);
                            }
                            else
                            {
                                usedForThisPage.Add(re);
                            }

                            if (re.tagInhtml.Contains("*repliername*") || re.tagInhtml.Contains("*postername*"))
                            {
                                if (!posterPostCount.ContainsKey(re.value))
                                {
                                    posterPostCount[re.value] = Int32.Parse(re.variant[2]);
                                }
                            }
                        }
                    }
                }
            }*/
            return rule;
        }

        private static string[] getRow(ExcelWorksheet workSheet, ExcelCellAddress start, ExcelCellAddress end,  int row)
        {
            List<string> list = new List<string>();
            for (int i = start.Column; i <= end.Column; i++)
            {
                Object cellValue = workSheet.Cells[row, i].Value;
                if (cellValue != null)
                {
                    list.Add(workSheet.Cells[row, i].Text);
                }
            }
            return list.ToArray();
        }

        private static bool isAvailableKeyWordValue(Replacer rep)
        {
            for (int i = 0; i < usedValue.Count; i++)
            {
                if (rep.equal(usedValue[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static void loadUsedKeywordValue() {
            using (var rd = new CsvReader(new StreamReader(@"UsedKeyword/usedKeyword.csv"), true)) {
                string[] headers = rd.GetFieldHeaders();
                if (headers.Length > 0) {
                    while (rd.ReadNextRecord())
                    {
                        for (int i = 0; i < rd.FieldCount; i++)
                        {
                            if (!String.IsNullOrEmpty(rd[i]))
                            {
                                Replacer re = new Replacer();
                                re.tagInhtml = headers[i];
                                re.value = rd[i];
                                usedValue.Add(re);
                            }
                        }
                    }
                }
                
            }
        }

        public static void saveUsedKeyword()
        {
            var unique = Replacer.findUniqueList(usedValue);
            usedValue = unique;
            if (usedValue.Count > 0)
            {
                var mem = new MemoryStream();
                var writer = new StreamWriter(mem);
                var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
                string[] headers = findHeader();
                for (int i = 0; i < headers.Length; i++)
                {
                    csvWriter.WriteField(headers[i]);
                }
                csvWriter.NextRecord();
                while (usedValue.Count != 0)
                {
                    for (int i = 0; i < headers.Length; i++)
                    {
                        csvWriter.WriteField(getUsedTagValue(headers[i]));
                    }
                    csvWriter.NextRecord();
                }
                writer.Flush();
                var result = Encoding.UTF8.GetString(mem.ToArray());
                File.WriteAllText(@"UsedKeyword/usedKeyword.csv", result);
            }
        }

        private static string getUsedTagValue(string tag)
        {
            for (int i = 0; i < usedValue.Count; i++)
            {
                if (usedValue[i].tagInhtml == tag)
                {
                    var r = usedValue[i];
                    usedValue.Remove(r);
                    return r.value;

                }
            }

            return "";
        }

        private static string[] findHeader()
        {
            HashSet<string> header = new HashSet<string>();
            for (int i = 0; i < usedValue.Count; i++)
            {
                header.Add(usedValue[i].tagInhtml);
            }

            return header.ToArray();
        }

        public static void createAndAddPaginator()
        {
            int totalPage = pages.Count;
            for (int i = 0; i < totalPage; i++)
            {
                string p = paginator(totalPage, i + 1);
                pages[i].addPaginator(p);
            }

        }

        public static string paginator(int totalPage, int currentPage)
        {
            string paginator = "<nav class='text-center'>";
            paginator += "<ul class='pagination'>";
            paginator += "<ul class='pagination justify-content-center'>";
            if (currentPage != 1)
            {
                paginator += "<li class='page-item'><a class='page-link' tabindex=' - 1' href='" + pages[currentPage-2].getUrl() +"'>Previous</a></li>";
            }

            int startPageNumber, lastPageNumber;
            if (currentPage - 2 < 1)
            {
                startPageNumber = 1;
                lastPageNumber = (startPageNumber + 4 ) > totalPage ? totalPage : (startPageNumber + 4);
            }
            else if(currentPage + 2 > totalPage)
            {
                lastPageNumber = totalPage;
                startPageNumber = (lastPageNumber - 4) < 1 ? 1: (lastPageNumber - 4);
            }
            else
            {
                startPageNumber = currentPage - 2;
                lastPageNumber = currentPage + 2;
            }

            for (int i = startPageNumber; i <= lastPageNumber; i++)
            {
                if (i == currentPage)
                {
                    paginator += "<li class='active'><a class='page-link' style='background-color: #ffa500; border-color: #FFA500;' href='" + pages[i-1].getUrl() + "'>" + i + "</a></li>";
                }
                else
                {
                    paginator += "<li class='page-item'><a class='page-link' href='" + pages[i - 1].getUrl() + "'>" + i + "</a></li>";
                }
            }

            if (currentPage != totalPage)
            {
                paginator += "<li class='page-item'><a class='page-link' href='" + pages[currentPage].getUrl() + "'>Next</a></li>";
            }

            paginator += "</ul></ul></nav>";
            return paginator;

        }

        public static List<List<string>> getRules(string fileName)
        {
            List<List<string>> rules = new List<List<string>>();
            FileInfo intialInfo = new FileInfo(fileName);
            var excel = new ExcelPackage(intialInfo);
            foreach (ExcelWorksheet workSheet in excel.Workbook.Worksheets)
            {
                for (int i = workSheet.Dimension.Start.Row; i <= workSheet.Dimension.End.Row; i++)
                {
                    List<string> rule = new List<string>();
                    for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                    {
                        object cellValue = workSheet.Cells[i, j].Value;
                        if (cellValue == null)
                        {
                            rule.Add("");
                        }
                        else
                        {
                            rule.Add((string)cellValue);
                        }
                    }
                    rules.Add(rule);
                }
            }

            /*foreach (var worksheet in Workbook.Worksheets(fileName))
            {
                foreach (var rd in worksheet.Rows)
                {
                    List<string> rule = new List<string>();
                    foreach (var cell in rd.Cells)
                    {
                        if (cell == null)
                        {
                            rule.Add("");
                        }
                        else
                        {
                            rule.Add(cell.Text);
                        }
                    }
                    rules.Add(rule);
                }
            }*/

            return rules;
        }

        public static void setSystemName(string[] headers, string[] rules)
        {
            string s = "";
            for (int i = 0; i < headers.Length; i++)
            {
                if ("System name" == headers[i])
                {
                    s = rules[i];
                    break;
                }
            }

            systemName = s;
        }

    }
}
