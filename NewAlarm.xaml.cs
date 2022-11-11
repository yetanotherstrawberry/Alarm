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
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Enter:
                    CloseButton(sender, e);
                    break;
            }
        }

    }
}
