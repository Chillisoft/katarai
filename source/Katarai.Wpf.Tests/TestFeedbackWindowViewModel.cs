using System;
using Katarai.Wpf.Commands;
using Katarai.Wpf.ViewModels;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestFeedbackWindowViewModel
    {
        // ReSharper disable InconsistentNaming
        [Test]
        public void Constructor_GivenNullLogFeedbackCommand_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new FeedbackViewModel(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("logFeedbackCommand", exception.ParamName);
        }

        [Test]
        public void Feedback_WhenSet_ShouldReturnFeedback()
        {
            //---------------Set up test pack-------------------
            var viewModel = CreateViewModel();
            var expected = RandomValueGen.GetRandomString();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            viewModel.Feedback = expected;
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, viewModel.Feedback);
        }

        [Test]
        public void ViewModel_WhenNewKataAttempt_ShouldSendNotification()
        {
            //---------------Set up test pack-------------------
            var command = Substitute.For<ILogFeedbackCommand>();
            var wasCalled = false;
            var viewModel = CreateViewModel(command);
            viewModel.RequestClose += (sender, args) => { wasCalled = true; };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            command.FeedbackLogged += Raise.EventWith(command, EventArgs.Empty);
            //---------------Test Result -----------------------
            Assert.IsTrue(wasCalled);
            
        }

        private FeedbackViewModel CreateViewModel(ILogFeedbackCommand logFeedbackCommand)
        {
            return new FeedbackViewModel(logFeedbackCommand);
        }

        private FeedbackViewModel CreateViewModel()
        {
            return CreateViewModel(Substitute.For<ILogFeedbackCommand>());
        }
    }
}