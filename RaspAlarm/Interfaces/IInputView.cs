namespace RaspAlarm.Interfaces
{
    internal interface IInputView
    {

        IInputViewModel DataContext { get; }
        bool? DialogResult { get; set; }
        bool? ShowDialog();
        void Close();

    }
}
