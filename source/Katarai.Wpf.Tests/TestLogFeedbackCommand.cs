using System;
using Katarai.Wpf.Commands;
using NUnit.Framework;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestLogFeedbackCommand
    {
        [Test]
        public void Constructor_GivenNullLogger_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new LogFeedbackCommand(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("splunkLogger", exception.ParamName);
        }
    }
}