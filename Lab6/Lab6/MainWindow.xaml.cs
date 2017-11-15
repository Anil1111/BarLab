using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        //Patron queue
        ConcurrentQueue<Patron> patronQueue = new ConcurrentQueue<Patron>();

        //Glass queues
        ConcurrentStack<Glass> cleanGlassStack = new ConcurrentStack<Glass>();
        ConcurrentStack<Glass> dirtyGlassStack = new ConcurrentStack<Glass>();

        //Chair queue
        ConcurrentStack<Chair> freeChairStack = new ConcurrentStack<Chair>();
        
        Bouncer bouncer = new Bouncer();
        Bartender bartender = new Bartender();
        Waiter waiter = new Waiter();

        // Variables for the test cases
        // Patron drinking time is found in the patron class
        private int barOpenUI = 120;
        private int barOpenBouncer = 120;
        private int glasses = 22;
        private int chairs = 11;
        private int waiterWashingSec = 15000;
        private int waiterPickingGlassesSec = 10000;




        public MainWindow()
        {
            InitializeComponent();
            bouncer.IsClosing += bartender.StopServing;
            bouncer.IsClosing += waiter.StopServing;

        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            BtnStart.IsEnabled = false;
            CreateGlassStack();
            CreateChairStack();

            // Timer to be shown in the UI
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            bouncer.Work(UpdatePatronList, AddPatronToQueue, barOpenBouncer);
            bartender.Work(patronQueue, UpdateBartenderList, UpdatePatronList, cleanGlassStack, 
                    dirtyGlassStack, bouncer.IsWorking, freeChairStack);
            waiter.Work(UpdateWaiterList, dirtyGlassStack, cleanGlassStack, bouncer.IsWorking, 
                    patronQueue, waiterWashingSec, waiterPickingGlassesSec);
        }

        // Event handler for the timer
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if(barOpenUI > 0)
            {
                barOpenUI--;
                lblTimeLeftOpen.Content = string.Format($"Time left open: {barOpenUI}");
            }
            else
            {
                lblTimeLeftOpen.Content = "Time left open: 0";
            }
        }

        //Updating Listbox elements for Patron ListBox
        private void UpdatePatronList(string info)
        {
            Dispatcher.Invoke(() => 
            {
                ListPatron.Items.Insert(0, info);
                LblPatronCount.Content = $"Patrons in bar: {patronQueue.Count()}";
                LblChairCount.Content = $"Vacant chairs: {freeChairStack.Count()} ({chairs} total)";
            });
        }

        //Updating Listbox elements for Bartender ListBox
        private void UpdateBartenderList(string info)
        {
            Dispatcher.Invoke(() =>
            {
                ListBartender.Items.Insert(0, info);
                LblGlassCount.Content = $"Glasses on shelf: {cleanGlassStack.Count()} ({glasses} total)";
                LblChairCount.Content = $"Vacant chairs: {freeChairStack.Count()} ({chairs} total)";
            });
        }

        //Updating Listbox elements for Waiter ListBox
        private void UpdateWaiterList(string info)
        {
            Dispatcher.Invoke(() =>
            {
                ListWaiter.Items.Insert(0, info);
                LblGlassCount.Content = $"Glasses on shelf: {cleanGlassStack.Count()} ({glasses} total)";
            });
        }

        //Function that adds Patron to Bar
        private void AddPatronToQueue(Patron p)
        {
            patronQueue.Enqueue(p);
        }

        //Function that creates glass objects and adds to ConcurrentStack
        private void CreateGlassStack()
        {
            for (int i = 0; i < glasses; i++)
            {
                cleanGlassStack.Push(new Glass());
                Console.WriteLine("Added glass object to stack.");
            }
        }

        //Function that creates chair objects and add to ConcurrentStack
        private void CreateChairStack()
        {
            for (int i = 0; i < chairs; i++)
            {
                freeChairStack.Push(new Chair());
                Console.WriteLine("Added chair object to stack");
            }
        }
    }
}