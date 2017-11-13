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
        private ConcurrentStack<Glass> DirtyGlassStack;
        private ConcurrentStack<Glass> CleanGlassStack;

        public void Work(ConcurrentQueue<Patron> patronBarQueue, Action<string> callback, ConcurrentStack<Glass> cleanGlassStack,
            ConcurrentStack<Glass> dirtyGlassStack, bool IsOpen)

        {
            this.Callback = callback;
            this.PatronBarQueue = patronBarQueue;
            this.DirtyGlassStack = dirtyGlassStack;
            this.CleanGlassStack = cleanGlassStack;

            Task.Run(() =>
            {
                while (IsOpen) //Kommer att kolla om baren är öppen
                {
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
                            // Här nånstanns borde ett event raiseas. "Nu har patronen fått en öl och lämnar barQueuen!"
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
            });
        }
    }
}
