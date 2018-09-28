using System.Collections.Generic;
using Engine;
using Engine.Analysers;
using Engine.Annotations;
using Engine.Runners;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Engine.Tests.Analysers
{
    [TestFixture]
    public class TestPlayerTestsLevelAnalyser
    {
        [Test]
        public void GetAnalysisResult_WhenNoneOfThePlayersTestArePassing_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var goldenImplementation = new GoldenImplementation(new ClassWithAttributes().GetType());
            var testResult = CreateTestResult(false);
            var implementationTestResults = CreateImplementationTestResults(goldenImplementation, testResult);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = analyser.GetAnalysisResult(implementationTestResults);
            //---------------Test Result -----------------------
            Assert.IsNull(result.FirstGoldenImplementationPassingAllTests);
        }

        [Test]
        public void GetAnalysisResult_WhenOneOfThePlayersTestArePassing_ShouldReturnGoldenImplementation()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var goldenImplementation = new GoldenImplementation(new ClassWithAttributes().GetType());
            var testResult = CreateTestResult(true);

            var implementationTestResults = CreateImplementationTestResults(goldenImplementation, testResult);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = analyser.GetAnalysisResult(implementationTestResults);
            //---------------Test Result -----------------------
            Assert.AreSame(goldenImplementation, result.FirstGoldenImplementationPassingAllTests);
        }

        [Test]
        public void GetAnalysisResult_WhenNotAllTestsInLevel2Passes_ShouldReturnLevel1GoldenImplementation()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var goldenImplementation1 = CreateGoldenImplementation();
            var goldenImplementation2 = CreateGoldenImplementation();
            var goldenImplementation3 = CreateGoldenImplementation();
            var testResults1 = CreateTestResults(1, 1, true);
            var testResults2 = CreateTestResults(2, 2, true, false);
            var testResults3 = CreateTestResults(3, 1, false);
            var implementationTestResults = new Dictionary<GoldenImplementation, ITestResult[]>
                {
                    {goldenImplementation3, testResults3},
                    {goldenImplementation2, testResults2},
                    {goldenImplementation1, testResults1}
                };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = analyser.GetAnalysisResult(implementationTestResults);
            //---------------Test Result -----------------------
            Assert.AreSame(goldenImplementation1, result.FirstGoldenImplementationPassingAllTests);
        }

        [Test]
        public void GetAnalysisResult_WhenTypeHasShouldFailEdgeCaseTestAttribute_ShouldBeIgnored()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var goldenImplementation1 = new GoldenImplementation(new ClassWithShouldFailEdgeCaseTestAttributes().GetType());
            var goldenImplementation2 = CreateGoldenImplementation();
            var goldenImplementation3 = CreateGoldenImplementation();
            var testResults1 = CreateTestResults(1, 1, true);
            var testResults2 = CreateTestResults(2, 2, true, false);
            var testResults3 = CreateTestResults(3, 1, false);
            var implementationTestResults = new Dictionary<GoldenImplementation, ITestResult[]>
                {
                    {goldenImplementation3, testResults3},
                    {goldenImplementation2, testResults2},
                    {goldenImplementation1, testResults1}
                };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = analyser.GetAnalysisResult(implementationTestResults);
            //---------------Test Result -----------------------
            Assert.IsNull(result.FirstGoldenImplementationPassingAllTests);
        }

        private ITestResult[] CreateTestResults(int level, int noOfTests, params bool[] testsPassed)
        {
            var testResults = new List<ITestResult>();
            for (var i = 0; i < noOfTests; i++)
            {
                var testMethodStub = CreateTestMethodStub(level);
                testResults.Add(CreateTestResult(testsPassed[i], testMethodStub));
            }
            return testResults.ToArray();
        }

        private static TestResult CreateTestResult(bool passed, ITestMethod testMethodStub)
        {
            return new TestResult{Passed = passed, TestMethod = testMethodStub};
        }

        private static GoldenImplementation CreateGoldenImplementation()
        {
            return new GoldenImplementation(new ClassWithAttributes().GetType());
        }

        private static Dictionary<GoldenImplementation, ITestResult[]> CreateImplementationTestResults(GoldenImplementation goldenImplementation,
            TestResult testResult)
        {
            var implementationTestResults = new Dictionary<GoldenImplementation, ITestResult[]>
            {
                {
                    goldenImplementation, new ITestResult[]
                    {
                        testResult
                    }
                }
            };
            return implementationTestResults;
        }

        private TestResult CreateTestResult(bool passed)
        {
            var testResult = new TestResult
            {
                Passed = passed
            };
            return testResult;
        }


        private PlayerTestsLevelAnalyser CreateAnalyser()
        {
            return new PlayerTestsLevelAnalyser();
        }

        private ITestMethod CreateTestMethodStub(int level)
        {
            var testMethod = Substitute.For<ITestMethod>();
            testMethod.Level.Returns(level);
            return testMethod;
        }

        [TestStep(1)]
        private class ClassWithAttributes
        {

        }

        [ShouldFailEdgeCaseTest]
        private class ClassWithShouldFailEdgeCaseTestAttributes
        {

        }
    }
}