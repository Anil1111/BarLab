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

        public void Work(Action<string> callback, ConcurrentStack<Glass> dirtyGlassStack, ConcurrentStack<Glass> cleanGlassStack, bool isOpen)
        {
            this.Callback = callback;
            this.DirtyGlassStack = dirtyGlassStack;
            this.CleanGlassStack = cleanGlassStack;

            Task.Run(() =>
            {
                while(isOpen) // Kolla om baren är öppen
                {
                    if (!dirtyGlassStack.IsEmpty)
                    {
                        callback("The waiter picks up a dirty glass from a table.");
                        dirtyGlassStack.TryPop(out Glass g);
                        Thread.Sleep(10000);
                        callback("The waiter is washing a glass.");
                        Thread.Sleep(15000);
                        callback("The waiter places the clean glass back on the shelf.");
                        cleanGlassStack.Push(new Glass()); // Går det att pusha utan att göra ett nytt glass-object?
                    }
                }

            });
        }
    }
}
