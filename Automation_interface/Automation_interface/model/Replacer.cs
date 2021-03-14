using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automation_interface.model
{
    class Replacer
    {
        public string tagInhtml;
        public string value;
        public bool isOnlyOncesUse = false;
        public List<string> variant = new List<string>();

        public bool equal(Replacer res)
        {
            if (this.tagInhtml == res.tagInhtml && this.value == res.value)
            {
                return true;
            }

            return false;
        }
    }
}
