﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Lab6
{

    public class Bouncer
    {
        private Action<string> Callback;
        private Action<Patron> PatronCallback;
        Random random = new Random();
        Stopwatch stopwatch = new Stopwatch();

        public bool IsWorking { get; set; }

        public event Action IsClosing;
        
        List<string> PatronNameList = new List<string>()
            { "Jonas", "Klas", "Göran", "Getrud", "Daniel", "Petra", "Tor", "Styr-Björn", "Greta", "Livingston", "Margret",
             "Ingemar", "Birgit", "David", "Jon", "Tyrion", "Gandalf", "Jamie", "Frodo", "Ron", "Harry", "Hagrid"};

        //Work method
        public void Work(Action<string> Callback, Action<Patron> patronCallback, int barOpenBouncer)
        {
            IsWorking = true;
            Task.Run(() => {
                this.PatronCallback = patronCallback;
                this.Callback = Callback;

                stopwatch.Start();
                while (stopwatch.Elapsed < TimeSpan.FromSeconds(barOpenBouncer))
                {
                    Thread.Sleep(random.Next(3000, 10000));
                    string patronName = PatronNameList[random.Next(PatronNameList.Count)];
                    patronCallback(new Patron(patronName));
                    Callback($"{patronName} has entered the bar.");
                }
                stopwatch.Stop();
                IsClosing();
                Callback("The bouncer goes home.");
            });
        }
    }
}