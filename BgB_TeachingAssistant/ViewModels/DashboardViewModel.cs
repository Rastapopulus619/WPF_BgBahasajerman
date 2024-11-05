using Bgb_DataAccessLibrary.Factories; // Ensure this namespace is correct
using BgB_TeachingAssistant.Commands;
using System.Windows;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public override string Name => "Dashboard"; // Implementing the Name property

        public ICommand TriggerDataProcessingCommand { get; }

        private string _message = "default display-message";
        public string Message
        {
            get => _message;
            set
            {
                if (SetProperty(ref _message, value)) // Using SetProperty from ViewModelBase
                {
                    // Additional logic can go here if needed
                }
            }
        }

        public DashboardViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
        {
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
    }
}
