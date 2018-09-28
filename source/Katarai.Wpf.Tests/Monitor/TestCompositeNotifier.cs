using System;
using System.Collections.Generic;
using Engine;
using Katarai.Wpf.Monitor;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Monitor
{
    [TestFixture]
    public class TestCompositeNotifier
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
        public void Constructor_GivenNullNotifiers_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var logTarget = Substitute.For<ILogTarget>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new CompositePlayerNotifier(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("playerNotifiers", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenNotifiersWithNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new CompositePlayerNotifier(Substitute.For<IPlayerNotifier>(), null, Substitute.For<IPlayerNotifier>())
                );
            //---------------Test Result -----------------------
            Assert.AreEqual("playerNotifier(1)", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenManyNotifiersWithNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new CompositePlayerNotifier(Substitute.For<IPlayerNotifier>(), null, null, Substitute.For<IPlayerNotifier>())
                );
            //---------------Test Result -----------------------
            Assert.AreEqual("playerNotifier(1), playerNotifier(2)", exception.ParamName);
        }

        [Test]
        public void CheckForAndDisplayErrorMessage_ShouldCallUnderlyingNotifier()
        {
            //---------------Set up test pack-------------------
            var underlyingNotifier = Substitute.For<IPlayerNotifier>();
            var notifier = CreateNotifier(playerNotifier: underlyingNotifier);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            notifier.DisplayErrorMessage("Some Message");
            //---------------Test Result -----------------------
            underlyingNotifier.Received().DisplayErrorMessage("Some Message");
        }

        [Test]
        public void DisplayMessage_ShouldCallUnderlyingNotifier()
        {
            //---------------Set up test pack-------------------
            var underlyingNotifier = Substitute.For<IPlayerNotifier>();
            var notifier = CreateNotifier(playerNotifier: underlyingNotifier);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            notifier.DisplayMessage("My title", "My message", NotifyIcon.Red,NotifyIcon.Warning);
            //---------------Test Result -----------------------
            underlyingNotifier.Received().DisplayMessage("My title", "My message", NotifyIcon.Red, NotifyIcon.Warning);
        }
        
        [Test]
        public void Dispose_ShouldCallUnderlyingNotifier()
        {
            //---------------Set up test pack-------------------
            var underlyingNotifier = Substitute.For<IPlayerNotifier>();
            var notifier = CreateNotifier(playerNotifier: underlyingNotifier);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            notifier.Dispose();
            //---------------Test Result -----------------------
            underlyingNotifier.Received().Dispose();
        }


        private CompositePlayerNotifier CreateNotifier(IPlayerNotifier playerNotifier = null)
        {
            playerNotifier = playerNotifier ?? Substitute.For<IPlayerNotifier>();
            var loggingNotifier = new CompositePlayerNotifier(playerNotifier);
            _itemsToDispose.Push(loggingNotifier);
            return loggingNotifier;

        }

    }
}