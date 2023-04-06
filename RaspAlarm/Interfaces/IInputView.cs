namespace RaspAlarm.Interfaces
{
    internal interface IInputView
    {

        bool? DialogResult { get; set; }
        void Close();

    }
}
