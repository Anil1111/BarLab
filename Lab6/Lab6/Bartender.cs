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
            ConcurrentStack<Glass> cleanGlassStack, ConcurrentStack<Glass> dirtyGlassStack, 
            bool bartenderIsWorking, ConcurrentStack<Chair> freeChairStack)

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
                    if (!PatronQueue.IsEmpty)
                    {
                        if (!cleanGlassStack.IsEmpty)
                        {
                            cleanGlassStack.TryPop(out Glass g);
                            Thread.Sleep(1000);

                            //Patron Dequeue fuckar upp. Testa
                            Callback($"The Bartender is fetching {PatronQueue.First().Name} a glass.");
                            Thread.Sleep(3000);
                            Callback($"The Bartender is pouring {PatronQueue.First().Name} a beer.");
                            Thread.Sleep(3000);
                            PatronQueue.FirstOrDefault().SitDown(PatronListCallback, DirtyGlassStack, FreeChairStack, PatronQueue);
                        }
                        else
                        {
                            Callback("The Bartender is waiting for Glasses.");
                            Thread.Sleep(3000);
                        }
                    }
                    else
                    {
                        Callback("The Bartender is waiting for Patrons.");
                        Thread.Sleep(3000);
                    }
                }
                Callback("The bartender goes home.");
            });
        }
        
        public void StopServing()
        {
            BarIsOpen = false;
        }
    }
}
