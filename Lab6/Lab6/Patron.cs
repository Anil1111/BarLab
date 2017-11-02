using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
   public class Patron
    {
        //EVENT LÖSNING:
            //Vi behöver ett event som Patron tittar på:
            //När Patron har fått sin öl och dequeueas från barQueuen så ska Patron.SitDown() triggas. 

        public string Name { get; set; }
       
        private Action<string> Callback;
        private Action<Chair> FreeChairStack;
        private Action<Patron> PatronChairQueue;
        
        public Patron(string name)
        {
            this.Name = name;
        }
        
        public void SitDown()
        {

        }
    }
}
