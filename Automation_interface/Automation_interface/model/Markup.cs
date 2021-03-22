using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Automation_interface.model
{
    class Markup
    {
        private string page;
        private JObject pageJObject;
        private JObject mainEntry;
        private JObject acceptedAnswer;
        private JArray suggestedAnswer;
        private int answerCount;
        private bool isCreatedAllready = false;

        public Markup()
        {
            page += "<html><head>";
            pageJObject = new JObject();
            mainEntry = new JObject();
            pageJObject.Add("@context", "https://schema.org");
            pageJObject.Add("@type", "QAPage");
            acceptedAnswer = null;
            answerCount = 0;
            suggestedAnswer = new JArray();
        }

        public void addTile(string title)
        {
            page += "<title> " + title + "</title>";
        }

        public void addQuestion(string question, string date, string posterName)
        {
            mainEntry.Add("@type", "Question");
            mainEntry.Add("name", question);
            mainEntry.Add("text", question);
            mainEntry.Add("answerCount", "0");
            mainEntry.Add("dateCreated", date);
            //mainEntry.Add("dateCreated", date);
            JObject auther = new JObject();
            auther.Add("@type", "Person");
            auther.Add("name", posterName);
            mainEntry.Add("author", auther);
        }

        public void addAcceptedAnswer(string answer, string date, string posterName, int votes, string url)
        {
            answerCount++;
            acceptedAnswer = new JObject();
            acceptedAnswer.Add("@type", "Answer");
            acceptedAnswer.Add("text", "question");
            acceptedAnswer.Add("answerCount", "");
            acceptedAnswer.Add("dateCreated", date);
            acceptedAnswer.Add("upvoteCount", votes);
            acceptedAnswer.Add("url", url + "#accepted");
            JObject auther = new JObject();
            auther.Add("@type", "Person");
            auther.Add("name", posterName);
            acceptedAnswer.Add("author", auther);
        }

        public void addSuggestedAnswer(string answer, string date, string posterName, int votes, string url)
        {
            answerCount++;
            JObject answerJObject = new JObject();
            answerJObject.Add("@type", "Answer");
            answerJObject.Add("text", "question");
            answerJObject.Add("answerCount", "");
            answerJObject.Add("dateCreated", date);
            answerJObject.Add("upvoteCount", votes);
            answerJObject.Add("url", url + "#suggested" + (answerCount - 1));
            JObject auther = new JObject();
            auther.Add("@type", "Person");
            auther.Add("name", posterName);
            answerJObject.Add("author", auther);
            suggestedAnswer.Add(answerJObject);
        }


        public string getMarkup()
        {
            if (isCreatedAllready)
            {
                return page;
            }

            mainEntry["answerCount"] = answerCount;
            page += File.ReadAllText("HTMLs/schema.txt");
            page += "<script type='application/ld+json'>";
            if (acceptedAnswer != null)
            {
                mainEntry.Add("acceptedAnswer", acceptedAnswer);
            }
            if (suggestedAnswer.Count > 0)
            {
                mainEntry.Add("suggestedAnswer", suggestedAnswer);
            }
            pageJObject.Add("mainEntity", mainEntry);
            page += pageJObject.ToString();
            page += "</script></head><body>";
                    //"</body></html>";
            isCreatedAllready = true;
            return page;
        }

    }
}
