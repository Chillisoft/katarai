using Engine;
using Engine.Analysers;
using Engine.Annotations;
using Engine.Runners;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;

namespace Katarai.Engine.Tests.Analysers
{
    [TestFixture]
    public class TestEdgeCaseTestResultAnalyser
    {
        [Test]
        public void GetAnalysisResult_WhenNoResults_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var testResults = new ITestResult[0];
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void GetAnalysisResult_WhenPassingPrimaryTest_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(1), Passed = true} 
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void GetAnalysisResult_WhenPassingPrimaryTest_WithSinglePassingEdge_ShouldReturnListWithOneResult()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var edgeTestForLevel = CreateEdgeTestForLevel(1);
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(1), Passed = true},
                new TestResult{ TestMethod = edgeTestForLevel, Passed = true} 
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.That(results.Count, Is.EqualTo(1));
            AssertResultIs(results[0], edgeTestForLevel, true);
        }

        [Test]
        public void GetAnalysisResult_WhenPassingPrimaryTest_WithSingleFailingEdge_ShouldReturnListWithOneResult()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var edgeTestForLevel = CreateEdgeTestForLevel(1);
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(1), Passed = true},
                new TestResult{ TestMethod = edgeTestForLevel, Passed = false} 
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.That(results.Count, Is.EqualTo(1));
            AssertResultIs(results[0], edgeTestForLevel, false);
        }

        [Test]
        public void GetAnalysisResult_WhenFailingPrimaryTest_AndNoOtherTests_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(1), Passed = false}
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void GetAnalysisResult_WhenPassingPrimaryTest_WithOnePassingEdgeCaseOfTwo_ShouldReturnListWithTwoResults()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var edgeTest1ForLevel = CreateEdgeTestForLevel(1);
            var edgeTest2ForLevel = CreateEdgeTestForLevel(1);
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(1), Passed = true}, 
                new TestResult{ TestMethod = edgeTest1ForLevel, Passed = true}, 
                new TestResult{ TestMethod = edgeTest2ForLevel, Passed = false} 
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.That(results.Count, Is.EqualTo(2));
            AssertResultIs(results[0], edgeTest1ForLevel, true);
            AssertResultIs(results[1], edgeTest2ForLevel, false);
        }

        [Test]
        public void GetAnalysisResult_WithSecondLevel_WhenZeroPassingOfTwo_ShouldReturnListWithTwoResults()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var edgeTest1ForLevel = CreateEdgeTestForLevel(2);
            var edgeTest2ForLevel = CreateEdgeTestForLevel(2);
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(1), Passed = true}, 
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(2), Passed = true}, 
                new TestResult{ TestMethod = edgeTest1ForLevel, Passed = false}, 
                new TestResult{ TestMethod = edgeTest2ForLevel, Passed = false}, 
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(3), Passed = false} 
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.That(results.Count, Is.EqualTo(2));
            AssertResultIs(results[0], edgeTest1ForLevel, false);
            AssertResultIs(results[1], edgeTest2ForLevel, false);
        }

        [Test]
        public void GetAnalysisResult_WithSecondLevel_WhenOnePassingOfTwo_ShouldReturnListWithTwoResults()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var edgeTest1ForLevel = CreateEdgeTestForLevel(2);
            var edgeTest2ForLevel = CreateEdgeTestForLevel(2);
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(1), Passed = true}, 
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(2), Passed = true},  
                new TestResult{ TestMethod = edgeTest1ForLevel, Passed = true}, 
                new TestResult{ TestMethod = edgeTest2ForLevel, Passed = false}, 
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(3), Passed = false}
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.That(results.Count, Is.EqualTo(2));
            AssertResultIs(results[0], edgeTest1ForLevel, true);
            AssertResultIs(results[1], edgeTest2ForLevel, false);
        }

        [Test]
        public void GetAnalysisResult_WithSecondLevel_WhenOnePassingOfTwo_AndNextLevelPasses_ShouldReturnListWithTwoResults()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var edgeTest1ForLevel = CreateEdgeTestForLevel(2);
            var edgeTest2ForLevel = CreateEdgeTestForLevel(2);
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(1), Passed = true}, 
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(2), Passed = true},  
                new TestResult{ TestMethod = edgeTest1ForLevel, Passed = true}, 
                new TestResult{ TestMethod = edgeTest2ForLevel, Passed = false}, 
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(3), Passed = true}
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.That(results.Count, Is.EqualTo(2));
            AssertResultIs(results[0], edgeTest1ForLevel, true);
            AssertResultIs(results[1], edgeTest2ForLevel, false);
        }

        [Test]
        public void GetAnalysisResult_WithThirdLevel_WhenZeroPassingOfOne_ShouldReturnListWithThreeResults()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var edgeTest1ForLevel2 = CreateEdgeTestForLevel(2);
            var edgeTest2ForLevel2 = CreateEdgeTestForLevel(2);
            var edgeTest1ForLevel3 = CreateEdgeTestForLevel(2);
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(1), Passed = true}, 
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(2), Passed = true}, 
                new TestResult{ TestMethod = edgeTest1ForLevel2, Passed = true}, 
                new TestResult{ TestMethod = edgeTest2ForLevel2, Passed = true}, 
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(3), Passed = true},
                new TestResult{ TestMethod = edgeTest1ForLevel3, Passed = false} 
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.That(results.Count, Is.EqualTo(3));
            AssertResultIs(results[0], edgeTest1ForLevel2, true);
            AssertResultIs(results[1], edgeTest2ForLevel2, true);
            AssertResultIs(results[2], edgeTest1ForLevel3, false);
        }

        [Test]
        public void GetAnalysisResult_WithLastLevel_WhenComplete_ShouldReturnListWithTwoResults()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateAnalyser();
            var edgeTest1ForLevel = CreateEdgeTestForLevel(2);
            var edgeTest2ForLevel = CreateEdgeTestForLevel(2);
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(1), Passed = true}, 
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(2), Passed = true}, 
                new TestResult{ TestMethod = edgeTest1ForLevel, Passed = true}, 
                new TestResult{ TestMethod = edgeTest2ForLevel, Passed = true}, 
                new TestResult{ TestMethod = CreatePrimaryTestForLevel(3), Passed = true}
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            Assert.That(results.Count, Is.EqualTo(2));
            AssertResultIs(results[0], edgeTest1ForLevel, true);
            AssertResultIs(results[1], edgeTest2ForLevel, true);
        }

        private static void AssertResultIs(EdgeCaseImplementationResult edgeCaseImplementationResult,
                ITestMethod edgeTestForLevel,
                bool isImplemented)
        {
            Assert.That(edgeCaseImplementationResult.Level, Is.EqualTo(edgeTestForLevel.Level));
            Assert.That(edgeCaseImplementationResult.EdgeCaseMessage, Is.EqualTo(edgeTestForLevel.EdgeCaseHint));
            Assert.That(edgeCaseImplementationResult.IsImplemented, Is.EqualTo(isImplemented));
        }


        private static ITestMethod CreateEdgeTestForLevel(int level, string edgeCaseHint = "some hint")
        {
            var testMethod = CreatePrimaryTestForLevel(level);
            testMethod.KataAnnotations = new IKataAnnotation[] { new EdgeCaseHintAttribute(edgeCaseHint) };
            return testMethod;
        }

        private static ITestMethod CreatePrimaryTestForLevel(int level)
        {
            var testMethod = Substitute.For<ITestMethod>();
            testMethod.Level.Returns(level);
            return testMethod;
        }

        private static EdgeCaseTestResultAnalyser CreateAnalyser()
        {
            var analyser = new EdgeCaseTestResultAnalyser();
            return analyser;
        }
    }
}