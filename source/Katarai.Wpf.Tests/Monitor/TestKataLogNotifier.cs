using System;
using System.Collections.Generic;
using Engine;
using Katarai.Wpf.Monitor;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Monitor
{
    [TestFixture]
    public class TestKataLogNotifier
    {
        readonly Stack<IDisposable> _itemsToDispose = new Stack<IDisposable>();

        [TearDown]
        public void TesrDown()
        {
            while (_itemsToDispose.Count > 0)
            {
                var disposable = _itemsToDispose.Pop();
                disposable.Dispose();
            }
        }
        
        [Test]
        public void Constructor_GivenNullLogTarget_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var underlyingNotifier = Substitute.For<IPlayerNotifier>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new KataLogPlayerNotifier(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("logTarget", exception.ParamName);
        }

        [Test]
        public void CheckForAndDisplayErrorMessage_ShouldCallUnderlyingLogTarget()
        {
            //---------------Set up test pack-------------------
            var underlyingLogTarget = Substitute.For<ILogTarget>();
            var notifier = CreateNotifier(logTarget: underlyingLogTarget);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            notifier.DisplayErrorMessage("Some Message");
            //---------------Test Result -----------------------
            underlyingLogTarget.Received().LogError("Some Message");
        }

        [Test]
        public void DisplayMessage_ShouldCallUnderlyingLogTarget()
        {
            //---------------Set up test pack-------------------
            var underlyingLogTarget = Substitute.For<ILogTarget>();
            var notifier = CreateNotifier(logTarget: underlyingLogTarget);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            notifier.DisplayMessage("My Title", "Some Message", NotifyIcon.Red,NotifyIcon.TwoThumbs);
            //---------------Test Result -----------------------
            underlyingLogTarget.Received().LogInfo("(Red) My Title - Some Message");
        }

        [Test]
        public void Dispose_ShouldCallUnderlyingLogTarget()
        {
            //---------------Set up test pack-------------------
            var underlyingLogTarget = Substitute.For<ILogTarget>();
            var notifier = CreateNotifier(logTarget: underlyingLogTarget);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            notifier.Dispose();
            //---------------Test Result -----------------------
            underlyingLogTarget.Received().Dispose();
        }


        private KataLogPlayerNotifier CreateNotifier(ILogTarget logTarget = null)
        {
            logTarget = logTarget ?? Substitute.For<ILogTarget>();
            var loggingNotifier = new KataLogPlayerNotifier(logTarget);
            _itemsToDispose.Push(loggingNotifier);
            return loggingNotifier;

        }

    }
}