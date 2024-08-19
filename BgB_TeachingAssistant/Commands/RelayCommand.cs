using System;
using System.Windows.Input;

namespace BgB_TeachingAssistant.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // New constructor for Action delegates (no parameters)
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute != null ? new Action<object>(o => execute()) : null;
            _canExecute = canExecute != null ? new Func<object, bool>(o => canExecute()) : null;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
