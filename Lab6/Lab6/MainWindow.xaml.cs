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
        ConcurrentQueue<Patron> patronQueue = new ConcurrentQueue<Patron>();

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
            
            Bouncer b = new Bouncer();
            b.Work(AddPatronList, AddPatronToQueue);
        }

        //Delegate funktion för Bouncer
        private void AddPatronList(string info)
        {
            Dispatcher.Invoke(() => 
            {
                ListPatron.Items.Insert(0, $"{info} has entered the bar.");      
            });
        }

        //Function that adds Patron to ConcurrentQueue
        private void AddPatronToQueue(Patron p)
        {
            patronQueue.Enqueue(p);
        }

        //Function that removes Patron from ConcurrentQueue
        private void RemovePatronFromQueue(Patron p)
        {
            patronQueue.TryDequeue(out p);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
            BtnClose.IsEnabled = true;
            BtnStart.IsEnabled = false;
        }

        private void DebugPatron_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(((Patron)patronQueue.Last()).Name);
        }
    }
}