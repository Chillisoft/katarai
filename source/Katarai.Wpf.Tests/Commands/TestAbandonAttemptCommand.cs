using Katarai.Wpf.Commands;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Commands
{
    [TestFixture]
    public class TestAbandonAttemptCommand
    {
        [Test]
        public void Construct_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new AbandonAttemptCommand());

            //---------------Test Result -----------------------
        }

        [Test]
        public void Execute_WhenNoEndPracticeHandlers_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            var sut = Create();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => sut.Execute(null));

            //---------------Test Result -----------------------
        }

        [Test]
        public void Execute_WhenHaveEndPracticeHandler_ShouldCallIt()
        {
            //---------------Set up test pack-------------------
            var sut = Create();
            var called = false;
            sut.EndPractice += (s, e) => called = true;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            sut.Execute(null);

            //---------------Test Result -----------------------
            Assert.IsTrue(called);
        }


        private AbandonAttemptCommand Create()
        {
            return new AbandonAttemptCommand();
        }
    }
}
