using System;
using Caliburn.Micro;
using Engine;
using Katarai.Wpf.Events;
using Katarai.Wpf.Monitor;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace Katarai.Wpf.Tests.Monitor
{
    [TestFixture]
    public class TestPlayerNotifier
    {
        [Test]
        public void Constructor_GivenNullToastDisplayer_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new PlayerNotifier(null, null));
            //---------------Test Result -----------------------
            Assert.AreEqual("notifyIcon",exception.ParamName);
        }

        [Test]
        public void Construct_GivenNullEventAggregator_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => new PlayerNotifier(Substitute.For<IToastDisplayer>(), (IEventAggregator) null));

            //---------------Test Result -----------------------
            Assert.AreEqual("eventAggregator", ex.ParamName);
        }


        [Test]
        public void DisplayErrorMessage_GivenMessage_ShouldEmitDisplayFeedbackEvent()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            using (var playerNotifier = CreatePlayerNotifier(eventAggregator: eventAggregator))
            {
                //---------------Assert Precondition----------------
                var error = RandomValueGen.GetRandomString();
                var expected = string.Format("Error :: {0}", error);

                //---------------Execute Test ----------------------
                playerNotifier.DisplayErrorMessage(error);
                //---------------Test Result -----------------------
                eventAggregator.ShouldHavePublished<DisplayFeedbackEvent>(m => m.Message == expected);
            }
        }

        [Test]
        public void DisplayErrorMessage_GivenAnotherMessage_ShouldNotEmitSecondDisplayFeedbackEvent()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            var playerNotifier = CreatePlayerNotifier(eventAggregator: eventAggregator);
            var firstMessage = RandomValueGen.GetRandomString();
            var secondMessage = firstMessage + RandomValueGen.GetRandomString(2);
            Func<string, string> expectedErrorMessageFor = s => "Error :: " + s;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            playerNotifier.DisplayErrorMessage(firstMessage);
            playerNotifier.DisplayErrorMessage(secondMessage);
            //---------------Test Result -----------------------
            eventAggregator.ShouldHavePublished<DisplayFeedbackEvent>(m => m.Message == expectedErrorMessageFor(firstMessage));
            eventAggregator.ShouldNotHavePublished<DisplayFeedbackEvent>(m => m.Message == expectedErrorMessageFor(secondMessage));
        }

        [Test]
        public void DisplayMessage_GivenMessage_ShouldEmitDisplayFeedbackEVent()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();
            var playerNotifier = CreatePlayerNotifier(eventAggregator: eventAggregator);
            var expected = RandomValueGen.GetRandomString();
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            playerNotifier.DisplayMessage("A", expected, NotifyIcon.Red,NotifyIcon.Warning);
            //---------------Test Result -----------------------
            eventAggregator.ShouldHavePublished<DisplayFeedbackEvent>(m => m.Message == expected);
        }

        [Test]
        public void Dispose_ShouldDisposeTrayIcon()
        {
            //---------------Set up test pack-------------------
            var taskbarIcon = Substitute.For<IToastDisplayer>();
            var playerNotifier = CreatePlayerNotifier(taskbarIcon);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            playerNotifier.Dispose();
            //---------------Test Result -----------------------
            taskbarIcon.Received().Dispose();
        }

        private PlayerNotifier CreatePlayerNotifier(IToastDisplayer toastDisplayer = null, IEventAggregator eventAggregator = null)
        {
            return new PlayerNotifier(toastDisplayer ?? Substitute.For<IToastDisplayer>(),
                eventAggregator ?? Substitute.For<IEventAggregator>());
        }
    }
}