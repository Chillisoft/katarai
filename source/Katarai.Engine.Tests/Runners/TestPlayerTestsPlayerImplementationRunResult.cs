using Engine.Runners;
using NUnit.Framework;

namespace Katarai.Engine.Tests.Runners
{
    [TestFixture]
    public class TestPlayerTestsPlayerImplementationRunResult
    {
        [Test]
        public void ToString_GivenPassedIsTrue_ShouldReturnGreenForPlayerTestState()
        {
            //---------------Set up test pack-------------------
            var runResult = new PlayerTestsPlayerImplementationRunResult {Passed = true};
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var toString = runResult.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual("Player Test State: Green", toString);
        }

        [Test]
        public void ToString_GivenPassedIsFalse_ShouldReturnRedForPlayerTestState()
        {
            //---------------Set up test pack-------------------
            var runResult = new PlayerTestsPlayerImplementationRunResult {Passed = false};
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var toString = runResult.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual("Player Test State: Red", toString);
        }

        [Test]
        public void ToString_GivenPassedIsNotSet_ShouldReturnRedForPlayerTestState()
        {
            //---------------Set up test pack-------------------
            var runResult = new PlayerTestsPlayerImplementationRunResult();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var toString = runResult.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual("Player Test State: Red", toString);
        }

    }
}