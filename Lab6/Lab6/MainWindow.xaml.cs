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

        bool barIsOpen;

        // Här deklarerar vi en ConcurrectQueue som patorns hamnar i när vi skapar dem. I main ska vi även ha funktioner som lägger till och
        // tar bort patrons från queuen. Med hjälp av delegates kan vi meddela de andra klasserna
        // (tex bouncer och bartender) vad som händer i queuen

        public MainWindow()
        {
            InitializeComponent();
        }
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            CancellationToken ct = cts.Token;
            BtnClose.IsEnabled = true;
            BtnStart.IsEnabled = false;
            FillGlassStack();
            barIsOpen = true;

            Bouncer bouncer = new Bouncer();
            Bartender bartender = new Bartender();
            bouncer.Work(UpdatePatronList, AddPatronToQueue);
            bartender.Work(patronBarQueue, UpdateBartenderList, cleanGlassStack, barIsOpen);
        }

        //Updating Listbox elements
        private void UpdatePatronList(string info)
        {
            Dispatcher.Invoke(() => 
            {
                ListPatron.Items.Insert(0, info);      
            });
        }

        private void UpdateBartenderList(string info)
        {
            Dispatcher.Invoke(() =>
            {
                ListBartender.Items.Insert(0, info);
            });
        }

        //Function that adds Patron to ConcurrentQueue
        private void AddPatronToQueue(Patron p)
        {
            patronBarQueue.Enqueue(p);
        }

        //Function that removes Patron from ConcurrentQueue
        private void RemovePatronFromQueue(Patron p)
        {
            patronBarQueue.TryDequeue(out p);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
            BtnClose.IsEnabled = true;
            BtnStart.IsEnabled = false;
        }

        //Function that creates glass objects and adds to ConcurrentStack
        private void FillGlassStack()
        {
            for (int i = 0; i < 8; i++)
            {
                cleanGlassStack.Push(new Glass());
                Console.WriteLine("Added glass object to stack.");
            }
        }
    }
}