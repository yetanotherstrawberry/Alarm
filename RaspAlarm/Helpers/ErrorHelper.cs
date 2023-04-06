using RaspAlarm.Properties;
using System;
using System.Windows;

namespace RaspAlarm.Helpers
{
    internal static class ErrorHelper
    {

        /// <summary>
        /// Shows a templated <c>MessageBox</c> with the supplied message.
        /// </summary>
        /// <param name="text">Message text.</param>
        public static void ShowError(string text)
        {
            MessageBox.Show(text, Resources.ERR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Executes <c>ShowError(string)</c> with message od the <c>Exception</c>.
        /// </summary>
        /// <param name="exc"><c>Exception</c> which <c>Message</c> field will be displayed.</param>
        public static void ShowError(Exception exc) => ShowError(exc.Message);

    }
}
