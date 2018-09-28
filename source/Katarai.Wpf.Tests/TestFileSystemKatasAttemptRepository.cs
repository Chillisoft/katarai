using System;
using System.IO;
using Katarai.Wpf.PackagedKata;
using NUnit.Framework;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestFileSystemKatasAttemptRepository
    {
        [TestCase("FizzBuzz", KataName.FizzBuzz)]
        [TestCase("StringCalculator", KataName.StringCalculator)]
        public void CreateNewKataAttempt_ReturnsAttempDirectoryPath(string kataName, KataName kataEnum)
        {
            //---------------Set up test pack-------------------
            var testStartDateTime = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            var expectedAttemptPath = Path.Combine(GetSpecialFolder(),
                string.Format(@"{0}-{1}", kataName, testStartDateTime));
            var repository = CreateRepository();
            if (Directory.Exists(expectedAttemptPath))
            {
                Directory.Delete(expectedAttemptPath, true);
            }
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string attemptPath = expectedAttemptPath;
            try
            {
                var kataAttempt = repository.CreateNewKataAttempt(kataEnum);
                attemptPath = kataAttempt.Location;
            }
            finally
            {
                if (Directory.Exists(attemptPath)) Directory.Delete(attemptPath, true);
            }
            //---------------Test Result -----------------------
            var dateTime = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            var latestExpectedAttemptPath = Path.Combine(GetSpecialFolder(),
                string.Format(@"{0}-{1}", kataName, dateTime));
            Assert.GreaterOrEqual(attemptPath, expectedAttemptPath);
            Assert.LessOrEqual(attemptPath, latestExpectedAttemptPath);
        }

        private FileSystemKatasAttemptRepository CreateRepository()
        {
            return new FileSystemKatasAttemptRepository();
        }

        private string GetSpecialFolder()
        {
            var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("Roaming", "Local");
            var fullPath = Path.Combine(rootPath, "Katarai");
            return fullPath;
        }
    }
}