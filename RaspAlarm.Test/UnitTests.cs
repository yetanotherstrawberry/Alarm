using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaspAlarm.Models;
using RaspAlarm.ViewModels;
using System;
using System.Threading;

namespace RaspAlarm.Tests
{
    /// <summary>
    /// Tests for RaspAlarm.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// <c>DataContext</c> of the <c>Window</c> of the aplication.
        /// </summary>
        private readonly MainWindowViewModel vm;

        public UnitTests()
        {
            vm = new MainWindowViewModel();
            /*
             * Stop all DispatcherTimers - as they are part of C# no testing is required.
             * It is hard to test DispatcherTimers as awaiting anything will block the test thread and
             * the binary is not a WPF application, so Dispatcher does not exist (cannot use Dispatcher.Yield()).
             */
            vm.Dispose();
        }

        /// <summary>
        /// Checks adding and deleting of alarms.
        /// </summary>
        [TestMethod]
        public void CheckAlarmListAddRemove()
        {
            // Alarms that do nothing and do it now.
            var alarm1 = new Alarm(_ => { }, TimeSpan.Zero);
            var alarm2 = new Alarm(_ => { }, TimeSpan.Zero);

            vm.Alarms.Add(alarm1);
            vm.Alarms.Add(alarm2);
            Assert.IsTrue(vm.Alarms.Count == 2);
            vm.Dispose(); // Stop added alarms.

            vm.DeleteAlarmCommand.Execute(alarm1);
            vm.DeleteAlarmCommand.Execute(alarm2);
            Assert.IsTrue(vm.Alarms.Count == 0);
        }

        /// <summary>
        /// Checks if application language is equal to current culture.
        /// </summary>
        [TestMethod]
        public void LangTest()
        {
            Assert.IsTrue(vm.Language.GetSpecificCulture().Equals(Thread.CurrentThread.CurrentCulture));
        }

    }
}
