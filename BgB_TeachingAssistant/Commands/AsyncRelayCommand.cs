using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BgB_TeachingAssistant.Commands
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, Task> _executeAsync;
        private readonly Func<object, bool> _canExecute;

        // Constructor for async commands with parameters
        public AsyncRelayCommand(Func<object, Task> executeAsync, Func<object, bool> canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        // Constructor for async commands without parameters
        public AsyncRelayCommand(Func<Task> executeAsync, Func<bool> canExecute = null)
        {
            if (executeAsync == null) throw new ArgumentNullException(nameof(executeAsync));
            _executeAsync = _ => executeAsync();
            _canExecute = canExecute != null ? _ => canExecute() : null;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(object parameter)
        {
            await _executeAsync(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
