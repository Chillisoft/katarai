using Engine;
using Engine.Analysers;
using Engine.Runners;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Engine.Tests.Analysers
{
    [TestFixture]
    public class TestHighestLevelPassedTestResultAnalyser
    {
        [Test]
        public void GetAnalysisResult_WhenNoResults_ShouldReturnZero()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var testResults = new ITestResult[0];
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetAnalysisResult_WhenPassingTestAtLevelThree_ShouldReturnThree()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreateTestMethodStub(3), Passed = true} 
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, result);
        }

        [Test]
        public void GetAnalysisResult_WhenTwoPassingTestLevels_ShouldReturnHighestTestLevel()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreateTestMethodStub(1), Passed = true}, 
                new TestResult{ TestMethod = CreateTestMethodStub(2), Passed = true} 
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetAnalysisResult_WhenManyPassingTestLevels_ShouldReturnHighestTestLevel()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreateTestMethodStub(1), Passed = true}, 
                new TestResult{ TestMethod = CreateTestMethodStub(2), Passed = true}, 
                new TestResult{ TestMethod = CreateTestMethodStub(3), Passed = true} 
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, result);
        }

        [Test]
        public void GetAnalysisResult_WhenNotPassingAllTestsInLevel2_ShouldReturnTwo()
        {
            //---------------Set up test pack-------------------
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreateTestMethodStub(1), Passed = true}, 
                new TestResult{ TestMethod = CreateTestMethodStub(2), Passed = true},
                new TestResult{ TestMethod = CreateTestMethodStub(2), Passed = false} 
            };
            var analyser = CreateAnalyser();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetAnalysisResult_WhenNonConsecutiveLevelPassed_ShouldReturnHighestConsecutiveTestLevel()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreateTestMethodStub(1), Passed = true}, 
                new TestResult{ TestMethod = CreateTestMethodStub(2), Passed = false}, 
                new TestResult{ TestMethod = CreateTestMethodStub(3), Passed = true} 
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, result);
        }
        
        private ITestMethod CreateTestMethodStub(int level)
        {
            var testMethod = Substitute.For<ITestMethod>();
            testMethod.Level.Returns(level);
            return testMethod;
        }

        private static HighestLevelPassedTestResultAnalyser CreateAnalyser()
        {
            return new HighestLevelPassedTestResultAnalyser();
        }
    }
}