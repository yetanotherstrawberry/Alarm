using RaspAlarm.Interfaces;
using RaspAlarm.Views;

namespace RaspAlarm.Helpers
{
    internal class DialogWindowFactory : IDialog
    {

        public string ShowDialog()
        {
            var window = new InputWindow();

            if (!window.ShowDialog() ?? true)
                return string.Empty;

            var viewModel = window.DataContext as IInputViewModel;

            return viewModel.Text;
        }

    }
}
