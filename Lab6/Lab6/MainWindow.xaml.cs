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

            bouncer.Work(UpdatePatronList, AddPatronToQueue);
            bartender.Work(patronQueue, UpdateBartenderList, UpdatePatronList, cleanGlassStack, 
                dirtyGlassStack, bouncer.IsWorking, freeChairStack);
            waiter.Work(UpdateWaiterList, dirtyGlassStack, cleanGlassStack, bouncer.IsWorking, patronQueue);
        }

        //Updating Listbox elements for Patron ListBox
        private void UpdatePatronList(string info)
        {
            Dispatcher.Invoke(() => 
            {
                ListPatron.Items.Insert(0, info);
                LblPatronCount.Content = "Patrons in bar: " + ((int)patronQueue.Count()+1);
                LblChairCount.Content = $"Vacant chairs: {freeChairStack.Count()} (9 total)";
            });
        }

        //Updating Listbox elements for Bartender ListBox
        private void UpdateBartenderList(string info)
        {
            Dispatcher.Invoke(() =>
            {
                ListBartender.Items.Insert(0, info);
                LblGlassCount.Content = $"Glasses on shelf: {cleanGlassStack.Count()} (8 total)";
            });
        }

        //Updating Listbox elements for Waiter ListBox
        private void UpdateWaiterList(string info)
        {
            Dispatcher.Invoke(() =>
            {
                ListWaiter.Items.Insert(0, info);
                LblGlassCount.Content = $"Glasses on shelf: {cleanGlassStack.Count()} (8 total)";
            });
        }

        //Function that adds Patron to Bar
        private void AddPatronToQueue(Patron p)
        {
            patronQueue.Enqueue(p);
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
            for (int i = 0; i < 1; i++)
            {
                freeChairStack.Push(new Chair());
                Console.WriteLine("Added chair object to stack");
            }
        }
    }
}