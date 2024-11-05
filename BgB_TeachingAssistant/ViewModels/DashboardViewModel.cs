using Bgb_DataAccessLibrary.Factories;
using BgB_TeachingAssistant.Services;
using BgB_TeachingAssistant.Services.Infrastructure;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BgB_TeachingAssistant.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        // Override the Name property from IPageViewModel
        public override string Name => "Dashboard";
        public event PropertyChangedEventHandler PropertyChanged;

        private string _message = "default display-message";
        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    Console.WriteLine("PropertyChanged triggered for Message");
                    OnPropertyChanged(nameof(Message));
                }
            }
        }

        // Step 1: Define a delegate type
        //public delegate void SimpleDelegate(string message);
        private readonly EventAggregator _eventAggregator;

        private readonly DashboardEventTesting _dashboardEventTesting;
        public DashboardViewModel(IServiceFactory serviceFactory)
            : base(serviceFactory)
        {
            EventAggregator eventAggregator = new EventAggregator();
            //*****
            //event Aggregator test
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe<string>(OnDataReceived);


            //*****
            //normal communication
            ////get and store dataService class object with event in it
            _dashboardEventTesting = new DashboardEventTesting();

            
            NewVM_EA_Exchange(eventAggregator);

            //NormalVM_DS_Exchange();

            //*****
            //no communication
            //TriggerDelegateTest();

        }

        private async void NewVM_EA_Exchange(EventAggregator eventAggregator)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Would you like to trigger the DataProcessing?");
                string userInput = Console.ReadLine();

                if (userInput == "y")
                {
                    // Use Dispatcher to update UI-bound properties from a non-UI thread
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        Message = "DataProcessing triggered.. ";
                        _dashboardEventTesting.ProcessData(eventAggregator);
                    });
                }
            });
        }
        private void NormalVM_DS_Exchange()
        {

            //*****
            //normal communication
            //_dashboardEventTesting.DataProcessed += OnDataProcessed;

            Console.WriteLine("Would you like to trigger the DataProcessing?");
            string userInput = Console.ReadLine();

            if (userInput == "y")
            {
                _dashboardEventTesting.ProcessData();
            }
            else
            {
                return;
            }
        }

        private void OnDataProcessed(string data)
        {
            Console.WriteLine("DashboardViewModel received data: " + data);
        }

        private void OnDataReceived(string data)
        {
            Console.WriteLine("Data received in ViewModel: " + data);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        //private void TriggerDelegateTest()
        //{
        //    // Step 3: Instantiate the delegate and pass it a method
        //    SimpleDelegate del = new SimpleDelegate(PrintMessage);

        //    // Step 4: Call the delegate
        //    del("Hello, this is a simple delegate example!");

        //    // Step 5: Change the method the delegate points to
        //    del = PrintUpperCaseMessage;

        //    // Call the delegate again
        //    del("This is now uppercase!");

        //    // Step 6: Passing the delegate as a parameter to another method
        //    ExecuteDelegate(del, "Delegates are powerful!");

        //    // Step 7: Multicast delegate (invoking multiple methods)
        //    del += PrintMessage; // Add another method to the invocation list
        //    del("This is a multicast delegate!");
        //}
        //static void ExecuteDelegate(SimpleDelegate simpleDelegate, string msg)
        //{
        //// Method that accepts a delegate as a parameter
        //    simpleDelegate(msg);
        //}
        //public static void PrintMessage(string message)
        //{
        //// Step 2: Create methods that match the delegate signature
        //    Console.WriteLine("Message: " + message);
        //}
        //public static void PrintUpperCaseMessage(string message)
        //{
        //// Step 2: Create methods that match the delegate signature
        //    Console.WriteLine("Uppercase Message: " + message.ToUpper());
        //}


    }
}
