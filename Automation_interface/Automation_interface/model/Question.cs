using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_interface.model
{
    class Question
    {
        public string value;
        public int serialNumber;

        public Question(string s, int i)
        {
            value = s;
            serialNumber = i;
        }
    }
}
