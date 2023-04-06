using RaspAlarm.Helpers;
using RaspAlarm.Interfaces;
using System.Windows.Input;

namespace RaspAlarm.ViewModels
{
    /// <summary>
    /// Implementation for <c>IInputViewModel</c>.
    /// </summary>
    internal class InputViewModel : IInputViewModel
    {

        /// <summary>
        /// Text entered by the user.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Closes the view.
        /// </summary>
        public ICommand DoneCommand { get; private set; }

        /// <summary>
        /// Implements commands.
        /// </summary>
        public InputViewModel()
        {
            DoneCommand = new DelegateCommand<IInputView>(view =>
            {
                view.DialogResult = view.DialogResult ?? !string.IsNullOrWhiteSpace(Text);
                view.Close();
            });
        }

    }
}
