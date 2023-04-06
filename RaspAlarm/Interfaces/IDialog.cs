namespace RaspAlarm.Interfaces
{
    internal interface IDialog
    {

        /// <summary>
        /// Shows a dialog that asks the user for an input.
        /// </summary>
        /// <returns>Text entered by the user. String.Empty if view aborted by user.</returns>
        string ShowDialog();

    }
}
