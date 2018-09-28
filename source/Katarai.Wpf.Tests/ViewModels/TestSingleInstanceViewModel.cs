using System.ComponentModel;
using System.Threading;
using Caliburn.Micro;
using Katarai.Wpf.Extensions;
using Katarai.Wpf.ViewModels;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.ViewModels
{
    [TestFixture]
    public class TestSingleInstanceViewModel
    {
        [Test]
        public void ShouldBeAbstract()
        {
            //---------------Set up test pack-------------------
            var sut = typeof (SingleInstanceViewModel);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.IsTrue(sut.IsAbstract);

            //---------------Test Result -----------------------
        }

        public class SomeViewModel: SingleInstanceViewModel
        {
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void OnClose_ShouldJustHide()
        {
            //---------------Set up test pack-------------------
            var windowManager = new WindowManager();
            var sut = new SomeViewModel();
            var args = new CancelEventArgs()
            {
                Cancel = false
            };

            //---------------Assert Precondition----------------
            windowManager.ShowWindow(sut);
            Assert.IsTrue(sut.GetWindow().IsActive);
            Assert.IsTrue(sut.GetWindow().IsVisible);

            //---------------Execute Test ----------------------
            sut.OnClose(new object(), args);

            //---------------Test Result -----------------------
            Assert.IsTrue(args.Cancel);
            Assert.IsFalse(sut.GetWindow().IsVisible);
        }

    }
}
