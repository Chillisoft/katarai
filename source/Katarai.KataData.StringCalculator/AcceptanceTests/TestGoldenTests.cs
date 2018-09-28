using System;
using System.Linq;
using Engine;
using Engine.Annotations;
using Engine.Runners;
using Katarai.KataData.StringCalculator.Implementations;
using Katarai.KataData.StringCalculator.Tests;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Katarai.KataData.StringCalculator.AcceptanceTests
{
    [TestFixture]
    public class TestGoldenTests
    {
        private static PlayerTestsRunResult GetTestsRunResultFor(Func<ITestMethod, bool> filter)
        {
            var kataConfiguration = new Configuration();
            var goldenTestType = kataConfiguration.GetKataTestsType();
            var kataImplementationTypes = kataConfiguration.GetKataImplementationTypes().ToArray();
            var testMethodsRetriever = new CustomTestMethodsRetriever(filter);
            var playerTestsRunner = new PlayerTestsRunner<IStringCalculator>(
                goldenTestType, kataImplementationTypes, testMethodsRetriever);
            var result = playerTestsRunner.Run();
            return result;
        }

        [Test]
        public void Run_GivenTestAtLevel1_ShouldReturnLevel1()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = GetTestsRunResultFor(method => method.Level <= 1);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(1));
        }

        [Test]
        public void Run_GivenTestAtLevel2_ShouldReturnLevel2()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = GetTestsRunResultFor(method => method.Level <= 2);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(2));
        }

        [Test]
        public void Run_GivenTestAtLevel3_ShouldReturnLevel3()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = GetTestsRunResultFor(method => method.Level <= 3);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(3));
        }

        [Test]
        public void Run_GivenTestAtLevel4_ShouldReturnLevel4()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = GetTestsRunResultFor(method => method.Level <= 4);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(4));
            Assert.That(result.EdgeCasesCovered, Is.EqualTo(2));
            Assert.That(result.TotalEdgeCases, Is.EqualTo(2));
            Assert.That(result.NumberOfTestsImplemented, Is.EqualTo(6));
        }

        [Test]
        public void Run_GivenTestAtLevel4_WhenEdgeCaseMissing_ShouldReturnLevel3AndEdgeCases1of2()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var filter = new Func<ITestMethod, bool>(method =>
            {
                var isLevelEdgeCaseTest = method.Level == 4 &&
                    method.KataAnnotations.Any(annotation => annotation is EdgeCaseHintAttribute);
                return method.Level <= 4 && !isLevelEdgeCaseTest;
            });
            var result = GetTestsRunResultFor(filter);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(4), "Level");
            Assert.That(result.EdgeCasesCovered, Is.EqualTo(1), "EdgeCasesCovered");
            Assert.That(result.TotalEdgeCases, Is.EqualTo(2), "TotalEdgeCases");
            Assert.That(result.NumberOfTestsImplemented, Is.EqualTo(4), "NumberOfTestsImplemented");
        }

        [Test]
        public void Run_GivenTestAtLevel5_ShouldReturnLevel5()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = GetTestsRunResultFor(method => method.Level <= 5);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(5));
        }

        [Test]
        public void Run_GivenTestAtLevel6_ShouldReturnLevel6()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = GetTestsRunResultFor(method => method.Level <= 6);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(6));
        }

        [Test]
        public void Run_GivenTestAtLevel7_ShouldReturnLevel7()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = GetTestsRunResultFor(method => method.Level <= 7);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(7));
        }

        [Test]
        public void Run_GivenTestAtLevel8_ShouldReturnLevel8()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = GetTestsRunResultFor(method => method.Level <= 8);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(8));
        }

        [Test]
        public void Run_GivenTestAtLevel9_ShouldReturnLevel9()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = GetTestsRunResultFor(method => method.Level <= 9);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(9));
            Assert.That(result.EdgeCasesCovered, Is.EqualTo(4));
            Assert.That(result.TotalEdgeCases, Is.EqualTo(4));
            Assert.That(result.NumberOfTestsImplemented, Is.EqualTo(13));
        }

        [Test]
        public void Run_GivenTestAtLevel10_WhenEdgeCaseMissing_ShouldReturnLevel9AndEdgeCases1of2()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var filter = new Func<ITestMethod, bool>(method =>
            {
                var isLevelTenEdgeCaseTest = method.Level == 9 &&
                    method.KataAnnotations.Any(annotation => annotation is EdgeCaseHintAttribute);
                return method.Level <= 9 && !isLevelTenEdgeCaseTest;
            });
            var result = GetTestsRunResultFor(filter);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(9), "Level");
            Assert.That(result.EdgeCasesCovered, Is.EqualTo(2), "EdgeCasesCovered");
            Assert.That(result.TotalEdgeCases, Is.EqualTo(4), "TotalEdgeCases");
            Assert.That(result.NumberOfTestsImplemented, Is.EqualTo(11), "NumberOfTestsImplemented");
        }

        [Test]
        public void Run_GivenTestAtLevel10_ShouldReturnLevel10()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = GetTestsRunResultFor(method => method.Level <= 10);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(10));
        }

        [Test]
        public void Run_GivenTestAtLevel11_ShouldReturnLevel11()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = GetTestsRunResultFor(method => method.Level <= 11);
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(11));
        }

    }
}