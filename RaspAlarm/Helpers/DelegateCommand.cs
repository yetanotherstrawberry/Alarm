using System;
using System.Windows.Input;

namespace RaspAlarm.Helpers
{

    internal class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> commandAction;
        private readonly Func<T, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action<T> callback, Func<T, bool> execAllowed = null)
        {
            commandAction = callback ?? throw new ArgumentNullException(paramName: nameof(callback));
            canExecute = execAllowed;
        }

        public void Execute(object parameter = null) => commandAction((T)parameter);

        public bool CanExecute(object parameter = null) => canExecute == null || canExecute((T)parameter);
    }

}
