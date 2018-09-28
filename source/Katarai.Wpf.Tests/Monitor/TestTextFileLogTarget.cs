using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Katarai.Utils;
using Katarai.Utils.Tests;
using Katarai.Wpf.Monitor;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Monitor
{
    [TestFixture]
    public class TestTextFileLogTarget
    {
        [Test]
        public void Constructor_GivenNullFileName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception =
                Assert.Throws<ArgumentNullException>(() => new TextFileLogTarget(null, Substitute.For<IFileSystem>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("fileName", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenEmptyFileName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception =
                Assert.Throws<ArgumentNullException>(
                    () => new TextFileLogTarget(string.Empty, Substitute.For<IFileSystem>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("fileName", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenNullFileSystem_ShouldThrowException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception =
                Assert.Throws<ArgumentNullException>(() => new TextFileLogTarget(Guid.NewGuid().ToString(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("fileSystem", exception.ParamName);
        }

        [Test]
        public void LogInfo_ShouldLogMessage()
        {
            //---------------Set up test pack-------------------
            const string thisIsAMessage = "This is a message";
            var folder = Guid.NewGuid().ToString();
            var logFile = Guid.NewGuid().ToString();
            var fileName = Path.Combine(folder, logFile);

            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.GetDirectoryName(fileName).Returns(folder);
            fileSystem.DirectoryExists(folder).Returns(true);

            string[] messages = null;
            fileSystem.AppendAllLines(fileName, Arg.Do<string[]>(x => messages = x));

            var logTarget = new TextFileLogTarget(fileName, fileSystem);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            logTarget.LogInfo(thisIsAMessage);
            //---------------Test Result -----------------------
            Assert.IsTrue(messages.First(m => m.Contains(thisIsAMessage)) != null);
        }



        [Test]
        public void LogError_ShouldLogErrorMessage()
        {
            //---------------Set up test pack-------------------
            var fileName = Path.GetTempFileName();
            var fileSystem = Substitute.For<IFileSystem>();
            var logTarget = new TextFileLogTarget(fileName, fileSystem);
            const string folderName = "FolderName";
            fileSystem.GetDirectoryName(fileName).Returns(folderName);
            fileSystem.DirectoryExists(folderName).Returns(true);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            const string thisIsAMessage = "This is a error message";
            logTarget.LogError(thisIsAMessage);
            //---------------Test Result -----------------------
            fileSystem.Received().AppendAllLines(fileName, Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void LogInfo_WhenFolderMissing_ShouldCreateFolderAndLogMessage()
        {
            //---------------Set up test pack-------------------
            const string thisIsAMessage = "This is a message";
            var missingSubFolder = Guid.NewGuid().ToString();
            var logFile = Guid.NewGuid().ToString();
            var fileName = Path.Combine(missingSubFolder, logFile);

            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.GetDirectoryName(fileName).Returns(missingSubFolder);
            fileSystem.DirectoryExists(missingSubFolder).Returns(false);

            string[] actualMessages = null;
            fileSystem.AppendAllLines(fileName, Arg.Do<string[]>(x => actualMessages = x));

            var logTarget = new TextFileLogTarget(fileName, fileSystem);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            logTarget.LogInfo(thisIsAMessage);
            //---------------Test Result -----------------------
            fileSystem.Received().CreateDirectory(missingSubFolder);
            Assert.IsTrue(actualMessages.First(m => m.Contains(thisIsAMessage)) != null);
        }

        [Test]
        public void LogError_WhenFolderMissing_ShouldCreateFolderAndLogMessage()
        {
            //---------------Set up test pack-------------------
            const string thisIsAMessage = "This is a error message";
            var missingSubFolder = Guid.NewGuid().ToString();
            var logFile = Guid.NewGuid().ToString();
            var fileName = Path.Combine(missingSubFolder, logFile);

            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.GetDirectoryName(fileName).Returns(missingSubFolder);
            fileSystem.DirectoryExists(missingSubFolder).Returns(false);

            string[] actualMessages = null;
            fileSystem.AppendAllLines(fileName, Arg.Do<string[]>(x => actualMessages = x));

            var logTarget = new TextFileLogTarget(fileName, fileSystem);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            logTarget.LogError(thisIsAMessage);
            //---------------Test Result -----------------------
            fileSystem.Received().CreateDirectory(missingSubFolder);
            Assert.IsTrue(actualMessages.First(m => m.Contains(thisIsAMessage)) != null);
        }
    }
}