using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Alarm
{
    public partial class MainWindow : Window
    {

        private const int SnoozeTime = 5, AlarmFreq = 500, AlarmBeepTime = 3000, AlarmDelay = 2000;

        public ObservableCollection<Alarm> Alarms { get; } = new ObservableCollection<Alarm>();

        public class Alarm
        {
            public DispatcherTimer Dispatcher { get; }
            public string Time => DateTime.Now.AddTicks(Dispatcher.Interval.Ticks).ToString();

            public Alarm(EventHandler handler, TimeSpan interval)
            {
                Dispatcher = CreateDispatcher(handler, interval);
                Dispatcher.Tag = this;
                Dispatcher.Start();
            }
        }

        private void RefreshClock(object source = null, EventArgs ea = null) => Clock.Content = DateTime.Now.ToLongTimeString();

        private static DispatcherTimer CreateDispatcher(EventHandler handler, TimeSpan interval) =>
            new DispatcherTimer(
                callback: handler,
                interval: interval,
                priority: DispatcherPriority.Render,
                dispatcher: Dispatcher.CurrentDispatcher
            );

        private void ShowError(Exception exc) => ShowError(exc.Message);
        private void ShowError(string text) =>
            MessageBox.Show(owner: this, text, Properties.Resources.ERR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);

        private void Ring(object source, EventArgs ea)
        {
            try
            {
                var dispatcher = (DispatcherTimer)source;
                var alarm = (Alarm)dispatcher.Tag;

                dispatcher.Stop();
                Alarms.Remove(alarm);

                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                var ring = new Task(async () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        Console.Beep(AlarmFreq, AlarmBeepTime);
                        await Task.Delay(AlarmDelay);
                    }
                }, token);
                ring.Start();

                if (MessageBoxResult.Yes.Equals(
                    MessageBox.Show(
                        string.Format(Properties.Resources.ALARM_MSGBOX_CONTENT, SnoozeTime),
                        Properties.Resources.ALARM_MSGBOX_TITLE,
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question)))
                {
                    dispatcher.Interval = TimeSpan.FromMinutes(SnoozeTime);
                    dispatcher.Start();
                    Alarms.Add(alarm);
                }

                tokenSource.Cancel();
                if (ring.Exception != null)
                    throw ring.Exception;
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        private void NewAlarm(object sender, RoutedEventArgs rea)
        {
            try
            {
                var modal = new NewAlarm
                {
                    Owner = this,
                };
                if (!modal.ShowDialog() ?? true) return;

                string userInput = modal.Text.Text;
                if (string.IsNullOrEmpty(userInput)) return;

                if (!TimeSpan.TryParse(userInput, out TimeSpan time))
                {
                    ShowError(Properties.Resources.INVALID_TIME);
                    return;
                }

                var timeOfAlarm = time - DateTime.Now.TimeOfDay;
                if (timeOfAlarm <= TimeSpan.Zero) timeOfAlarm = TimeSpan.FromDays(1) - timeOfAlarm.Duration();

                Alarms.Add(new Alarm(Ring, timeOfAlarm));
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        private void DeleteAlarm(object sender, RoutedEventArgs e)
        {
            try
            {
                var alarm = (Alarm)((Button)sender).Tag;

                alarm.Dispatcher.Stop();
                Alarms.Remove(alarm);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                RefreshClock();
                CreateDispatcher(RefreshClock, TimeSpan.FromSeconds(1)).Start();
            }
            catch (Exception exc)
            {
                ShowError(exc);
                Application.Current.Shutdown();
            }
        }

    }
}
