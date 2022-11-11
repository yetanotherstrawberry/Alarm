using System.Windows;
using System.Windows.Input;

namespace Alarm
{
    public partial class NewAlarm : Window
    {
        public NewAlarm() => InitializeComponent();

        private void CloseButton(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void KeyAction(object sender, KeyEventArgs e)
        {
            if (Key.Enter.Equals(e.Key))
            {
                CloseButton(sender, e);
            }
        }
    }
}
