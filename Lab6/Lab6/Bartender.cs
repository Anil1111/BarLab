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
        private ConcurrentQueue<Patron> PatronQueue;
        private ConcurrentQueue<Patron> BartenderQueue;
        private ConcurrentStack<Glass> DirtyGlassStack;
        private ConcurrentStack<Glass> CleanGlassStack;
        private ConcurrentStack<Chair> FreeChairStack;
        public bool BarIsOpen { get; set; }

        public void Work(ConcurrentQueue<Patron> patronQueue, ConcurrentQueue<Patron> bartenderQueue, Action<string> callback, Action<string> PatronListCallback,
            ConcurrentStack<Glass> cleanGlassStack, ConcurrentStack<Glass> dirtyGlassStack, 
            bool bartenderIsWorking, ConcurrentStack<Chair> freeChairStack, ConcurrentQueue<string> uiPatronCountDequeue)

        {
            this.Callback = callback;
            this.PatronQueue = patronQueue;
            this.BartenderQueue = bartenderQueue;
            this.DirtyGlassStack = dirtyGlassStack;
            this.CleanGlassStack = cleanGlassStack;
            this.FreeChairStack = freeChairStack;
            this.BarIsOpen = bartenderIsWorking;

            Task.Run(() =>
            {

                //För tillfället så går Bartendern hem så fort Bartender queuen är tom, vilket är fel. 
                //Kolla närmare på en lösning. Det bör ju bara vara en if-jonas som kan lösa det. Behöver kaffe. 
                //They're coming... 
                //Balrogen är här, Frodo. 
                //Ich bin Disco Gandalf.
                while (BarIsOpen || !PatronQueue.IsEmpty)
                {
                    if (!PatronQueue.IsEmpty && !BartenderQueue.IsEmpty)
                    {
                        if (!cleanGlassStack.IsEmpty)
                        {
                            cleanGlassStack.TryPop(out Glass g);
                            Thread.Sleep(1000);
                            Callback($"The Bartender is fetching {BartenderQueue.First().Name} a glass.");
                            Thread.Sleep(3000);
                            Callback($"The Bartender is pouring {BartenderQueue.First().Name} a beer.");
                            Thread.Sleep(3000);
                            PatronQueue.FirstOrDefault().SitDown(PatronListCallback, DirtyGlassStack, FreeChairStack, PatronQueue, uiPatronCountDequeue);
                            BartenderQueue.TryDequeue(out Patron p);
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
                Callback("The Bartender goes home.");
            });
        }
        
        public void StopServing()
        {
            BarIsOpen = false;
        }
    }
}
