using RaspAlarm.Interfaces;
using System.Windows;

namespace RaspAlarm.Views
{
    /// <summary>
    /// WPF window asking the user for input. Implements <c>IInputView</c>.
    /// </summary>
    public partial class InputWindow : Window, IInputView
    {

        /// <summary>
        /// Initializes XAML.
        /// </summary>
        public InputWindow()
        {
            InitializeComponent();
        }

        IInputViewModel IInputView.DataContext => DataContext as IInputViewModel;

    }
}
