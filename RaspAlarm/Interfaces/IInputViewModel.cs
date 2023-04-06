namespace RaspAlarm.Interfaces
{
    /// <summary>
    /// Interface for ViewModels of dialog views. Provides text from user.
    /// </summary>
    internal interface IInputViewModel
    {

        /// <summary>
        /// Text entered by the user.
        /// </summary>
        string Text { get; }

    }
}
