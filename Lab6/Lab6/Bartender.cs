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
        private ConcurrentQueue<Patron> PatronBarQueue;
        private ConcurrentQueue<Patron> PatronChairQueue;
        private ConcurrentStack<Glass> DirtyGlassStack;
        private ConcurrentStack<Glass> CleanGlassStack;
        public bool BarIsOpen { get; set; }

        public void Work(ConcurrentQueue<Patron> patronBarQueue, ConcurrentQueue<Patron> patronChairQueue, Action<string> callback, 
            ConcurrentStack<Glass> cleanGlassStack, ConcurrentStack<Glass> dirtyGlassStack, bool bartenderIsWorking)

        {
            this.Callback = callback;
            this.PatronBarQueue = patronBarQueue;
            this.DirtyGlassStack = dirtyGlassStack;
            this.CleanGlassStack = cleanGlassStack;
            this.PatronChairQueue = patronChairQueue;
            this.BarIsOpen = bartenderIsWorking;

            Task.Run(() =>
            {
                while (BarIsOpen || !PatronBarQueue.IsEmpty) // Kommer att jobba medan det finns kunder kvar
                {
                    Console.WriteLine("Bartendern jobbar");
                    Thread.Sleep(1000);
                    if (!patronBarQueue.IsEmpty)
                    {
                        if (!cleanGlassStack.IsEmpty)
                        {
                            callback($"The Bartender is fetching {((Patron)patronBarQueue.First()).Name} a glass");
                            Thread.Sleep(3000);
                            cleanGlassStack.TryPop(out Glass g);
                            callback($"The Bartender is pouring {((Patron)patronBarQueue.First()).Name} a beer.");
                            Thread.Sleep(3000);

                            //If Patron has a beer:
                            //Patron looks for chairs and triggers Sitdown(). 
                            //Sitdown() dequeues Patrons from its queue instead of the bartender queue
                            patronBarQueue.TryDequeue(out Patron p);
                        }
                        else
                        {
                            callback("The Bartender is waiting for Glasses.");
                        }
                    }
                    else
                    {
                        callback("The Bartender is waiting for Patrons.");
                    }
                }
                Console.WriteLine("bartender jobbar inte");
            });
        }
        
        public void StopServing()
        {
            BarIsOpen = false;
        }
    }
}
