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
        private ConcurrentStack<Glass> DirtyGlassStack;
        private ConcurrentStack<Glass> CleanGlassStack;
        private ConcurrentStack<Chair> FreeChairStack;
        public bool BarIsOpen { get; set; }

        public void Work(ConcurrentQueue<Patron> patronQueue, Action<string> callback, Action<string> PatronListCallback,
            ConcurrentStack<Glass> cleanGlassStack, ConcurrentStack<Glass> dirtyGlassStack, bool bartenderIsWorking, ConcurrentStack<Chair> freeChairStack)

        {
            this.Callback = callback;
            this.PatronQueue = patronQueue;
            this.DirtyGlassStack = dirtyGlassStack;
            this.CleanGlassStack = cleanGlassStack;
            this.FreeChairStack = freeChairStack;
            this.BarIsOpen = bartenderIsWorking;

            Task.Run(() =>
            {
                while (BarIsOpen || !PatronQueue.IsEmpty) // Kommer att jobba medan det finns kunder kvar
                {
                    Console.WriteLine("Bartendern jobbar");
                    Thread.Sleep(1000);
                    if (!PatronQueue.IsEmpty)
                    {
                        if (!cleanGlassStack.IsEmpty)
                        {
                            Callback($"The Bartender is fetching {((Patron)PatronQueue.First()).Name} a glass");
                            Thread.Sleep(3000);
                            cleanGlassStack.TryPop(out Glass g);
                            Callback($"The Bartender is pouring {((Patron)PatronQueue.First()).Name} a beer.");
                            Thread.Sleep(3000);

                            //If Patron has a beer:
                            //Patron looks for chairs and triggers Sitdown(). 
                            //Sitdown() dequeues Patrons from its queue instead of the bartender queue
                            PatronQueue.First().SitDown(PatronListCallback, DirtyGlassStack, FreeChairStack, PatronQueue);
                            PatronQueue.TryDequeue(out Patron p);
                        }
                        else
                        {
                            Callback("The Bartender is waiting for Glasses.");
                        }
                    }
                    else
                    {
                        Callback("The Bartender is waiting for Patrons.");
                    }
                }
                Callback("bartender jobbar inte");
            });
        }
        
        public void StopServing()
        {
            BarIsOpen = false;
        }
    }
}
