using RaspAlarm.Helpers;
using RaspAlarm.Interfaces;
using RaspAlarm.Models;
using RaspAlarm.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace RaspAlarm.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {

        #region Constants
        private const int SnoozeMinutes = 5, AlarmFreq = 500, AlarmBeepTime = 3000, AlarmDelay = 2000, RefreshSec = 1;
        #endregion Constants

        #region Fields
        /// <summary>
        /// <c>DispatcherTimer</c> that constantly updates the <c>Time</c> field.
        /// </summary>
        private readonly DispatcherTimer timer;

        /// <summary>
        /// Field used by <c>Time</c> property.
        /// </summary>
        private DateTime time = DateTime.Now;
        #endregion Fields

        #region Events
        /// <summary>
        /// Event fired every time a notifying property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Events

        #region Commands
        /// <summary>
        /// Command executed after clicking a new alarm button.
        /// </summary>
        public ICommand NewAlarmCommand { get; }

        /// <summary>
        /// Deletes supplied alarm after clicking the delete button.
        /// </summary>
        public ICommand DeleteAlarmCommand { get; }
        #endregion Commands

        #region Properties
        /// <summary>
        /// Current time refreshed constantly. Notifies listeners on change.
        /// </summary>
        public DateTime Time
        {
            get => time;
            private set
            {
                time = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Language/culture setting. Used by ToString(). Value taken from the OS.
        /// </summary>
        public XmlLanguage Language { get; } = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);

        /// <summary>
        /// Observable collection of alarms that were set by the user.
        /// </summary>
        public ObservableCollection<Alarm> Alarms { get; } = new ObservableCollection<Alarm>();
        #endregion Properties

        #region Methods
        /// <summary>
        /// Notifies listeners that the property has been changed.
        /// </summary>
        /// <param name="propertyName">Name of the changed property. Leave empty if called from a setter.</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Creates a message box that waits for the user to choose whether they want to snooze.
        /// Plays an asynchronous sound in a loop which waites for the message box to be closed.
        /// Removes alarm from the list and inserts it again if user chooses to snooze.
        /// </summary>
        /// <param name="alarm">
        /// The triggered instance of Alarm.
        /// Will be modified if user chooses to snooze.
        /// </param>
        private async void Ring(Alarm alarm)
        {
            try
            {
                alarm.Stop();
                Alarms.Remove(alarm);

                using (var tokenSource = new CancellationTokenSource())
                {
                    var ring = Task.Run(async () =>
                    {
                        while (!tokenSource.Token.IsCancellationRequested)
                        {
                            Console.Beep(AlarmFreq, AlarmBeepTime);
                            await Task.Delay(AlarmDelay, tokenSource.Token);
                        }
                    });

                    var userReply = MessageBox.Show(
                            string.Format(Resources.ALARM_MSGBOX_CONTENT, SnoozeMinutes),
                            Resources.ALARM_MSGBOX_TITLE,
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question,
                            MessageBoxResult.No);

                    tokenSource.Cancel();

                    if (userReply == MessageBoxResult.Yes)
                    {
                        alarm.Reset(TimeSpan.FromMinutes(SnoozeMinutes));
                        Alarms.Add(alarm);
                    }

                    await ring; // Stops token from being disposed before the task ends.
                }
            }
            catch (Exception exc)
            {
                if (!(exc is OperationCanceledException))
                    ErrorHelper.ShowError(exc);
            }
        }

        /// <summary>
        /// Creates a new alarm and adds it to the list.
        /// </summary>
        /// <param name="dateTime">Time the alarm is to be set at.</param>
        private void NewAlarm(string dateTime)
        {
            if (string.IsNullOrWhiteSpace(dateTime))
                return;

            // Throw error if parsing failed or the date is earlier than today's midnight.
            if (!DateTime.TryParse(dateTime, out DateTime time) || time < DateTime.Now.Date)
            {
                ErrorHelper.ShowError(Resources.INVALID_TIME);
                return;
            }

            // Add 1 day if the time is earlier than current.
            if (time < DateTime.Now) time = time.AddDays(1);

            var timeOfAlarm = time - DateTime.Now;

            Alarms.Add(new Alarm(Ring, timeOfAlarm));
        }

        /// <summary>
        /// Stops all alarms in the parameter.
        /// </summary>
        /// <param name="alarms">Alarms to be stopped.</param>
        private void StopAlarms(IEnumerable<Alarm> alarms)
        {
            foreach (var alarm in alarms)
                StopAlarm(alarm);
        }

        /// <summary>
        /// Stops the alarm in the parameter.
        /// </summary>
        /// <param name="alarms">Instance of <c>Alarm</c> to be stopped.</param>
        private void StopAlarm(Alarm alarm)
        {
            alarm.Stop();
        }

        /// <summary>
        /// Stops all <c>DispatcherTimer</c>s of alarms and clock.
        /// </summary>
        public void Dispose()
        {
            StopAlarms(Alarms);
            timer.Stop();
        }
        #endregion Methods

        #region ConstructorDestructor
        /// <summary>
        /// Initiates fields and starts <c>DispatcherTimer</c> that refreshes the time.
        /// Implements commands that access fields.
        /// </summary>
        public MainWindowViewModel()
        {
            NewAlarmCommand = new DelegateCommand<IDialog>(dialog =>
            {
                NewAlarm(dialog.ShowDialog());
            });
            DeleteAlarmCommand = new DelegateCommand<Alarm>(alarm =>
            {
                Alarms.Remove(alarm);
                StopAlarm(alarm);
            });
            timer = DispatcherHelper.StartDispatchTimer(
                () => Time = DateTime.Now,
                TimeSpan.FromSeconds(RefreshSec));
        }

        /// <summary>
        /// Dispose the ViewModel just in case Dispose() was not executed.
        /// </summary>
        ~MainWindowViewModel()
        {
            Dispose();
        }
        #endregion ConstructorDestructor

    }
}
