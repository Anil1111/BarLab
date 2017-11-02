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

        public void Work(ConcurrentQueue<Patron> patronBarQueue, Action<string> callback, ConcurrentStack<Glass> glassStack, bool IsOpen)
        {
            this.Callback = callback;
            this.PatronQueue = patronBarQueue;
            this.GlassStack = glassStack;

            Task.Run(() =>
            {
                while (IsOpen) //Kommer att kolla om baren är öppen
                {
                    Thread.Sleep(1000);
                    if (!patronBarQueue.IsEmpty)
                    {
                        if (!glassStack.IsEmpty)
                        {
                            callback($"The Bartender is fetching {((Patron)patronBarQueue.First()).Name} a glass");
                            Thread.Sleep(3000);
                            GlassStack.TryPop(out Glass g);
                            callback($"The Bartender is pouring {((Patron)patronBarQueue.First()).Name} a beer.");
                            Thread.Sleep(3000);
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
