using System.Globalization;
using System.Threading;
using System.Windows;
using Engine;
using NUnit.Framework;

namespace Katarai.Controls.Tests
{
    [TestFixture]
    public class TestPlayerTestStateToVisibilityConverter
    {
        [Test]
        [Apartment(ApartmentState.STA)]
        public void Convert_GivenPlayerTestStateIsPositiveAndParameterIsGreen_ShouldReturnVisible()
        {
            //---------------Set up test pack-------------------
            var converter = CreateConverter();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var visibility = converter.Convert(NotifyIcon.Green, typeof(Visibility),"Green",CultureInfo.CurrentCulture);
            //---------------Test Result -----------------------
            Assert.AreEqual(Visibility.Visible,visibility);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void Convert_GivenPlayerTestStateIsPositiveAndParameterIsRed_ShouldReturnVisible()
        {
            //---------------Set up test pack-------------------
            var converter = CreateConverter();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var visibility = converter.Convert(NotifyIcon.Green, typeof(Visibility),"Red",CultureInfo.CurrentCulture);
            //---------------Test Result -----------------------
            Assert.AreEqual(Visibility.Collapsed,visibility);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void Convert_GivenPlayerTestStateIsNegativeAndParameterIsGreen_ShouldReturnVisible()
        {
            //---------------Set up test pack-------------------
            var converter = CreateConverter();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var visibility = converter.Convert(NotifyIcon.Red, typeof(Visibility),"Green",CultureInfo.CurrentCulture);
            //---------------Test Result -----------------------
            Assert.AreEqual(Visibility.Collapsed,visibility);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void Convert_GivenPlayerTestStateIsNegativeAndParameterIsRed_ShouldReturnVisible()
        {
            //---------------Set up test pack-------------------
            var converter = CreateConverter();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var visibility = converter.Convert(NotifyIcon.Red, typeof(Visibility),"Red",CultureInfo.CurrentCulture);
            //---------------Test Result -----------------------
            Assert.AreEqual(Visibility.Visible,visibility);
        }

        private static PlayerTestStateToVisibilitConverter CreateConverter()
        {
            var converter = new PlayerTestStateToVisibilitConverter();
            return converter;
        }

    }
}