using Katarai.Wpf.Extensions;
using NUnit.Framework;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestImmediateDispatcher
    {
        [Test]
        public void Invoke_ShouldInvokeTheGivenAction()
        {
            //---------------Set up test pack-------------------
            var sut = Create();
            var called = false;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.Invoke(() => called = true);

            //---------------Test Result -----------------------
            Assert.IsTrue(called);
        }

        private ImmediateDispatcher Create()
        {
            return new ImmediateDispatcher();
        }
    }
}