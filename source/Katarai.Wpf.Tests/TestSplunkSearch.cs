using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestSplunkSearch
    {
        [Test]
        public void Constructor_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new SplunkSearch(Substitute.For<ISplunkSettings>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Constructor_IsInstanceOfISplunkSearch()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var splunkSearch = new SplunkSearch(Substitute.For<ISplunkSettings>());
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ISplunkSearch>(splunkSearch);
        }
    }
}