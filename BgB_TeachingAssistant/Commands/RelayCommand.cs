using System;
using System.Windows.Input;

namespace BgB_TeachingAssistant.Commands
{
    public class RelayCommand : ICommand, IDisposable
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;
        private bool _isDisposed;

        // Backing delegate to store CanExecuteChanged handlers
        private EventHandler _canExecuteChangedHandlers;

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

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!_isDisposed)
                {
                    CommandManager.RequerySuggested += value;
                    _canExecuteChangedHandlers += value;
                }
            }
            remove
            {
                if (!_isDisposed)
                {
                    CommandManager.RequerySuggested -= value;
                    _canExecuteChangedHandlers -= value;
                }
            }
        }

        public void RaiseCanExecuteChanged()
        {
            // Raise CanExecuteChanged to notify the command system that the command's ability to execute has changed
            _canExecuteChangedHandlers?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                // Set disposed flag
                _isDisposed = true;

                // Clear references to help with garbage collection
                _execute = null;
                _canExecute = null;

                // Clear all subscriptions to CanExecuteChanged
                if (_canExecuteChangedHandlers != null)
                {
                    Delegate[] invocationList = _canExecuteChangedHandlers.GetInvocationList();
                    foreach (var handler in invocationList)
                    {
                        CommandManager.RequerySuggested -= (EventHandler)handler;
                    }
                }

                // Nullify handlers to release references
                _canExecuteChangedHandlers = null;
            }
        }
    }
}
