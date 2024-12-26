using MVVM_UtilitiesLibrary.BaseClasses;
using BgB_TeachingAssistant.Commands;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class PromptViewModel : ObservableObject
    {
        private string _title;
        private object _content;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public object Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public PromptViewModel(ICommand okCommand, ICommand cancelCommand)
        {
            OkCommand = okCommand;
            CancelCommand = cancelCommand;
        }
    }
}