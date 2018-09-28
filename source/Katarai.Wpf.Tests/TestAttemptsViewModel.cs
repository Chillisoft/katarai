using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Katarai.Wpf.ViewModels;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestAttemptsViewModel
    {
        // ReSharper disable InconsistentNaming
        [Test]
        public void Constructor_GivenNullLogRepository_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new AttemptsViewModel(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("logRepository", exception.ParamName);
        }

        [Test]
        public void EmptyConstructor_ShouldCallOtherConstructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var attemptsViewModel = new AttemptsViewModel();
            //---------------Test Result -----------------------
            Assert.IsNotNull(attemptsViewModel.LogRepository);
        }

        [Test]
        public void Constructor_ShouldCallGetKataAttemptLogs()
        {
            //---------------Set up test pack-------------------
            var logRepository = Substitute.For<ILogRepository>();
            var attemptLogs = new List<IAttemptLog>();
            var task = new Task<List<IAttemptLog>>(() => attemptLogs);
            logRepository.GetKataAttemptLogs(Arg.Any<string>(), 20).Returns(task);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var viewModel = new AttemptsViewModel(logRepository);
            //---------------Test Result -----------------------
            logRepository.Received(1).GetKataAttemptLogs(Arg.Any<string>(), 20);
        }

        [Test]
        public void LoadingMessage_WhenSet_ShouldNotifyPropertyChanged()
        {
            //---------------Set up test pack-------------------
            var logRepository = Substitute.For<ILogRepository>();
            //var attemptLogs = new List<IAttemptLog>();
            //var task = new Task<List<IAttemptLog>>(() => attemptLogs);
            //logRepository.GetKataAttemptLogs(Arg.Any<string>()).Returns(task);
            var propNames = new List<string>();
            var viewModel = CreateViewModel(logRepository);
            viewModel.PropertyChanged += (sender, args) => propNames.Add(args.PropertyName);
            var loadingMessage = RandomValueGen.GetRandomString();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            viewModel.LoadingMessage = loadingMessage;
            //---------------Test Result -----------------------
            CollectionAssert.Contains(propNames, "LoadingMessage");
            Assert.AreEqual(loadingMessage, viewModel.LoadingMessage);
        }

        [Test]
        public void AttemptLogs_WhenSet_ShouldNotifyPropertyChanged()
        {
            //---------------Set up test pack-------------------
            var propNames = new List<string>();
            var viewModel = CreateViewModel();
            viewModel.PropertyChanged += (sender, args) => propNames.Add(args.PropertyName);
            var attemptLogs = new List<IAttemptLog>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            
            viewModel.AttemptLogs = attemptLogs;
            //---------------Test Result -----------------------
            CollectionAssert.Contains(propNames, "AttemptLogs");
            Assert.AreSame(attemptLogs, viewModel.AttemptLogs);
        }

        private AttemptsViewModel CreateViewModel(ILogRepository logRepository)
        {
            return new AttemptsViewModel(logRepository);
        }

        private AttemptsViewModel CreateViewModel()
        {
            return CreateViewModel(Substitute.For<ILogRepository>());
        }
    }
}