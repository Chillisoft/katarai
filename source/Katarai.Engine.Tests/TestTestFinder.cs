using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Katarai.StringCalculator;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace Katarai.Engine.Tests
{
    [TestFixture]
    public class TestTestFinder
    {
        private readonly TempFileContainer _tempFileContainer = new TempFileContainer();
        
        [TearDown]
        public void TearDown()
        {
            _tempFileContainer.ClearTempFiles();
        }

        [Test]
        public void Construct_GivenNullPath_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentException>(() => new TestFinder(null, null));

            //---------------Test Result -----------------------
            StringAssert.Contains("Testpack assembly not specified", ex.Message);
        }

        [Test]
        public void Construct_GivenEmptyPath_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentException>(() => new TestFinder("", null));

            //---------------Test Result -----------------------
            StringAssert.Contains("Testpack assembly not specified", ex.Message);
        }

        [Test]
        public void Construct_GivenNullTestPackInterface_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentException>(() => new TestFinder(RandomValueGen.GetRandomString(2, 10), null));

            //---------------Test Result -----------------------
            StringAssert.Contains("Testpack interface not specified", ex.Message);
        }

        private TestFinder Create(string asmPath, Type forType)
        {
            return new TestFinder(asmPath, forType);
        }

        private string GetTempPlayerAssembly(bool validatePath = true)
        {
            var asmFolder = GetFolderContainingCurrentTestAssembly();
            return _tempFileContainer.GetTempAssembly(asmFolder, "Katarai.KataData.StringCalculator", validatePath);
        }

        private string GetTempGoldenAssembly()
        {
            var asmFolder = GetFolderContainingCurrentTestAssembly();
            return _tempFileContainer.GetTempAssembly(asmFolder, "Katarai.StringCalculator", true);
        }

        private string GetFolderContainingCurrentTestAssembly()
        {
            var asmPath = new Uri(GetType().Assembly.CodeBase).LocalPath;
            var asmFolder = Path.GetDirectoryName(asmPath);
            return asmFolder;
        }

        [Test]
        public void Construct_GivenPathAndTestPackInterface_WhenOutputAssemblyDoesNotExist_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var testAsm = GetTempPlayerAssembly(false) + ".1";

            //---------------Assert Precondition----------------
            Assert.IsFalse(File.Exists(testAsm));

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<TestpackAssemblyNotFoundException>(() => Create(testAsm, typeof(ITestPack<IStringCalculator>)));

            //---------------Test Result -----------------------
            Assert.AreEqual(testAsm, ex.TestpackPath);
        }

        public interface IKataInterfaceThatShouldNotBeFound: IImplementationInstance
        {
        }

        [Test]
        public void Construct_WhenTestAssemblyExistsButDoesNotContainAClassImplementingTheTestPackInterface_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var testAsm = GetTempPlayerAssembly();
            
            //---------------Assert Precondition----------------
            Assert.IsTrue(File.Exists(testAsm));
            //---------------Execute Test ----------------------
            var theType = typeof(ITestPack<TestTestFinder.IKataInterfaceThatShouldNotBeFound>);
            var ex = Assert.Throws<TestpackException>(() => Create(testAsm, theType));
            //---------------Test Result -----------------------
            Assert.AreEqual(testAsm, ex.TestpackPath);
            Assert.AreEqual("Required interface implementation not found: " + theType.ToString(), ex.Detail);
        }

        [Test]
        [Ignore("WIP")]
        public void Construct_WhenPointedAtGoldenAssembly_ShouldReturnMethodsWithInfo()
        {
            //---------------Set up test pack-------------------
            var testAsm = GetTempGoldenAssembly();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var testFinder = Create(testAsm, typeof(ITestPack<IStringCalculator>));

            //---------------Test Result -----------------------
            Assert.IsNotNull(testFinder.TestMethods);
            Assert.IsTrue(testFinder.TestMethods.Any());
        }

    }
}
