using System;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Controls.Tests
{
    [TestFixture]
    public class TestFeedbackDisplayViewModel
    {
        // ReSharper disable InconsistentNaming
        [Test]
        public void Constructor_GivenNullNavigateToUrlCommand_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new FeedbackDisplayViewModel(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("command",exception.ParamName);
        }

        [Test]
        public void Constructor_ShouldInitializeNavigateToUrlCommand()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var viewModel = new FeedbackDisplayViewModel();
            //---------------Test Result -----------------------
            Assert.IsNotNull(viewModel.NavigateToUrlCommand);
        }

        [Test]
        public void Title_WhenSetShouldFirePropertyChangedEvent()
        {
            //---------------Set up test pack-------------------
            var viewModel = CreateViewModel();
            var isFired = false;
            viewModel.PropertyChanged += (sender, args) => isFired = true;
            var expected = Guid.NewGuid().ToString();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            viewModel.Title = expected;
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired);
            Assert.AreEqual(expected, viewModel.Title);
        }

        [Test]
        public void Message_WhenSetShouldFirePropertyChangedEvent()
        {
            //---------------Set up test pack-------------------
            var viewModel = CreateViewModel();
            var isFired = false;
            viewModel.PropertyChanged += (sender, args) => isFired = true;
            var expected = Guid.NewGuid().ToString();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            viewModel.Message = expected;
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired);
            expected = @"<html><head><style type=""text/css"">body { font-size: 12px; font-family:'Century Gothic'; color:#352063;} p {width: 400px;word-wrap: normal;} .hint{font-weight: bold;}</style></head><body><p>" 
                    + expected + "</p></body></html>";
            Assert.AreEqual(expected, viewModel.Message);
        }

        [Test]
        public void KataDuration_WhenSetShouldFirePropertyChangedEvent()
        {
            //---------------Set up test pack-------------------
            var viewModel = CreateViewModel();
            var isFired = false;
            viewModel.PropertyChanged += (sender, args) => isFired = true;
            var expected = Guid.NewGuid().ToString();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            viewModel.KataDuration = expected;
            //---------------Test Result -----------------------
            Assert.IsTrue(isFired);
            Assert.AreEqual(expected, viewModel.KataDuration);
        }


        [Test]
        public void Url_ShouldReturnUncleBobUrl()
        {
            //---------------Set up test pack-------------------
            var viewModel = CreateViewModel();
            var expected = "http://butunclebob.com/ArticleS.UncleBob.TheThreeRulesOfTdd"; 
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var url = viewModel.Url;
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, url);
        }

        private static FeedbackDisplayViewModel CreateViewModel()
        {
            var viewModel = new FeedbackDisplayViewModel(Substitute.For<INavigateToUrlCommand>());
            return viewModel;
        }
    }
}