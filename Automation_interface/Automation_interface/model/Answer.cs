using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_interface.model
{
    class Answer
    {
        public string value;
        public int serialNumber;
        public bool isAccepted;

        public Answer(string v, int s, bool i = false)
        {
            value = v;
            serialNumber = s;
            isAccepted = i;
        }
    }
}
