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

        public static List<Replacer> findUniqueList(List<Replacer> replacers)
        {
            List<Replacer> unique = new List<Replacer>();
            for (int i = 0; i < replacers.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < unique.Count; j++)
                {
                    if (replacers[i].equal(unique[j]))
                    {
                        found = true;
                        break;;
                    }
                }

                if (!found)
                {
                    unique.Add(replacers[i]);
                }
            }
            return unique;
        }

    }
}
