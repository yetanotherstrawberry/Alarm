namespace RaspAlarm.Interfaces
{
    /// <summary>
    /// Interface for Views that provide input from user. Provides <c>IInputViewModel</c> and <c>Window</c> methods.
    /// </summary>
    internal interface IInputView
    {

        /// <summary>
        /// Gets the ViewModel of this View.
        /// </summary>
        IInputViewModel DataContext { get; }

        /// <summary>
        /// Gets or sets the result of the dialog:
        /// <c>true</c> if user confirmed the dialog,
        /// <c>false</c> if user aborted or closed the dialog,
        /// <c>null</c> if unspecified.
        /// </summary>
        bool? DialogResult { get; set; }

        /// <summary>
        /// Shows the View of the input dialog.
        /// </summary>
        /// <returns><c>DialogResult</c></returns>
        bool? ShowDialog();

        /// <summary>
        /// Closes the input view window.
        /// </summary>
        void Close();

    }
}
