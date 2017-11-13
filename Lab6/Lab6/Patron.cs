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
        //EVENT LÖSNING:
            //Vi behöver ett event som Patron tittar på:
            //När Patron har fått sin öl och dequeueas från barQueuen så ska Patron.SitDown() triggas. 

        public string Name { get; set; }

        public Patron(string name)
        {
            this.Name = name;
        }

        private Action<string> Callback;
        private Action<Chair> FreeChairStack;
        private Action<Patron> PatronChairQueue;
        private ConcurrentStack<Glass> DirtyGlassStack;
        

        public void SitDown(Action<string> callback, ConcurrentStack<Glass> dirtyGlassStack, Action<Chair> freeChairStack,
            Action<Patron> patronChairQueue)
        {
            this.Callback = callback;
            this.DirtyGlassStack = dirtyGlassStack;
            this.FreeChairStack = freeChairStack;
            this.PatronChairQueue = patronChairQueue;

            Task.Run(() =>
            {
                callback("NAMN sits down.");

                dirtyGlassStack.Push(new Glass()); // händer när patronen lämnar baren
            });
        }
    }
}
