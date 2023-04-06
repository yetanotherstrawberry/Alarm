using RaspAlarm.Interfaces;
using System.Windows;

namespace RaspAlarm.Views
{
    public partial class InputWindow : Window, IInputView
    {

        /// <summary>
        /// Initializes XAML.
        /// </summary>
        public InputWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Returns ViewModel of this View.
        /// </summary>
        IInputViewModel IInputView.DataContext => DataContext as IInputViewModel;

    }
}
