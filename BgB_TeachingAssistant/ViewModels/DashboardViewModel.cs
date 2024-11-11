using Bgb_DataAccessLibrary.Factories; // Ensure this namespace is correct
using Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators;
using BgB_TeachingAssistant.Commands;
using System.Windows;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public override string Name => "Dashboard"; // Implementing the Name property
        private readonly IEventAggregator _eventAggregator;

        public ICommand TriggerDataProcessingCommand { get; }

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

        public DashboardViewModel(IServiceFactory serviceFactory, IEventAggregator eventAggregator) : base(serviceFactory, eventAggregator)
        {
            Console.WriteLine("DashboardViewModel created.");
            Message = "default display-message";
            _eventAggregator = eventAggregator;
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
        public void Dispose()
        {
            Console.WriteLine($"Message variable value at time of execution of the Dispose Method: {Message}");
            Console.WriteLine("DashboardViewModel disposed.");
            // Cleanup resources if needed
        }
    }

}
