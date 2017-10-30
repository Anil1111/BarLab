using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public class Bouncer
    {
        List<string> PatronNameList = new List<string>()
        { "Jonas", "Klas", "Göran", "Getrud", "Daniel", "Petra", "Tor", "Styr-Björn", "Greta", "Livingston", "Margret" };
        Random rnd = new Random();
        public Patron patron { get; set; }
        
        public void CreatePatron()
        {
            int index = rnd.Next(PatronNameList.Count);
            Patron patron = new Patron(PatronNameList[index]);
            ListPatron.Items.Insert(0, new Patron(PatronNameList[index]));
        }
    }
}
