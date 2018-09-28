using Katarai.Wpf.FileWatch;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.FileWatch
{
    [TestFixture]
    public class TestFileMonitorInformation
    {
        [Test]
        public void AreThereChanges_GivenOldKataHashIsNull_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var fileMonitorInformation = new FileMonitorInformation
            {
                OldKataHash = null
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var areThereChanges = fileMonitorInformation.AreThereChanges();
            //---------------Test Result -----------------------
            Assert.IsTrue(areThereChanges);
        }

        [Test]
        public void AreThereChanges_GivenOldKataHashIsEmpty_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var fileMonitorInformation = new FileMonitorInformation
            {
                OldKataHash = string.Empty
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var areThereChanges = fileMonitorInformation.AreThereChanges();
            //---------------Test Result -----------------------
            Assert.IsTrue(areThereChanges);
        }

        [Test]
        public void AreThereChanges_GivenOldPlayerHashIsNull_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var fileMonitorInformation = new FileMonitorInformation
            {
                OldPlayerHash = null
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var areThereChanges = fileMonitorInformation.AreThereChanges();
            //---------------Test Result -----------------------
            Assert.IsTrue(areThereChanges);
        }

        [Test]
        public void AreThereChanges_GivenOldPlayerHashIsEmpty_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var fileMonitorInformation = new FileMonitorInformation
            {
                OldPlayerHash = string.Empty
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var areThereChanges = fileMonitorInformation.AreThereChanges();
            //---------------Test Result -----------------------
            Assert.IsTrue(areThereChanges);
        }

        [Test]
        public void AreThereChanges_GivenOldKataHashIsNotEqualToKataHash_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var fileMonitorInformation = new FileMonitorInformation
            {
                OldKataHash = "old hash",
                KataHash = "new hash"
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var areThereChanges = fileMonitorInformation.AreThereChanges();
            //---------------Test Result -----------------------
            Assert.IsTrue(areThereChanges);
        }

        [Test]
        public void AreThereChanges_GivenOldPlayerHashIsNotEqualToPlayerHash_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var fileMonitorInformation = new FileMonitorInformation
            {
                OldPlayerHash = "old hash",
                PlayerHash = "new hash"
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var areThereChanges = fileMonitorInformation.AreThereChanges();
            //---------------Test Result -----------------------
            Assert.IsTrue(areThereChanges);
        }

        [Test]
        public void AreThereChanges_GivenOldKataHashIsNotEqualToKataHash_AndOldPlayerHashIsEqualToPlayerHash_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var fileMonitorInformation = new FileMonitorInformation
            {
                OldKataHash = "old hash",
                KataHash = "new hash",
                OldPlayerHash = "my hash",
                PlayerHash = "my hash"
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var areThereChanges = fileMonitorInformation.AreThereChanges();
            //---------------Test Result -----------------------
            Assert.IsTrue(areThereChanges);
        }

        [Test]
        public void AreThereChanges_GivenOldKataHashIsEqualToKataHash_AndOldPlayerHashIsNotEqualToPlayerHash_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var fileMonitorInformation = new FileMonitorInformation
            {
                OldKataHash = "kata hash",
                KataHash = "kata hash",
                OldPlayerHash = "old player",
                PlayerHash = "new player"
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var areThereChanges = fileMonitorInformation.AreThereChanges();
            //---------------Test Result -----------------------
            Assert.IsTrue(areThereChanges);
        }

        [Test]
        public void AreThereChanges_GivenOldKataHashIsEqualToKataHash_AndOldPlayerHashIsEqualToPlayerHash_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var fileMonitorInformation = new FileMonitorInformation
            {
                OldKataHash = "kata hash",
                KataHash = "kata hash",
                OldPlayerHash = "old player",
                PlayerHash = "old player"
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var areThereChanges = fileMonitorInformation.AreThereChanges();
            //---------------Test Result -----------------------
            Assert.IsFalse(areThereChanges);
        }


    }
}