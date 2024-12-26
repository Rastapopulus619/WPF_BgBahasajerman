using MVVM_UtilitiesLibrary.BaseClasses;
using BgB_TeachingAssistant.Commands;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class InformationalPromptViewModel : ObservableObject
    {
        private string _title;
        private string _message;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand OkCommand { get; }

        public InformationalPromptViewModel(ICommand okCommand)
        {
            OkCommand = okCommand;
        }
    }
}
