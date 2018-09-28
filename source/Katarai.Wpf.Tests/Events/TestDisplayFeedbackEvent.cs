using Katarai.Wpf.Events;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace Katarai.Wpf.Tests.Events
{
    [TestFixture]
    public class TestDisplayFeedbackEvent
    {
        [Test]
        public void Construct_ShouldCopyMessageParameter_ToProperty()
        {
            //---------------Set up test pack-------------------
            var expected = RandomValueGen.GetRandomString();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var sut = new DisplayFeedbackEvent(expected);

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, sut.Message);
        }

    }
}
