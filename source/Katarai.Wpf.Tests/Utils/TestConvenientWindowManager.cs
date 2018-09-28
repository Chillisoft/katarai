using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using Katarai.Wpf.Commands;
using Katarai.Wpf.Extensions;
using Katarai.Wpf.Utils;
using Katarai.Wpf.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Utils
{
    [TestFixture]
    [Ignore("Run manually: these tests actively show windows and seem to have issues running after each other in R#")]
    public class TestConvenientWindowManager
    {
        [Test]
        [Apartment(ApartmentState.STA)]
        public void ToggleWindow_WhenViewIsNotActive_ShouldShowWindow()
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<IMainWindowViewModel>();
            viewModel.IsActive.Returns(false);
            var window = Substitute.For<Window>();
            viewModel.GetView().Returns(window);
            var sut = Substitute.For<ConvenientWindowManager>();

            //---------------Assert Precondition----------------
            Assert.IsFalse(window.IsFocused);

            //---------------Execute Test ----------------------
            sut.ToggleWindow(viewModel);

            //---------------Test Result -----------------------
            sut.Received(1).ShowWindow(viewModel);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void ToggleWindow_WhenViewIsActive_ShouldHideIt()
        {
            //---------------Set up test pack-------------------
            var viewModel = new AboutViewModel();
            var sut = Create();
            sut.ShowWindow(viewModel);

            //---------------Assert Precondition----------------
            var window = viewModel.GetView() as Window;
            Assert.IsTrue(window.IsVisible);

            //---------------Execute Test ----------------------
            sut.ToggleWindow(viewModel);

            //---------------Test Result -----------------------
            Assert.IsFalse(window.IsVisible);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void RaiseWindow_WhenViewInactive_ShouldShowIt()
        {
            //---------------Set up test pack-------------------
            var viewModel = new AboutViewModel();
            var sut = Create();
            sut.RaiseWindow(viewModel);
            var window = viewModel.GetWindow();
            sut.HideWindow(viewModel);

            //---------------Assert Precondition----------------
            Assert.IsFalse(window.IsVisible);

            //---------------Execute Test ----------------------
            sut.RaiseWindow(viewModel);

            //---------------Test Result -----------------------
            Assert.IsTrue(window.IsVisible);
        }

        public class Win32Api
        {
            [DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void RaiseWindow_WhenViewIsNotOnTop_ShouldRaiseIt()
        {
            //---------------Set up test pack-------------------
            var viewModel1 = new AboutViewModel();
            var viewModel2 = new FeedbackViewModel(Substitute.For<ILogFeedbackCommand>());
            var sut = Create();
            sut.RaiseWindow(viewModel1);

            //---------------Assert Precondition----------------
            WaitForWindowOn(viewModel1);
            var vm1Handle = new WindowInteropHelper(viewModel1.GetWindow()).Handle;
            var currentTop = Win32Api.GetForegroundWindow();
            Assert.AreEqual(vm1Handle, currentTop);

            //---------------Execute Test ----------------------
            sut.RaiseWindow(viewModel2);
            WaitForWindowOn(viewModel2);
            Assert.AreNotEqual(vm1Handle, Win32Api.GetForegroundWindow());
            sut.RaiseWindow(viewModel1);

            //---------------Test Result -----------------------
            Assert.AreEqual(vm1Handle, Win32Api.GetForegroundWindow());
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void HideWindow_ShouldHideWindow()
        {
            //---------------Set up test pack-------------------
            var viewModel = new AboutViewModel();
            var sut = Create();
            sut.ShowWindow(viewModel);

            //---------------Assert Precondition----------------
            WaitForWindowOn(viewModel);
            Assert.IsTrue(viewModel.GetWindow().IsVisible);
            Assert.AreEqual(Win32Api.GetForegroundWindow(), new WindowInteropHelper(viewModel.GetWindow()).Handle);

            //---------------Execute Test ----------------------
            sut.HideWindow(viewModel);

            //---------------Test Result -----------------------
            Assert.IsFalse(viewModel.GetWindow().IsVisible);
        }

        private void WaitForWindowOn(IViewModel viewModel)
        {
            for (var i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                if (viewModel.GetWindow() != null)
                    return;
            }
            Assert.Fail("Waited 10s but got no window");
        }


        private IConvenientWindowManager Create()
        {
            return new ConvenientWindowManager();
        }

    }
}
