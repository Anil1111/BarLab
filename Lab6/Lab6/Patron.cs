using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
   public class Patron
    {
        public string Name { get; set; }

        public Patron(string name)
        {
            this.Name = name;
        }

        public string EnteredBar()
        {
            return Name + "ekjfe";
        }
    }
}
