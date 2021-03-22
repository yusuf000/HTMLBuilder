using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_interface.model
{
    class KeyWords
    {
        public List<Replacer> replacers = new List<Replacer>();
        public List<Question> questions = new List<Question>();
        public List<Answer> answers = new List<Answer>();
        public List<MetaTitle> metaTitles = new List<MetaTitle>();

        public Question findQuestion(int index)
        {
            List<Question> qq = new List<Question>();
            for (int i = 0; i < questions.Count; i++)
            {
                if (questions[i].serialNumber == index)
                {
                    qq.Add(questions[i]);
                }
            }
            Question q = qq[Util.rand.Next(0, qq.Count)];
            questions.Remove(q);
            return q;
        }

        public MetaTitle FindMetaTitle(int serialNumber)
        {
            List<MetaTitle> tempList = new List<MetaTitle>();
            for (int i = 0; i < metaTitles.Count; i++)
            {
                if (metaTitles[i].serialNumber == serialNumber)
                {
                    tempList.Add(metaTitles[i]);
                }
            }

            if (tempList.Count == 0)
            {
                throw new Exception("No metatile found for question number " + serialNumber);
            }

            return tempList[Util.rand.Next(0, tempList.Count)];
        }

        public List<Answer> FindAnswerList(int serialNumber)
        {
            List<Answer> ans = new List<Answer>();
            for (int i = 0; i < answers.Count; i++)
            {
                if (answers[i].serialNumber == serialNumber && answers[i].isAccepted == false)
                {
                    ans.Add(answers[i]);
                }
            }

            return ans;
        }

        public List<Answer> FindAnswerAcceptedList(int serialNumber)
        {
            List<Answer> ans = new List<Answer>();
            for (int i = 0; i < answers.Count; i++)
            {
                if (answers[i].serialNumber == serialNumber && answers[i].isAccepted == true)
                {
                    ans.Add(answers[i]);
                }
            }

            return ans;
        }
    }
}
