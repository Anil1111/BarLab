using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace Lab6
{
   public class Patron
    {
        public string Name { get; set; }
        Queue<string> patronNameQueue = new Queue<string>();

        public Patron(string name)
        {
            this.Name = name;
            patronNameQueue.Enqueue(Name);
        }

        private Action<string> Callback;
        private ConcurrentStack<Chair> FreeChairStack;
        private ConcurrentQueue<Patron> PatronQueue;
        private ConcurrentStack<Glass> DirtyGlassStack;
        public string BeerDrinkingPatron { get; set; }

        //Function that tells the Patron to "sit down" and drink the beer before disappearing from the queue
        public void SitDown(Action<string> callback, ConcurrentStack<Glass> dirtyGlassStack, ConcurrentStack<Chair> freeChairStack,
            ConcurrentQueue<Patron> patronQueue)
        {
            this.Callback = callback;
            this.DirtyGlassStack = dirtyGlassStack;
            this.FreeChairStack = freeChairStack;
            this.PatronQueue = patronQueue;

            Task.Run(() =>
            {
                BeerDrinkingPatron = patronNameQueue.FirstOrDefault();
                patronNameQueue.Dequeue();
                callback($"{BeerDrinkingPatron} letar efter ett bord.");
                Thread.Sleep(4000); 
                callback($"{BeerDrinkingPatron} sits down."); 
                Thread.Sleep(10000); //random mellan 10-20 sek 
                callback($"{BeerDrinkingPatron} finishes the beer and leaves the bar.");
                dirtyGlassStack.Push(new Glass()); // händer när patronen lämnar baren
            });
        }
    }
}
