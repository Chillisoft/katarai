using System;
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
    public class TestPlayerTestsEdgeCaseCoverageAnalyser
    {
        [Test]
        public void Constructor_GivenNullPlayerTestsLevelAnalyser_ShouldThrowException  ()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new PlayerTestsEdgeCaseCoverageAnalyser(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("playerTestsLevelAnalyser", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenPlayerTestsLevelAnalyser_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new PlayerTestsEdgeCaseCoverageAnalyser(Substitute.For<PlayerTestsLevelAnalyser>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void GetAnalysisResult_GivenNoEdgeCases_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var playerTestsLevelAnalyser = Substitute.For<IPlayerTestsLevelAnalyser>();
            var analyser = new PlayerTestsEdgeCaseCoverageAnalyser(playerTestsLevelAnalyser);
            var goldenImplementation = new GoldenImplementation(new ClassWithAttributes().GetType());
            var implementationTestResults = CreateImplementationTestResults(goldenImplementation, CreateTestResult(true) );
            playerTestsLevelAnalyser.GetAnalysisResult(implementationTestResults)
                .Returns(new PlayerTestsLevelResult {FirstGoldenImplementationPassingAllTests = goldenImplementation});
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(implementationTestResults);
            //---------------Test Result -----------------------
            CollectionAssert.IsEmpty(results);
        }

        [Test]
        public void GetAnalysisResult_GivenOneEdgeCasePassed_ShouldReturnListWithEdgeCase()
        {
            //---------------Set up test pack-------------------
            var playerTestsLevelAnalyser = Substitute.For<IPlayerTestsLevelAnalyser>();
            var analyser = new PlayerTestsEdgeCaseCoverageAnalyser(playerTestsLevelAnalyser);
            var goldenImplementation1 = new GoldenImplementation(new ClassWithShouldFailEdgeCaseTestAttributes().GetType());
            var goldenImplementation2 = new GoldenImplementation(new ClassWithAttributes().GetType());
            var implementationTestResults = CreateImplementationTestResults(goldenImplementation1, CreateTestResult(true) );
            implementationTestResults.Add(goldenImplementation2, new ITestResult[]{CreateTestResult(true)});
            playerTestsLevelAnalyser.GetAnalysisResult(implementationTestResults)
                .Returns(new PlayerTestsLevelResult {FirstGoldenImplementationPassingAllTests = goldenImplementation1});
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(implementationTestResults);
            //---------------Test Result -----------------------
            Assert.AreEqual(1,results.Count);
            Assert.AreEqual("This is a hint", results[0].EdgeCaseMessage);
            Assert.AreEqual(2, results[0].Level);
            Assert.AreEqual(false, results[0].IsCovered);
        }

        [Test]
        public void GetAnalysisResult_GivenOneEdgeCasePassedWithNoEdgeCaseHintAttribute_ShouldReturnListWithEdgeCaseAndEdgeCaseMessageSetToNull()
        {
            //---------------Set up test pack-------------------
            var playerTestsLevelAnalyser = Substitute.For<IPlayerTestsLevelAnalyser>();
            var analyser = new PlayerTestsEdgeCaseCoverageAnalyser(playerTestsLevelAnalyser);
            var goldenImplementation1 = new GoldenImplementation(new ClassWithShouldFailEdgeCaseTestAttributesWithNoHint().GetType());
            var goldenImplementation2 = new GoldenImplementation(new ClassWithAttributes().GetType());
            var implementationTestResults = CreateImplementationTestResults(goldenImplementation1, CreateTestResult(true) );
            implementationTestResults.Add(goldenImplementation2, new ITestResult[]{CreateTestResult(true)});
            playerTestsLevelAnalyser.GetAnalysisResult(implementationTestResults)
                .Returns(new PlayerTestsLevelResult {FirstGoldenImplementationPassingAllTests = goldenImplementation1});
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(implementationTestResults);
            //---------------Test Result -----------------------
            Assert.AreEqual(1,results.Count);
            Assert.IsNull(results[0].EdgeCaseMessage);
            Assert.AreEqual(2, results[0].Level);
            Assert.AreEqual(false, results[0].IsCovered);
        }

        [Test]
        public void GetAnalysisResult_GivenOneEdgeCaseFailed_ShouldReturnListWithEdgeCase()
        {
            //---------------Set up test pack-------------------
            var playerTestsLevelAnalyser = Substitute.For<IPlayerTestsLevelAnalyser>();
            var analyser = new PlayerTestsEdgeCaseCoverageAnalyser(playerTestsLevelAnalyser);
            var goldenImplementation1 = new GoldenImplementation(new ClassWithShouldFailEdgeCaseTestAttributes().GetType());
            var goldenImplementation2 = new GoldenImplementation(new ClassWithAttributes().GetType());
            var implementationTestResults = CreateImplementationTestResults(goldenImplementation1, CreateTestResult(false) );
            implementationTestResults.Add(goldenImplementation2, new ITestResult[]{CreateTestResult(true)});
            playerTestsLevelAnalyser.GetAnalysisResult(implementationTestResults)
                .Returns(new PlayerTestsLevelResult {FirstGoldenImplementationPassingAllTests = goldenImplementation1});
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(implementationTestResults);
            //---------------Test Result -----------------------
            Assert.AreEqual(1,results.Count);
            Assert.AreEqual("This is a hint", results[0].EdgeCaseMessage);
            Assert.AreEqual(2, results[0].Level);
            Assert.AreEqual(true, results[0].IsCovered);
        }

        [Test]
        public void GetAnalysisResult_GivenPassingTestInNextLevelAndEdgeCaseFailingInPreviousLevel_ShouldReturnListWithEdgeCase()
        {
            //---------------Set up test pack-------------------
            var playerTestsLevelAnalyser = Substitute.For<IPlayerTestsLevelAnalyser>();
            var analyser = new PlayerTestsEdgeCaseCoverageAnalyser(playerTestsLevelAnalyser);
            var goldenImplementation2 = new GoldenImplementation(new ClassWithAttributes().GetType());
            var implementationTestResults = CreateImplementationTestResults(goldenImplementation2, CreateTestResult(true));

            implementationTestResults.Add(new GoldenImplementation(new AnotherClassWithAttributes().GetType()), new ITestResult[] { CreateTestResult(true) });
            implementationTestResults.Add(new GoldenImplementation(new AnotherClassWithShouldFailEdgeCaseTestAttributes().GetType()), new ITestResult[] { CreateTestResult(true) });

            playerTestsLevelAnalyser.GetAnalysisResult(implementationTestResults)
                .Returns(new PlayerTestsLevelResult { FirstGoldenImplementationPassingAllTests = goldenImplementation2 });
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(implementationTestResults);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("This is a another hint", results[0].EdgeCaseMessage);
            Assert.AreEqual(1, results[0].Level);
            Assert.AreEqual(false, results[0].IsCovered);
        }

        [Test]
        public void GetAnalysisResult_GivenPassingTestInCurrentLevelAndEdgeCaseFailingInNextLevel_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var playerTestsLevelAnalyser = Substitute.For<IPlayerTestsLevelAnalyser>();
            var analyser = new PlayerTestsEdgeCaseCoverageAnalyser(playerTestsLevelAnalyser);
            var goldenImplementation2 = new GoldenImplementation(new ClassWithAttributes().GetType());
            var implementationTestResults = CreateImplementationTestResults(goldenImplementation2, CreateTestResult(true));

            implementationTestResults.Add(new GoldenImplementation(new Step3WithAttributes().GetType()), new ITestResult[] { CreateTestResult(false) });
            implementationTestResults.Add(new GoldenImplementation(new Step3WithShouldFailEdgeCaseTestAttributes().GetType()), new ITestResult[] { CreateTestResult(true) });

            playerTestsLevelAnalyser.GetAnalysisResult(implementationTestResults)
                .Returns(new PlayerTestsLevelResult { FirstGoldenImplementationPassingAllTests = goldenImplementation2 });
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(implementationTestResults);
            //---------------Test Result -----------------------
            CollectionAssert.IsEmpty(results);
        }

        [Test]
        public void GetAnalysisResult_GivenInvalidTestAndOneEdgeCaseFailed_ShouldReturnListWithEdgeCase()
        {
            //---------------Set up test pack-------------------
            var playerTestsLevelAnalyser = Substitute.For<IPlayerTestsLevelAnalyser>();
            var analyser = new PlayerTestsEdgeCaseCoverageAnalyser(playerTestsLevelAnalyser);
            var goldenImplementation1 = new GoldenImplementation(new ClassWithShouldFailEdgeCaseTestAttributes().GetType());
            var goldenImplementation2 = new GoldenImplementation(new ClassWithAttributes().GetType());
            var implementationTestResults = CreateImplementationTestResults(goldenImplementation1, CreateTestResult(false));
            implementationTestResults.Add(goldenImplementation2, new ITestResult[] { CreateTestResult(true) });
            playerTestsLevelAnalyser.GetAnalysisResult(implementationTestResults)
                .Returns(new PlayerTestsLevelResult { FirstGoldenImplementationPassingAllTests = null});
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = analyser.GetAnalysisResult(implementationTestResults, 2);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("This is a hint", results[0].EdgeCaseMessage);
            Assert.AreEqual(2, results[0].Level);
            Assert.AreEqual(true, results[0].IsCovered);
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

        [TestStep(2)]
        private class ClassWithAttributes
        {

        }

        [TestStep(2)]
        [ShouldFailEdgeCaseTest]
        [EdgeCaseHint("This is a hint")]
        private class ClassWithShouldFailEdgeCaseTestAttributes
        {

        }

        [TestStep(2)]
        [ShouldFailEdgeCaseTest]
        private class ClassWithShouldFailEdgeCaseTestAttributesWithNoHint
        {

        }


        [TestStep(1)]
        private class AnotherClassWithAttributes
        {

        }


        [TestStep(1)]
        [ShouldFailEdgeCaseTest]
        [EdgeCaseHint("This is a another hint")]
        private class AnotherClassWithShouldFailEdgeCaseTestAttributes
        {

        }


        [TestStep(3)]
        private class Step3WithAttributes
        {

        }


        [TestStep(3)]
        [ShouldFailEdgeCaseTest]
        [EdgeCaseHint("Step 3 hint")]
        private class Step3WithShouldFailEdgeCaseTestAttributes
        {

        }
    }
}