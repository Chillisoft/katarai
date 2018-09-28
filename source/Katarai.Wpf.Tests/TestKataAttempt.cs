using System;
using System.IO;
using Katarai.Utils;
using Katarai.Utils.Tests;
using Katarai.Wpf.Tests.Monitor;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestKataAttempt
    {
        private static KataAttempt CreateKataAttempt()
        {
            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.FileExists(Arg.Any<string>()).Returns(true);
            fileSystem.ReadAllText(Arg.Any<string>())
                .Returns(@"/*directory structure must remain as MasterSolution, PlayerSolution*/
                {
                ""masterSolutionAssembly"":""Katarai.KataData.StringCalculator.dll"",
                ""playerSolutionName"":""PlayerStringKata.sln"",
                ""playerSolutionAssembly"":""PlayerStringKata.dll"",
                ""kataName"":""String Calculator"",
                ""instructions"":{
		                ""header"":""Please note the following:"",
		                ""content"":[
			""Some things to note about doing a kata with Katarai:"",
			""- Katarai currently only supports NUnit [Test] attributes - please use only these for your test methods."",
			""- The provided Constructor test shows how to create your SUT so that Katarai can work with it."",
			""- Please do an initial build of the solution before you write your first test – this is the official kata start.""
		                ]
	                }
                }");
            fileSystem.Combine(Arg.Any<string>(), Arg.Any<string>()).Returns("sample/location");
            return new KataAttempt(fileSystem)
            {
                Location = string.Empty
            };
        }

        [Test]
        public void Name_ShouldSetAndGet()
        {
            //---------------Set up test pack-------------------
            var kataAttempt = CreateKataAttempt();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            kataAttempt.Name = "Some Name";
            //---------------Test Result -----------------------
            Assert.That(kataAttempt.Name, Is.EqualTo("Some Name"));
        }

        [Test]
        public void Location_ShouldSetAndGet()
        {
            //---------------Set up test pack-------------------
            var kataAttempt = CreateKataAttempt();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            kataAttempt.Location = "C:\\Some Location";
            //---------------Test Result -----------------------
            Assert.That(kataAttempt.Location, Is.EqualTo("C:\\Some Location"));
        }

        [Test]
        public void Config_MasterDll_ShouldReadFromFile_Config_txt()
        {
            //---------------Set up test pack-------------------
            var kataAttempt = CreateKataAttempt();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var config = kataAttempt.Config;
            //---------------Test Result -----------------------
            Assert.That(config.MasterDll, Is.EqualTo("Katarai.KataData.StringCalculator.dll"));
        }

        [Test]
        public void Config_MasterDllPath_ShouldReadFromFile_Config_txt()
        {
            //---------------Set up test pack-------------------
            var kataAttempt = CreateKataAttempt();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var config = kataAttempt.Config;
            //---------------Test Result -----------------------
            var expectedPath = Path.Combine("", "MasterSolution", "Katarai.KataData.StringCalculator.dll");
            Assert.That(config.MasterDllPath, Is.EqualTo(expectedPath));
        }

        [Test]
        public void Config_SolutionName_ShouldReadFromFile_Config_txt()
        {
            //---------------Set up test pack-------------------
            var kataAttempt = CreateKataAttempt();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var config = kataAttempt.Config;
            //---------------Test Result -----------------------
            Assert.That(config.SolutionName, Is.EqualTo("PlayerStringKata.sln"));
        }

        [Test]
        public void Config_SolutionFilePath_ShouldReadFromFile_Config_txt()
        {
            //---------------Set up test pack-------------------
            var kataAttempt = CreateKataAttempt();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var config = kataAttempt.Config;
            //---------------Test Result -----------------------
            var expectedPath = Path.Combine("", "PlayerSolution", "PlayerStringKata.sln");
            Assert.That(config.SolutionFilePath, Is.EqualTo(expectedPath));
        }

        [Test]
        public void Config_PlayerDll_ShouldReadFromFile_Config_txt()
        {
            //---------------Set up test pack-------------------
            var kataAttempt = CreateKataAttempt();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var config = kataAttempt.Config;
            //---------------Test Result -----------------------
            Assert.That(config.PlayerDll, Is.EqualTo("PlayerStringKata.dll"));
        }

        [Test]
        public void Config_Instructions_ShouldReadFromFile_Config()
        {
            //---------------Set up test pack-------------------
            var kataAttempt = CreateKataAttempt();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var config = kataAttempt.Config;
            //---------------Test Result -----------------------
            Assert.That(config.Instructions.Header, Is.EqualTo("Please note the following:\r\n"));
            CollectionAssert.AreEquivalent(config.Instructions.Content, new[]
            {
                "Some things to note about doing a kata with Katarai:", Environment.NewLine,
                "- Katarai currently only supports NUnit [Test] attributes - please use only these for your test methods.",Environment.NewLine,
                "- The provided Constructor test shows how to create your SUT so that Katarai can work with it.",Environment.NewLine,
                "- Please do an initial build of the solution before you write your first test – this is the official kata start.",Environment.NewLine,
            });
        }

        [Test]
        public void Config_PlayerDllPath_ShouldReadFromFile_Config_txt()
        {
            //---------------Set up test pack-------------------
            var kataAttempt = CreateKataAttempt();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var config = kataAttempt.Config;
            //---------------Test Result -----------------------
            var expectedPath = Path.Combine("", "PlayerSolution", "bin", "Debug", "PlayerStringKata.dll");
            Assert.That(config.PlayerDllPath, Is.EqualTo(expectedPath));
        }


        [Test]
        public void Config_KataName_ShouldReadFromFile_Config_txt()
        {
            //---------------Set up test pack-------------------
            var kataAttempt = CreateKataAttempt();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var config = kataAttempt.Config;
            //---------------Test Result -----------------------
            Assert.That(config.KataName, Is.EqualTo("String Calculator"));
        }
    }
}