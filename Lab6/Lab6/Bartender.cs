﻿using System;
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
        bool working = true;

        public void Work(ConcurrentQueue<Patron> patronBarQueue, ConcurrentQueue<Patron> patronChairQueue, Action<string> callback, 
            ConcurrentStack<Glass> cleanGlassStack, ConcurrentStack<Glass> dirtyGlassStack)

        {
            this.Callback = callback;
            this.PatronBarQueue = patronBarQueue;
            this.DirtyGlassStack = dirtyGlassStack;
            this.CleanGlassStack = cleanGlassStack;
            this.PatronChairQueue = patronChairQueue;

            Task.Run(() =>
            {
                while (/*en bool*/) // Kommer att jobba medan det finns kunder kvar
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

                    // kanske ha för att få bartendern att sluta jobba
                    if(patronBarQueue.IsEmpty && patronChairQueue.IsEmpty)
                    {
                        working = false;
                        callback("The Bartender went home.");
                    }
                }
            });
        }
    }
}
