using System;
using System.ComponentModel;
using System.Windows;

namespace RaspAlarm.Views
{
    /// <summary>
    /// Main <c>Window</c> of the application.
    /// </summary>
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
        private void Window_Closing(object sender, CancelEventArgs args) => (DataContext as IDisposable)?.Dispose();

    }
}
