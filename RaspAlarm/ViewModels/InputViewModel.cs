using RaspAlarm.Helpers;
using RaspAlarm.Interfaces;
using System.Windows.Input;

namespace RaspAlarm.ViewModels
{
    internal class InputViewModel : IInputViewModel
    {

        /// <summary>
        /// Text entered by the user.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Clears the <c>InputText</c> and closes the view.
        /// </summary>
        public ICommand AbortCommand { get; private set; }

        /// <summary>
        /// Closes the view.
        /// </summary>
        public ICommand CloseCommand { get; private set; }

        /// <summary>
        /// Implements commands.
        /// </summary>
        public InputViewModel()
        {
            AbortCommand = new DelegateCommand<IInputView>(view =>
            {
                view.DialogResult = false;
                Text = string.Empty;
                CloseCommand.Execute(view);
            });
            CloseCommand = new DelegateCommand<IInputView>(view =>
            {
                view.DialogResult = view.DialogResult ?? true;
                view.Close();
            });
        }

    }
}
