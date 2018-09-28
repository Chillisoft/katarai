using System.Windows;
using Katarai.Wpf.Extensions;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestAutoDispatcher
    {
        [Test]
        public void Type_ShouldImplement_IDispatcher()
        {
            //---------------Set up test pack-------------------
            var sut = typeof(AutoDispatcher);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.ShouldImplement<IDispatcher>();

            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_InRegularTest_ShouldMakeUseOfImmediateDispatcher()
        {
            //---------------Set up test pack-------------------
            var sut = Create();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = sut.Actual;

            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ImmediateDispatcher>(result);
        }

        private AutoDispatcher Create()
        {
            return new AutoDispatcher();
        }
    }
}