using Bgb_DataAccessLibrary.Contracts;
using BgB_TeachingAssistant.Commands;
using System.Windows;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public override string Name => "Dashboard"; // Implementing the Name property

        public ICommand TriggerDataProcessingCommand { get; private set; }

        private string _message = "default display-message";
        public string Message
        {
            get => _message;
            set
            {
                if (SetProperty(ref _message, value)) // Using SetProperty from ViewModelBase
                {
                    Console.WriteLine($"Message changed to: {_message}");
                }
            }
        }

        public DashboardViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
        {

            Message = "default display-message";
            //EventAggregator = eventAggregator;
            TriggerDataProcessingCommand = new RelayCommand(NewEventTester);
        }

        private void NewEventTester()
        {
            // Ensure that the property change happens on the UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                Message = "DataProcessing triggered.. ";
                Console.WriteLine("PropertyChanged triggered for Message"); // Debug line
            });
        }
        protected override void Cleanup()
        {
            // Specific cleanup for DashboardViewModel

            // Log that cleanup is called
            Console.WriteLine("DashboardViewModel: Cleanup called");

            // Dispose of the TriggerDataProcessingCommand if it's disposable
            if (TriggerDataProcessingCommand is IDisposable disposableCommand)
            {
                disposableCommand.Dispose();
            }
            TriggerDataProcessingCommand = null;

            // Nullify properties to release references
            EventAggregator = null;
            ServiceFactory = null;

            // Additional Cleanup
            Message = null; // If Message is being referenced elsewhere, clearing it might help

            // Call base cleanup (if ViewModelBase has specific cleanup logic)
            //base.Cleanup();
        }

        ~DashboardViewModel()
        {
            Console.WriteLine("DashboardViewModel: Destructor called");
        }
    }

}
