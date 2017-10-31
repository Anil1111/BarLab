using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Lab6
{
    public class Bouncer
    {
        private Action<string> Callback;

        public string PatronName { get; set; }
        int RemainingPatrons = 10;

        List<string> PatronNameList = new List<string>()
            { "Jonas", "Klas", "Göran", "Getrud", "Daniel", "Petra", "Tor", "Styr-Björn", "Greta", "Livingston", "Margret" };

        //Work method
        public void Work(Action<string> Callback)
        {
            this.Callback = Callback;
            Random rnd = new Random();

            Random random = new Random();
            while (RemainingPatrons >= 0)
            {
                Thread.Sleep(random.Next(3000, 10000));
                Callback(PatronNameList[rnd.Next(PatronNameList.Count)]);
                RemainingPatrons--;             
            }
        }
    }
}