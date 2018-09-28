using System;
using NUnit.Framework;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestKataTimer
    {
        [Test]
        public void Constructor_ShouldCreateTimer()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var kataTimer = new KataTimer();
            //---------------Test Result -----------------------
            Assert.IsNotNull(kataTimer.Timer);
            Assert.IsFalse(kataTimer.Timer.IsEnabled);
        }

        [Test]
        public void Constructor_ShouldSetKataDurationToZero()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var kataTimer = CreateKataTimer();
            //---------------Test Result -----------------------
            Assert.AreEqual("00:00:00", kataTimer.KataDuration);
        }

        [Test]
        public void StartTimer_ShouldStartTimer()
        {
            //---------------Set up test pack-------------------
            var kataTimer = CreateKataTimer();
            //---------------Assert Precondition----------------
            Assert.AreEqual(DateTime.MinValue, kataTimer.StartTime);
            //---------------Execute Test ----------------------
            kataTimer.StartTimer();
            //---------------Test Result -----------------------
            Assert.IsTrue(kataTimer.Timer.IsEnabled);
            Assert.AreNotEqual(DateTime.MinValue, kataTimer.StartTime);
        }

        [Test]
        public void StopTimer_ShouldStopTimer()
        {
            //---------------Set up test pack-------------------
            var kataTimer = CreateKataTimer();
            kataTimer.StartTimer(); 
            //---------------Assert Precondition----------------
            Assert.IsTrue(kataTimer.Timer.IsEnabled);
            //---------------Execute Test ----------------------
            kataTimer.StopTimer();
            //---------------Test Result -----------------------
            Assert.IsFalse(kataTimer.Timer.IsEnabled);
        }

        [Test]
        public void ResetTimer_ShouldStopTimer()
        {
            //---------------Set up test pack-------------------
            var kataTimer = CreateKataTimer();
            kataTimer.StartTimer();
            //---------------Assert Precondition----------------
            Assert.IsTrue(kataTimer.Timer.IsEnabled);
            //---------------Execute Test ----------------------
            kataTimer.ResetTimer();
            //---------------Test Result -----------------------
            Assert.IsFalse(kataTimer.Timer.IsEnabled);
        }
        
        private static KataTimer CreateKataTimer()
        {
            return new KataTimer();
        }
    }
}