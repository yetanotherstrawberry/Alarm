using RaspAlarm.Helpers;
using System;
using System.Windows.Threading;

namespace RaspAlarm.Models
{
    internal class Alarm
    {

        /// <summary>
        /// The DispatcherTimer responsible for calling supplied method after the countdown.
        /// </summary>
        private readonly DispatcherTimer DispatchTimer;

        /// <summary>
        /// The time at which the alarm will ring.
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// Calculates and assignes <c>Time</c>.
        /// </summary>
        private void SetTime()
        {
            Time = DateTime.Now.Add(DispatchTimer.Interval);
        }

        /// <summary>
        /// Created a new alarm and starts the countdown from the interval to the execution of the ring method.
        /// </summary>
        /// <param name="ring">Callback method for ringing.</param>
        /// <param name="interval">The time from DateTime.Now it should take for the alarm to ring.</param>
        /// <param name="dispatcher">Dispatcher of the thread the DispatcherTimer should use.</param>
        public Alarm(Action<Alarm> ring, TimeSpan interval)
        {
            DispatchTimer = DispatcherHelper.StartDispatchTimer(() => ring(this), interval);
            SetTime();
        }

        /// <summary>
        /// Stops the countdown of this alarm.
        /// </summary>
        public void Stop()
        {
            DispatchTimer.Stop();
        }

        /// <summary>
        /// Executes Stop(), sets the new interval and restarts the countdown.
        /// </summary>
        /// <param name="interval">Sets the time it takes for this alarm to ring.</param>
        public void Reset(TimeSpan interval)
        {
            Stop();
            DispatchTimer.Interval = interval;
            SetTime();
            DispatchTimer.Start();
        }

    }
}
