using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace Lab6
{
    public class Waiter
    {
        private Action<string> Callback;
        private ConcurrentStack<Glass> DirtyGlassStack;
        private ConcurrentStack<Glass> CleanGlassStack;
        private ConcurrentQueue<Patron> PatronQueue;
        public bool BarIsOpen { get; set; }

        public void Work(Action<string> callback, ConcurrentStack<Glass> dirtyGlassStack, 
            ConcurrentStack<Glass> cleanGlassStack, bool bouncerIsWorking, ConcurrentQueue<Patron> patronQueue)
        {
            this.Callback = callback;
            this.DirtyGlassStack = dirtyGlassStack;
            this.CleanGlassStack = cleanGlassStack;
            this.BarIsOpen = bouncerIsWorking;
            this.PatronQueue = patronQueue;

            Task.Run(() =>
            {
                while (BarIsOpen)
                {
                    while (CleanGlassStack.Count() != 8) 
                    {
                        if (!DirtyGlassStack.IsEmpty)
                        {
                            Callback("The waiter picks up a dirty glass from a table.");
                            DirtyGlassStack.TryPop(out Glass g);
                            Thread.Sleep(5000);
                            Callback("The waiter is washing a glass.");
                            Thread.Sleep(7500);
                            Callback("The waiter places the clean glass back on the shelf.");
                            CleanGlassStack.Push(new Glass());
                        }
                    }
                }
                callback("The waiter goes home.");
            });
        }
        public void StopServing()
        {
            BarIsOpen = false;
        }
    }
}
