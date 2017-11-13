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
        private CancellationTokenSource cts = new CancellationTokenSource();
        
        //Patron queues
        ConcurrentQueue<Patron> patronBarQueue = new ConcurrentQueue<Patron>();
        ConcurrentQueue<Patron> patronChairQueue = new ConcurrentQueue<Patron>();

        //Glass queues
        ConcurrentStack<Glass> cleanGlassStack = new ConcurrentStack<Glass>();
        ConcurrentStack<Glass> dirtyGlassStack = new ConcurrentStack<Glass>();

        //Chair queue
        ConcurrentStack<Chair> freeChairStack = new ConcurrentStack<Chair>();
        
        Bouncer bouncer = new Bouncer();
        Bartender bartender = new Bartender();
        Waiter waiter = new Waiter();

        public MainWindow()
        {
            InitializeComponent();
            bouncer.IsClosing += bartender.StopServing;
            bouncer.IsClosing += waiter.StopServing;
        }
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            CancellationToken ct = cts.Token;
            BtnClose.IsEnabled = true;
            BtnStart.IsEnabled = false;
            CreateGlassStack();
            CreateChairStack();

            bouncer.Work(UpdatePatronList, AddPatronToBarQueue);
            bartender.Work(patronBarQueue, patronChairQueue, UpdateBartenderList, cleanGlassStack, 
                dirtyGlassStack, bouncer.IsWorking);
            waiter.Work(UpdateWaiterList, dirtyGlassStack, cleanGlassStack, bouncer.IsWorking, patronChairQueue);
        }

        //Updating Listbox elements for Patron ListBox
        private void UpdatePatronList(string info)
        {
            Dispatcher.Invoke(() => 
            {
                LblPatronCount.Content = "Patrons in bar: " + patronBarQueue.Count();
                ListPatron.Items.Insert(0, info);
            });
        }

        //Updating Listbox elements for Bartender ListBox
        private void UpdateBartenderList(string info)
        {
            Dispatcher.Invoke(() =>
            {
                ListBartender.Items.Insert(0, info);
            });
        }

        //Updating Listbox elements for Waiter ListBox
        private void UpdateWaiterList(string info)
        {
            Dispatcher.Invoke(() =>
            {
                ListWaiter.Items.Insert(0, info);
            });
        }

        //Function that adds Patron to Bar
        private void AddPatronToBarQueue(Patron p)
        {
            patronBarQueue.Enqueue(p);
        }
        
        private void AddPatronToChairQueue(Patron p)
        {
            patronChairQueue.Enqueue(p);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
            BtnClose.IsEnabled = true;
            BtnStart.IsEnabled = false;
        }

        //Function that creates glass objects and adds to ConcurrentStack
        private void CreateGlassStack()
        {
            for (int i = 0; i < 8; i++)
            {
                cleanGlassStack.Push(new Glass());
                Console.WriteLine("Added glass object to stack.");
            }
        }

        //Function that creates chair objects and add to ConcurrentStack
        private void CreateChairStack()
        {
            for (int i = 0; i < 9; i++)
            {
                freeChairStack.Push(new Chair());
                Console.WriteLine("Added chair object to stack");
            }
        }
    }
}