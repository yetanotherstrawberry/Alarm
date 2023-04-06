using System;
using System.Windows.Input;

namespace RaspAlarm.Helpers
{
    internal class DelegateCommand<T> : ICommand
    {

        /// <summary>
        /// Method executed upon Execute().
        /// </summary>
        private readonly Action<T> commandAction;

        /// <summary>
        /// Method executed and returned upon CanExecute();
        /// </summary>
        private readonly Func<T, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Implements ICommand in such a way that it executes methods defined in parameters.
        /// </summary>
        /// <param name="callback">Method executed by the execution of this command.</param>
        /// <param name="execAllowed">Method determining whether execution is allowed; use <c>null</c> to always allow it.</param>
        /// <exception cref="ArgumentNullException">The <c>callback</c> is <c>null</c>.</exception>
        public DelegateCommand(Action<T> callback, Func<T, bool> execAllowed = null)
        {
            commandAction = callback ?? throw new ArgumentNullException(paramName: nameof(callback));
            canExecute = execAllowed;
        }

        public void Execute(object parameter = null) => commandAction((T)parameter);

        public bool CanExecute(object parameter = null) => canExecute == null || canExecute((T)parameter);

    }
}
