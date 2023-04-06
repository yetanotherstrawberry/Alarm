using System;
using System.Windows.Threading;

namespace RaspAlarm.Helpers
{
    internal static class DispatcherHelper
    {

        /// <summary>
        /// Creates and starts a new DispatcherTimer with render priority.
        /// </summary>
        /// <param name="callback">Method that will be executed after finishing the countdown.</param>
        /// <param name="interval">The time it takes for the countdown to finish.</param>
        /// <param name="dispatcher">Dispatcher of the thread that should be used by the DispatcherTimer.</param>
        /// <returns>Started DispatcherTimer of render priority with appropriate properties set.</returns>
        public static DispatcherTimer StartDispatchTimer(Action callback, TimeSpan interval)
        {
            var ret = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = interval,
                IsEnabled = true,
            };
            ret.Tick += (sender, args) => callback();
            ret.Start();
            return ret;
        }

    }
}
