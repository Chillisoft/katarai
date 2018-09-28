using System.Linq;
using Engine.Annotations;
using Engine.Runners;

namespace Engine
{
    public interface ITestResultAnalyser<TResult>
    {
        TResult GetAnalysisResult(ITestResult[] testResults);
    }

    public class TestResultAnalyserBase
    {
        protected ITestResult GetLastPassingConsecutiveTestResult(ITestResult[] testResults)
        {
            var lastPassingTestResult = testResults
                .OrderBy(result => result.TestMethod.Level)
                .ThenBy(result => IsEdgeCaseTest(result.TestMethod)) //Primary Tests First
                .ThenByDescending(result => result.Passed) //False at the bottom of the level
                .TakeWhile(result => result.Passed)
                .LastOrDefault();
            return lastPassingTestResult;
        }

        protected bool IsEdgeCaseTest(ITestMethod testMethod)
        {
            return testMethod.KataAnnotations.OfType<EdgeCaseHintAttribute>().Any();
        }
    }
}