using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Katarai.Utils.Tests
{
    [TestFixture]
    public class TestFileSystemAdapter
    {
        [Test]
        public void Combine_ShouldReturnCombinedPath()
        {
            using (var sandboxPath = new SandboxPath())
            {
                //---------------Set up test pack-------------------
                CreateDefaultConfigFile(sandboxPath);
                var fileSystemAdapter = new FileSystemAdapter();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                var fullPath = fileSystemAdapter.Combine("location", "mypath");
                //---------------Test Result -----------------------
                Assert.AreEqual("location\\mypath", fullPath);
            }
        }

        [Test]
        public void FileExists_GivenFileExists_ShouldReturnTrue()
        {
            using (var sandboxPath = new SandboxPath())
            {
                //---------------Set up test pack-------------------
                var filePath = CreateDefaultConfigFile(sandboxPath);
                var fileSystemAdapter = new FileSystemAdapter();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                var fileExists = fileSystemAdapter.FileExists(filePath);
                //---------------Test Result -----------------------
                Assert.IsTrue(fileExists);
            }
        }

        [Test]
        public void FileExists_GivenFileDoesNotExist_ShouldReturnFalse()
        {
            using (var sandboxPath = new SandboxPath())
            {
                //---------------Set up test pack-------------------
                var filePath = CreateDefaultConfigFile(sandboxPath);
                var fileSystemAdapter = new FileSystemAdapter();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                var fileExists = fileSystemAdapter.FileExists(Guid.NewGuid().ToString());
                //---------------Test Result -----------------------
                Assert.IsFalse(fileExists);
            }
        }

        [Test]
        public void ReadAllText_ShouldReturnAllTextInFile()
        {
            using (var sandboxPath = new SandboxPath())
            {
                //---------------Set up test pack-------------------
                var filePath = CreateDefaultConfigFile(sandboxPath);
                var fileSystemAdapter = new FileSystemAdapter();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                var fileContents = fileSystemAdapter.ReadAllText(filePath);
                //---------------Test Result -----------------------
                Assert.AreEqual("Line 1\\r\\nLine2", fileContents);
            }
        }

        [Test]
        public void GetDirectoryName_ShouldReturnName()
        {
            using (var sandboxPath = new SandboxPath())
            {
                //---------------Set up test pack-------------------
                var fileSystemAdapter = new FileSystemAdapter();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                var folderName = fileSystemAdapter.GetDirectoryName(sandboxPath.FullPath);
                //---------------Test Result -----------------------
                Assert.IsTrue(folderName.EndsWith("Temp"));
            }
        }

        [Test]
        public void DirectoryExists_GivenFolderExists_ShouldReturnTrue()
        {
            using (var sandboxPath = new SandboxPath())
            {
                //---------------Set up test pack-------------------
                var fileSystemAdapter = new FileSystemAdapter();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                var folderExists = fileSystemAdapter.DirectoryExists(sandboxPath.FullPath);
                //---------------Test Result -----------------------
                Assert.IsTrue(folderExists);
            }
        }

        [Test]
        public void DirectoryExists_GivenFolderDoesNotExist_ShouldReturnTrue()
        {
                //---------------Set up test pack-------------------
                var fileSystemAdapter = new FileSystemAdapter();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                var folderExists = fileSystemAdapter.DirectoryExists(Guid.NewGuid().ToString());
                //---------------Test Result -----------------------
                Assert.IsFalse(folderExists);
        }

        [Test]
        public void CreateDirectory_ShouldCreateDirectory()
        {
            using (var sandboxPath = new SandboxPath())
            {
                //---------------Set up test pack-------------------
                var fileSystemAdapter = new FileSystemAdapter();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                var newDirectory = Path.Combine(sandboxPath.FullPath,Guid.NewGuid().ToString());
                fileSystemAdapter.CreateDirectory(newDirectory);
                //---------------Test Result -----------------------
                Assert.IsTrue(fileSystemAdapter.DirectoryExists(newDirectory));
            }
        }

        [Test]
        public void AppendAllLines_ShouldWriteToFile()
        {
            using (var sandboxPath = new SandboxPath())
            {
                //---------------Set up test pack-------------------
                const string expected = "This is text";
                var fileSystemAdapter = new FileSystemAdapter();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                var newFile = Path.Combine(sandboxPath.FullPath,Guid.NewGuid().ToString());
                fileSystemAdapter.AppendAllLines(newFile,new []{expected});
                //---------------Test Result -----------------------
                var actual = fileSystemAdapter.ReadAllText(newFile).Trim();
                Assert.AreEqual(expected, actual);
            }
        }

        private static string CreateDefaultConfigFile(SandboxPath sandboxPath)
        {
            var configFilePath = Path.Combine(sandboxPath.FullPath, "Config.txt");
            File.WriteAllText(configFilePath, @"Line 1\r\nLine2");
            return configFilePath;
        }
    }
}