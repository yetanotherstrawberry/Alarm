using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaspAlarm.Models;
using RaspAlarm.ViewModels;
using System;
using System.Linq;

namespace RaspAlarm.Tests
{
    [TestClass]
    public class UnitTests
    {

        private readonly MainWindowViewModel vm;

        public UnitTests()
        {
            vm = new MainWindowViewModel();
            /*
             * Stop all DispatcherTimers - as they are part of C# no testing is required.
             * It is hard to test DispatcherTimers as awaiting anything will block the test thread and
             * the binary is not a WPF application, so Dispatcher does not exist (cannot use Dispatcher.Yield()).
             */
            vm.DisposeCommand.Execute(null);
        }

        [TestMethod]
        public void CheckAlarmListAddRemove()
        {
            // Alarms that do nothing and do it now.
            var alarm1 = new Alarm(_ => { }, TimeSpan.Zero);
            var alarm2 = new Alarm(_ => { }, TimeSpan.Zero);

            vm.Alarms.Add(alarm1);
            Assert.IsTrue(vm.Alarms.Contains(alarm1));
            vm.DeleteAlarmCommand.Execute(alarm1);
            Assert.IsFalse(vm.Alarms.Contains(alarm1));

            vm.Alarms.Add(alarm1);
            vm.Alarms.Add(alarm2);
            Assert.IsTrue(vm.Alarms.Count == 2);
            vm.DisposeCommand.Execute(null);
            Assert.IsFalse(vm.Alarms.Any());

            alarm1.Stop();
            alarm2.Stop();
        }

    }
}
