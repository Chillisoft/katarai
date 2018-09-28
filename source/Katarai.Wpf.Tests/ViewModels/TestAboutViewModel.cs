using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Katarai.Wpf.ViewModels;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;

namespace Katarai.Wpf.Tests.ViewModels
{
    [TestFixture]
    public class TestAboutViewModel
    {
        [Test]
        public void Type_ShouldImplement_IAboutViewModel()
        {
            //---------------Set up test pack-------------------
            var sut = typeof (AboutViewModel);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IAboutViewModel>();

            //---------------Test Result -----------------------
        }

        [Test]
        public void InterfaceType_ShouldImplement_IViewModel()
        {
            //---------------Set up test pack-------------------
            var sut = typeof (IAboutViewModel);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IViewModel>();

            //---------------Test Result -----------------------
        }

        [Test]
        public void Type_ShouldInheritFrom_SingleInstanceViewModel()
        {
            //---------------Set up test pack-------------------
            var sut = typeof (AboutViewModel);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldInheritFrom<SingleInstanceViewModel>();

            //---------------Test Result -----------------------
        }

        [Test]
        public void ApplicationVersion_ShouldReflectExecutingAssemblyVersion()
        {
            //---------------Set up test pack-------------------
            var sut = Create();
            var expected = sut.GetType().Assembly.GetName().Version.ToString();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.ApplicationVersion;

            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        private IAboutViewModel Create()
        {
            return new AboutViewModel();
        }
    }
}
