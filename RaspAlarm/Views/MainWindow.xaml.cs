using System;
using System.Windows;

namespace RaspAlarm.Views
{
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Initializes XAML.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Disposes the ViewModel.
        /// </summary>
        /// <param name="sender"><c>Object</c> that called this method.</param>
        /// <param name="args"><c>EventArgs</c> associated with this event.</param>
        private void Window_Closed(object sender, EventArgs args) => (DataContext as IDisposable)?.Dispose();

    }
}
