using System;
using System.Threading;
using System.Windows;
using Caliburn.Micro;
using Katarai.Wpf.Events;
using Katarai.Wpf.Utils;
using Katarai.Wpf.ViewModels;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestWindowController
    {
        [Test]
        public void Type_ShouldImplement_IWindowController()
        {
            //---------------Set up test pack-------------------
            var sut = typeof(WindowController);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IWindowController>();

            //---------------Test Result -----------------------
        }

        [TestCase("windowManager", typeof(IConvenientWindowManager))]
        [TestCase("eventAggregator", typeof(IEventAggregator))]
        [TestCase("aboutViewModel", typeof(IAboutViewModel))]
        [TestCase("attemptsPerWeekViewModel", typeof(IAttemptsPerWeekViewModel))]
        [TestCase("attemptsViewModel", typeof(IAttemptsViewModel))]
        [TestCase("completedLengthsViewModel", typeof(ICompletedLengthsViewModel))]
        [TestCase("kataCompletedViewModel", typeof(IKataCompletedViewModel))]
        [TestCase("mainWindowViewModel", typeof(IMainWindowViewModel))]
        [TestCase("reminderSettingsViewModel", typeof(IReminderSettingsViewModel))]
        public void Construct_ShouldExpectParameter_(string parameterName, Type parameterType)
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ConstructorTestUtils.ShouldExpectNonNullParameterFor<WindowController>(parameterName, parameterType);

            //---------------Test Result -----------------------
        }


        [Test]
        public void Construct_GivenAllParameters_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => Create());

            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_ShouldInstructEventAggregatorToSubscribe()
        {
            //---------------Set up test pack-------------------
            var eventAggregator = Substitute.For<IEventAggregator>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var sut = Create(eventAggregator: eventAggregator);

            //---------------Test Result -----------------------
            eventAggregator.Received(1).Subscribe(sut);
        }



        [Test]
        public void IWindowController_ShouldImplement_IHandleShowMainWindowEvent()
        {
            //---------------Set up test pack-------------------
            var sut = typeof(IWindowController);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IHandle<ShowMainWindowEvent>>();

            //---------------Test Result -----------------------
        }

        [Test]
        public void IWindowController_ShouldImplement_IHandleHideMainWindowEvent()
        {
            //---------------Set up test pack-------------------
            var sut = typeof(IWindowController);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IHandle<HideMainWindowEvent>>();

            //---------------Test Result -----------------------
        }

        [Test]
        public void IWindowController_ShouldImplement_IHandleToggleMainWindowEvent()
        {
            //---------------Set up test pack-------------------
            var sut = typeof(IWindowController);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IHandle<ToggleMainWindowEvent>>();

            //---------------Test Result -----------------------
        }


        [Test]
        public void Handle_ShowMainWindowEvent_FirstCall_ShouldShowMainWindow()
        {
            //---------------Set up test pack-------------------
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var viewModel = Substitute.For<IMainWindowViewModel>();
            var sut = Create(windowManager: windowManager, mainWindowViewModel: viewModel);

            //---------------Assert Precondition----------------
            windowManager.Received(0).RaiseWindow(viewModel);

            //---------------Execute Test ----------------------
            sut.Handle(new ShowMainWindowEvent());

            //---------------Test Result -----------------------
            windowManager.Received(1).RaiseWindow(viewModel);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void Handle_ShowMainWindowEvent_WhenViewModelActive_ShouldJustRaise()
        {
            //---------------Set up test pack-------------------
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var viewModel = Substitute.For<IMainWindowViewModel>();
            var sut = Create(windowManager: windowManager, mainWindowViewModel: viewModel);
            viewModel.IsActive.Returns(true);
            var window = Substitute.For<Window>();
            viewModel.GetView().Returns(window);

            //---------------Assert Precondition----------------
            windowManager.Received(0).ShowWindow(viewModel);

            //---------------Execute Test ----------------------
            sut.Handle(new ShowMainWindowEvent());

            //---------------Test Result -----------------------
            windowManager.Received(0).ShowWindow(viewModel);
            window.Received().Activate();
        }

        [Test]
        public void Handle_HideMainWindowEvent_WhenViewModelIsActive_ShouldTryClose()
        {
            //---------------Set up test pack-------------------
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var viewModel = Substitute.For<IMainWindowViewModel>();
            var sut = Create(windowManager, mainWindowViewModel: viewModel);

            //---------------Assert Precondition----------------
            windowManager.DidNotReceive().HideWindow(Arg.Any<IViewModel>());

            //---------------Execute Test ----------------------
            sut.Handle(new HideMainWindowEvent());

            //---------------Test Result -----------------------
            windowManager.Received(1).HideWindow(viewModel);
        }

        [Test]
        public void Handle_ToggleMainWindowEvent_ShouldToggleTheMainWindow()
        {
            //---------------Set up test pack-------------------
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var viewModel = Substitute.For<IMainWindowViewModel>();
            var sut = Create(windowManager, mainWindowViewModel: viewModel);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.Handle(new ToggleMainWindowEvent());

            //---------------Test Result -----------------------
            windowManager.Received(1).ToggleWindow(viewModel);
        }

        [Test]
        public void InterfaceType_ShouldImplement_IHandle_SHowAboutWindowEvent()
        {
            //---------------Set up test pack-------------------
            var sut = typeof (IWindowController);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IHandle<ShowAboutWindowEvent>>();

            //---------------Test Result -----------------------
        }


        [Test]
        public void Handle_ShowAboutWindowEvent_WhenIsNotActive_ShouldShow()
        {
            //---------------Set up test pack-------------------
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var viewModel = Substitute.For<IAboutViewModel>();
            var sut = Create(windowManager, aboutViewModel: viewModel);

            //---------------Assert Precondition----------------
            Assert.IsFalse(viewModel.IsActive);

            //---------------Execute Test ----------------------
            sut.Handle(new ShowAboutWindowEvent());

            //---------------Test Result -----------------------
            windowManager.Received().RaiseWindow(viewModel);
        }

        [Test]
        public void InterfaceType_ShouldImplement_IHandleShowFeedbackWindowEvent()
        {
            //---------------Set up test pack-------------------
            var sut = typeof (IWindowController);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IHandle<ShowFeedbackWindowEvent>>();

            //---------------Test Result -----------------------
        }

        [Test]
        public void Handle_ShowFeedbackWindowEvent_ShouldRaiseFeedbackViewModel()
        {
            //---------------Set up test pack-------------------
            var windowManager = Substitute.For<IConvenientWindowManager>();
            var viewModel = Substitute.For<IFeedbackViewModel>();
            var sut = Create(windowManager, feedbackViewModel: viewModel);

            //---------------Assert Precondition----------------
            windowManager.DidNotReceive().RaiseWindow(viewModel);

            //---------------Execute Test ----------------------
            sut.Handle(new ShowFeedbackWindowEvent());

            //---------------Test Result -----------------------
            windowManager.Received(1).RaiseWindow(viewModel);
        }


        private WindowController Create(IConvenientWindowManager windowManager = null,
            IEventAggregator eventAggregator = null,
            IAboutViewModel aboutViewModel = null,
            IMainWindowViewModel mainWindowViewModel = null,
            IAttemptsPerWeekViewModel attemptsPerWeekViewModel = null,
            IAttemptsViewModel attemptsViewModel = null,
            ICompletedLengthsViewModel completedLengthsViewModel = null,
            IKataCompletedViewModel kataCompletedViewModel = null,
            IReminderSettingsViewModel reminderSettingsViewModel = null,
            IFeedbackViewModel feedbackViewModel = null)
        {
            return new WindowController(
                windowManager ?? Substitute.For<IConvenientWindowManager>(),
                eventAggregator ?? Substitute.For<IEventAggregator>(),
                mainWindowViewModel ?? Substitute.For<IMainWindowViewModel>(),
                aboutViewModel ?? Substitute.For<IAboutViewModel>(),
                attemptsPerWeekViewModel ?? Substitute.For<IAttemptsPerWeekViewModel>(),
                attemptsViewModel ?? Substitute.For<IAttemptsViewModel>(),
                completedLengthsViewModel ?? Substitute.For<ICompletedLengthsViewModel>(),
                kataCompletedViewModel ?? Substitute.For<IKataCompletedViewModel>(),
                reminderSettingsViewModel ?? Substitute.For<IReminderSettingsViewModel>(),
                feedbackViewModel ?? Substitute.For<IFeedbackViewModel>()
                );
        }
    }
}
