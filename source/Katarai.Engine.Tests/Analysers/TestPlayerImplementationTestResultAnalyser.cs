using Engine;
using Engine.Analysers;
using Engine.Runners;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Engine.Tests.Analysers
{
    [TestFixture]
    public class TestPlayerImplementationTestResultAnalyser
    {
        [Test]
        [Ignore("WIP")] //TODO Soriya 20 Jan 2015: Ignored Test - WIP
        public void GetAnalysisResult_GivenTestResultsCollectionWithOneTestResult_ShouldReturnResult()
        {
            //---------------Set up test pack-------------------
            var analyser = CreateTestResultAnalyser();
            var testResults = new ITestResult[]
            {
                new TestResult{ TestMethod = CreateTestMethodStub(3), Passed = true} 
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var analysisResult = analyser.GetAnalysisResult(testResults);
            //---------------Test Result -----------------------
            //Assert.AreEqual();
        }

        private PlayerImplementationTestResultAnalyser CreateTestResultAnalyser()
        {
            var analyser = new PlayerImplementationTestResultAnalyser();
            return analyser;
        }
                private static ITestMethod CreateTestMethodStub(int level)
        {
            var testMethod = Substitute.For<ITestMethod>();
            testMethod.Level.Returns(level);
            return testMethod;
        }
    }
}