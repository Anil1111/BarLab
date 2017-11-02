using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace Lab6
{
    public class Bartender
    {
        private Action<string> Callback;
        private ConcurrentQueue<Patron> PatronQueue;
        private ConcurrentStack<Glass> GlassStack;

        public void Work(ConcurrentQueue<Patron> patronQueue, Action<string> callback, ConcurrentStack<Glass> glassStack, bool isOpen)
        {
            this.Callback = callback;
            this.PatronQueue = patronQueue;
            this.GlassStack = glassStack;

            Task.Run(() => 
            {
                Thread.Sleep(1000);
                if (!patronQueue.IsEmpty)
                {
                    //Gå till hyllan och hämta ett glas -> 3 sek
                    if (GetGlass(glassStack))
                    {
                        //Häll upp ett glas till kunden -> 3 sek
                        callback($"Bartender is pouring up a beer for {((Patron)patronQueue.First()).Name}");
                    }
                }
                else if (patronQueue.IsEmpty)
                {
                    callback("The Bartender is waiting for Patrons.");
                }
            });
        }

        public bool GetGlass(ConcurrentStack<Glass> glassStack)
        {
            bool isSuccess = glassStack.TryPop(out Glass jonas);
            if (isSuccess)
            {
                return isSuccess;
            }
            else
            {
                return false;
            }
        }
    }
}
