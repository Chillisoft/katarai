using Katarai.Runner;
using Katarai.Wpf.Settings;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Settings
{
    [TestFixture]
    public class TestKataraiSettings
    {
        [Test]
        public void Ctor_WhenCreating_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            
            //---------------Test Result -----------------------
            Assert.DoesNotThrow(() => new KataraiSettings());
        }

        [Test]
        public void SetKataHashProperty_ShouldGiveValueBack()
        {
            //---------------Set up test pack-------------------
            const string expected = "a";
            var settings = Create();
            settings.KataHash = "a";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = settings.KataHash;

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SetKataPathProperty_ShouldGiveValueBack()
        {
            //---------------Set up test pack-------------------
            const string expected = "b";
            var settings = Create();
            settings.KataPath = "b";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = settings.KataPath;

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SetPlayerHashProperty_ShouldGiveValueBack()
        {
            //---------------Set up test pack-------------------
            const string expected = "c";
            var settings = Create();
            settings.PlayerHash = "c";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = settings.PlayerHash;

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SetPlayerPathProperty_ShouldGiveValueBack()
        {
            //---------------Set up test pack-------------------
            const string expected = "d";
            var settings = Create();
            settings.PlayerPath = "d";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = settings.PlayerPath;

            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        private static KataraiSettings Create()
        {
            return new KataraiSettings();
        }
    }
}
