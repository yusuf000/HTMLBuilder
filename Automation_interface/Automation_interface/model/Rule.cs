using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_interface.model
{
    class Rule
    {
        public List<Replacer> replacers = new List<Replacer>();
        public List<Question> questions = new List<Question>();
        public List<Answer> answers = new List<Answer>();
        public List<MetaTitle> metaTitles = new List<MetaTitle>();

        public Question findQuestion(int index)
        {
            Question q = questions[index];
            questions.RemoveAt(index);
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
            
            return tempList[Util.rand.Next(0, tempList.Count)];
        }

        public List<Answer> FindAnswerList(int serialNumber)
        {
            List<Answer> ans = new List<Answer>();
            for (int i = 0; i < answers.Count; i++)
            {
                if (answers[i].serialNumber == serialNumber)
                {
                    ans.Add(answers[i]);
                }
            }

            return ans;
        }
    }
}
