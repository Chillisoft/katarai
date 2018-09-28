using Katarai.Wpf.Events;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Events
{
    [TestFixture]
    public class TestKataAttemptCreatedEvent
    {
        [Test]
        public void Construct_ShouldCopyKataAttemptParameter_To_Property()
        {
            //---------------Set up test pack-------------------
            var input = Substitute.For<IKataAttempt>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var sut = new KataAttemptCreatedEvent(input);

            //---------------Test Result -----------------------
            Assert.AreEqual(input, sut.KataAttempt);
        }

    }
}
