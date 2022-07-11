using Microsoft.VisualBasic;
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

        public class Alarm
        {
            public Alarm(DispatcherTimer dispatcher)
            {
                Dispatcher = dispatcher;
                Dispatcher.Tag = this;
            }

            public DispatcherTimer Dispatcher { get; }
            public string Time
            {
                get => DateTime.Now.AddTicks(Dispatcher.Interval.Ticks).ToString();
            }
        }

        public ObservableCollection<Alarm> Alarms { get; } = new ObservableCollection<Alarm>();

        private const int SnoozeTime = 5, AlarmFreq = 500, AlarmBeepTime = 3000, AlarmDelay = 2000;

        private void RefreshClock(object source = null, EventArgs ea = null) => Clock.Content = DateTime.Now.ToLongTimeString();

        private DispatcherTimer CreateDispatcher(EventHandler handler, TimeSpan interval) =>
            new DispatcherTimer(
                callback: handler,
                interval: interval,
                priority: DispatcherPriority.Render,
                dispatcher: Dispatcher.CurrentDispatcher
            );

        private void ShowError(Exception exc) => ShowError(exc.Message);
        private void ShowError(string text) => MessageBox.Show(owner: this, text, Properties.Resources.ERR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);

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
                new Task(async () => {
                    while (!token.IsCancellationRequested) {
                        Console.Beep(AlarmFreq, AlarmBeepTime);
                        await Task.Delay(AlarmDelay);
                    }
                }, token).Start();

                if (MessageBoxResult.Yes.Equals(MessageBox.Show(string.Format(Properties.Resources.ALARM_MSGBOX_CONTENT, SnoozeTime), Properties.Resources.ALARM_MSGBOX_TITLE, MessageBoxButton.YesNo, MessageBoxImage.Question)))
                {
                    dispatcher.Interval = TimeSpan.FromMinutes(SnoozeTime);
                    dispatcher.Start();
                    Alarms.Add(alarm);
                }

                tokenSource.Cancel();

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

                string userInput = Interaction.InputBox(Properties.Resources.INTERACTION_NEWALARM_CONTENT, Properties.Resources.INTERACTION_NEWALARM_TITLE);

                if (string.IsNullOrEmpty(userInput)) return;

                if (!TimeSpan.TryParse(userInput, out var time))
                {
                    ShowError(Properties.Resources.INVALID_TIME);
                    return;
                }

                TimeSpan timeOfAlarm = time - DateTime.Now.TimeOfDay;
                if (timeOfAlarm <= TimeSpan.Zero) timeOfAlarm = TimeSpan.FromDays(1) - timeOfAlarm.Duration();

                var alarm = CreateDispatcher(Ring, timeOfAlarm);

                Alarms.Add(new Alarm(alarm));

                alarm.Start();

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
