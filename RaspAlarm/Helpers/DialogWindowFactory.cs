﻿using RaspAlarm.Interfaces;
using RaspAlarm.Views;

namespace RaspAlarm.Helpers
{
    internal class DialogWindowFactory : IDialog
    {

        public string ShowDialog()
        {
            IInputView window = new InputWindow();

            if (!window.ShowDialog() ?? true) // Return String.Empty if view aborted by user.
                return string.Empty;

            var viewModel = window.DataContext;

            return viewModel.Text;
        }

    }
}
