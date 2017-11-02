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
        private Action<Patron> PatronCallback;
        Random random = new Random();
        int maxPatrons = 10;

        List<string> PatronNameList = new List<string>()
            { "Jonas", "Klas", "Göran", "Getrud", "Daniel", "Petra", "Tor", "Styr-Björn", "Greta", "Livingston", "Margret" };

        //Work method
        public void Work(Action<string> Callback, Action<Patron> patronCallback)
        {
            Task.Run(() => {
                this.PatronCallback = patronCallback;
                this.Callback = Callback;

                while (maxPatrons > 0)
                {
                    Thread.Sleep(random.Next(1000, 5000));
                    string patronName = PatronNameList[random.Next(PatronNameList.Count)];
                    Callback($"{patronName} has entered the bar.");
                    patronCallback(new Patron(patronName));
                    maxPatrons--;
                }
            });
        }
    }
}