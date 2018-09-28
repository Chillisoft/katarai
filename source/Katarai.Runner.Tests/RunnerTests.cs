using System;
using System.CodeDom;
using System.IO;
using NUnit.Framework;

namespace Katarai.Runner.Tests
{
    [TestFixture]
    public class RunnerTests
    {
        private const string masterFizzbuzzDll = "Katarai.KataData.FizzBuzz.dll";
        private const string fizzbuzzinterfaceDll = "Katarai.FizzBuzz.Interfaces.dll";
        private const string playerfizzbuzzDll = "PlayerFizzBuzz.dll";

        [Test]
        public void Ctor_GivenFizzbuzzKataAndPlayerDll_ShouldCreateSUT()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            
            //---------------Test Result -----------------------
            Assert.DoesNotThrow(() => new Runner("", "", null));
        }

        [Test]
        public void Run_GivenFizzBuzzDataAndPlayerTest_ShouldReturnCorrectLevel()
        {
            //---------------Set up test pack-------------------
            const int expectedPlayer = 1;
            const int expectedImpl = 3;
            var tmpPath = GetRandomTestPath();

            UnpackAssemblies(tmpPath);
            var testRunner = new Runner(Path.Combine(tmpPath, masterFizzbuzzDll),
                Path.Combine(tmpPath, playerfizzbuzzDll), null);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = testRunner.Run();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedPlayer, result.PlayerTestLevel);
            Assert.AreEqual(expectedImpl, result.PlayerImplementationLevel);
                

        }

        private string GetRandomTestPath()
        {
            RemovePathsInCleanupList();
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);
            AddPathToCleanupList(tempPath);
            return tempPath;
        }

        private void AddPathToCleanupList(string path)
        {
            var tempPath = Path.GetTempPath();
            const string filestocleanupTxt = "filesToCleanup.txt";
            var fileFullName = Path.Combine(tempPath, filestocleanupTxt);
            if (!File.Exists(fileFullName))
            {
                File.WriteAllLines(fileFullName, new []{ path });
            }
            else
            {
                File.AppendAllLines(fileFullName, new[] { path });
                
            }

        }

        private void RemovePathsInCleanupList()
        {
            var tempPath = Path.GetTempPath();
            const string filestocleanupTxt = "filesToCleanup.txt";
            var fileFullName = Path.Combine(tempPath, filestocleanupTxt);
            if (File.Exists(fileFullName))
            {
                var lines = File.ReadAllLines(fileFullName);
                foreach (var line in lines)
                {
                    if (Directory.Exists(line))
                    {
                        Directory.Delete(line, true);
                    }
                }
                File.Delete(fileFullName);
            }
        }

        private void UnpackAssemblies(string path)
        {
            File.WriteAllBytes(Path.Combine(path, masterFizzbuzzDll),Resources.Katarai_KataData_FizzBuzz);
            File.WriteAllBytes(Path.Combine(path, fizzbuzzinterfaceDll), Resources.Katarai_FizzBuzz_Interfaces);
            File.WriteAllBytes(Path.Combine(path, playerfizzbuzzDll), Resources.SamplePlayerFizzBuzz);

        }
    }
}
