using System;
using System.IO;
using System.Linq;
using Engine;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.Engine.Tests
{
    [TestFixture]
    public class TestAssemblyInspector
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
            var ex = Assert.Throws<ArgumentNullException>(() => new AssemblyInspector(null));

            //---------------Test Result -----------------------
            Assert.That(ex.ParamName, Is.EqualTo("assemblyPath"));
        }

        [Test]
        public void Construct_GivenEmptyPath_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => new AssemblyInspector(""));

            //---------------Test Result -----------------------
            Assert.That(ex.ParamName, Is.EqualTo("assemblyPath"));
        }

        private AssemblyInspector CreateSUT(string asmPath)
        {
            return new AssemblyInspector(asmPath);
        }

        private string GetFolderContainingCurrentTestAssembly()
        {
            var asmPath = new Uri(GetType().Assembly.CodeBase).LocalPath;
            var asmFolder = Path.GetDirectoryName(asmPath);
            return asmFolder;
        }
        private string GetTempPlayerAssembly()
        {
            return _tempFileContainer.GetTempAssembly(GetFolderContainingCurrentTestAssembly(), "Katarai.KataData.StringCalculator", true);
        }

        private string GetTempGoldenAssembly()
        {
            return _tempFileContainer.GetTempAssembly(GetFolderContainingCurrentTestAssembly(), "Katarai.StringCalculator.Golden", true);
        }

        [Test]
        public void Construct_GivenPath_WhenOutputAssemblyDoesNotExist_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var testAsm = GetTempPlayerAssembly() + ".1";

            //---------------Assert Precondition----------------
            Assert.IsFalse(File.Exists(testAsm));
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<FileNotFoundException>(() => CreateSUT(testAsm));

            //---------------Test Result -----------------------
            Assert.That(ex.FileName, Is.EqualTo(testAsm));
        }

        [Test]
        public void Construct_GivenPath_WhenAssemblyExistsAndNotCurrentlyLoaded_ShouldLoadAssembly()
        {
            //---------------Set up test pack-------------------
            var testAsm = GetTempPlayerAssembly();

            //---------------Assert Precondition----------------

            Assert.IsTrue(File.Exists(testAsm));
            //---------------Execute Test ----------------------
            var sut = CreateSUT(testAsm);

            //---------------Test Result -----------------------
            var types = sut.GetTypesImplementing(typeof(object));
            var codeBase = types.First().Assembly.CodeBase;
            Assert.That(AsUriString(codeBase), Is.EqualTo(AsUriString(testAsm)));
            Assert.IsTrue(AssemblyIsLoaded(testAsm));
        }

        private static bool AssemblyIsLoaded(string assemblyPath)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblyIsLoaded = loadedAssemblies
                .Where(assembly => !assembly.IsDynamic)
                .Any(assembly => AsUriString(assembly.CodeBase) == AsUriString(assemblyPath));
            return assemblyIsLoaded;
        }

        [Test]
        public void Construct_GivenPath_WhenAssemblyExistsAndAlreadyReferenced_ShouldNotReloadAssembly()
        {
            //---------------Set up test pack-------------------
            var referencedAssembly = typeof (IStringCalculator).Assembly;
            var testAsm = GetTempGoldenAssembly();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var sut = CreateSUT(testAsm);
            //---------------Test Result -----------------------
            OutputLoadedAssemblies();
            var types = sut.GetTypesImplementing(typeof(IStringCalculator));
            var returnedAssembly = types.First().GetInterface("IStringCalculator").Assembly;
            Assert.That(returnedAssembly, Is.SameAs(referencedAssembly));
        }

        private static void OutputLoadedAssemblies()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            loadedAssemblies.ToList().ForEach(Console.WriteLine);
        }

        private static string AsUriString(string url)
        {
            return new Uri(url).ToString();
        }

        [Test]
        public void GetTypesImplementing_GivenNullInterface_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var tempGoldenAssembly = GetTempGoldenAssembly();
            var sut = CreateSUT(tempGoldenAssembly);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                sut.GetTypesImplementing(null);
            });
            //---------------Test Result -----------------------
            Assert.That(exception.ParamName, Is.EqualTo("interfaceType"));
        }

        public interface IKataInterfaceThatShouldNotBeFound : IImplementationInstance
        {
        }

        [Test]
        public void GetTypesImplementing_GivenUnknownInterface_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var tempGoldenAssembly = GetTempGoldenAssembly();
            var sut = CreateSUT(tempGoldenAssembly);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var typesImplementing = sut.GetTypesImplementing(typeof (IKataInterfaceThatShouldNotBeFound));
            //---------------Test Result -----------------------
            Assert.IsNotNull(typesImplementing);
            Assert.IsFalse(typesImplementing.Any());
        }

        [Test]
        public void GetTypesImplementing_GivenKnownInterface_ShouldReturnListWithTypes()
        {
            //---------------Set up test pack-------------------
            var tempGoldenAssembly = GetTempGoldenAssembly();
            var sut = CreateSUT(tempGoldenAssembly);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var typesImplementing = sut.GetTypesImplementing(typeof(IStringCalculator));
            //---------------Test Result -----------------------
            Assert.IsNotNull(typesImplementing);
            Assert.IsTrue(typesImplementing.Any());
        }

        [Test]
        public void GetTypesImplementing_GivenKnownInterface_ShouldReturnListWithCorrectType()
        {
            //---------------Set up test pack-------------------
            var tempGoldenAssembly = GetTempGoldenAssembly();
            var sut = CreateSUT(tempGoldenAssembly);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var typesImplementing = sut.GetTypesImplementing(typeof(IStringCalculator));
            //---------------Test Result -----------------------
            Assert.IsNotNull(typesImplementing);
            var type = typesImplementing.First().GetInterfaces().First();
            var expectedType = typeof (IStringCalculator);
            Assert.That(type, Is.EqualTo(expectedType));
        }

        [Test]
        [Ignore("WIP: come back to this and other engine testing when the app is unfuxed again")]
        public void GetTypesImplementing_GivenKnownInterface_ShouldReturn()
        {
            //---------------Set up test pack-------------------
            var tempPlayerAssembly = GetTempPlayerAssembly();
            var sut = CreateSUT(tempPlayerAssembly);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var typesImplementing = sut.GetTypesImplementing(typeof(IStringCalculator));
            //---------------Test Result -----------------------
            Assert.IsNotNull(typesImplementing);

            var type = typesImplementing.First();
            Assert.That(type.GetInterfaces(), Contains.Item(typeof(IStringCalculator)));
            Assert.That(type.Name, Is.EqualTo("StringCalculator_AtLevel_002"));
            /*var expectedType = typeof (IStringCalculator);
            Assert.That(type.Assembly.FullName, Is.EqualTo(expectedType.Assembly.FullName));
            Assert.That(type.Assembly.CodeBase, Is.EqualTo(expectedType.Assembly.CodeBase));
            Assert.That(type.Assembly, Is.EqualTo(expectedType.Assembly));
            Assert.That(type, Is.EqualTo(expectedType));*/
        }
    }
}
