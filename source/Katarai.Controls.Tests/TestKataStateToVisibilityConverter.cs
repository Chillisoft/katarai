using System.Globalization;
using System.Threading;
using System.Windows;
using Engine;
using NUnit.Framework;

namespace Katarai.Controls.Tests
{
    [TestFixture]
    public class TestKataStateToVisibilityConverter
    {
        [Test]
        [Apartment(ApartmentState.STA)]
        public void Convert_GivenKataStatePositiveAndParameterIsGreenKataState_ShouldReturnVisible()
        {
            //---------------Set up test pack-------------------
            var converter = new KataStateToVisibilitConverter();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var visibility = converter.Convert(NotifyIcon.TwoThumbs, typeof(Visibility),"Green",CultureInfo.CurrentCulture);
            //---------------Test Result -----------------------
            Assert.AreEqual(Visibility.Visible,visibility);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void Convert_GivenKataStatePositiveAndParameterIsRedKataState_ShouldReturnCollapsed()
        {
            //---------------Set up test pack-------------------
            var converter = new KataStateToVisibilitConverter();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var visibility = converter.Convert(NotifyIcon.TwoThumbs, typeof(Visibility),"Red",CultureInfo.CurrentCulture);
            //---------------Test Result -----------------------
            Assert.AreEqual(Visibility.Collapsed,visibility);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void Convert_GivenKataStateNegativeAndParameterIsGreenKataState_ShouldReturnVisible()
        {
            //---------------Set up test pack-------------------
            var converter = new KataStateToVisibilitConverter();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var visibility = converter.Convert(NotifyIcon.Warning, typeof(Visibility),"Green",CultureInfo.CurrentCulture);
            //---------------Test Result -----------------------
            Assert.AreEqual(Visibility.Collapsed,visibility);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void Convert_GivenKataStateNegativeAndParameterIsRedKataState_ShouldReturnCollapsed()
        {
            //---------------Set up test pack-------------------
            var converter = new KataStateToVisibilitConverter();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var visibility = converter.Convert(NotifyIcon.Warning, typeof(Visibility),"Red",CultureInfo.CurrentCulture);
            //---------------Test Result -----------------------
            Assert.AreEqual(Visibility.Visible,visibility);
        }
    }
}