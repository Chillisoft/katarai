using System;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestMonitorTimer
    {
        [Test]
        public void Constructor_GivenNullLogger_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>  new MonitorTimer(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("splunkLogger", exception.ParamName);
        }

        [Test]
        public void Constructor_ShouldCreateTimer()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var monitorTimer = new MonitorTimer(Substitute.For<ISplunkLogger>());
            //---------------Test Result -----------------------
            Assert.IsFalse(monitorTimer.IsTimerRunning());
        }

        [Test]
        public void Start_ShouldStartTimer()
        {
            //---------------Set up test pack-------------------
            var monitorTimer = CreateMonitorTimer();
            //---------------Assert Precondition----------------
            Assert.IsFalse(monitorTimer.IsTimerRunning());
            //---------------Execute Test ----------------------
            monitorTimer.Start();
            //---------------Test Result -----------------------
            Assert.IsTrue(monitorTimer.IsTimerRunning());
        }

        [Test]
        public void Stop_ShouldStopTimer()
        {
            //---------------Set up test pack-------------------
            var monitorTimer = CreateMonitorTimer();
            monitorTimer.Start();
            //---------------Assert Precondition----------------
            Assert.IsTrue(monitorTimer.IsTimerRunning());
            //---------------Execute Test ----------------------
            monitorTimer.Stop();
            //---------------Test Result -----------------------
            Assert.IsFalse(monitorTimer.IsTimerRunning());
        }
        
        private MonitorTimer CreateMonitorTimer()
        {
            return CreateMonitorTimer(Substitute.For<ISplunkLogger>());
        }

        private MonitorTimer CreateMonitorTimer(ISplunkLogger splunkLogger)
        {
            return new MonitorTimer(splunkLogger);
        }

    }
}