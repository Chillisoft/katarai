using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Runners;

namespace Engine.Analysers
{
    [Serializable]
    public class EdgeCaseImplementationResult
    {
        public int Level { get; set; }
        public string EdgeCaseMessage { get; set; }
        public bool IsImplemented { get; set; }
    }

    public class EdgeCaseTestResultAnalyser : TestResultAnalyserBase, ITestResultAnalyser<List<EdgeCaseImplementationResult>>
    {
        public List<EdgeCaseImplementationResult> GetAnalysisResult(ITestResult[] testResults)
        {

            var lastPassedLevel = GetLastCompletedLevel(testResults);
            var edgeCaseTestResultsForLevel = GetEdgeCaseTestResultsForLevel(testResults, lastPassedLevel);
            var results = edgeCaseTestResultsForLevel.ToList();
            return results.Select(CreateEdgeCaseImplementationResult).ToList();
        }

        private static EdgeCaseImplementationResult CreateEdgeCaseImplementationResult(ITestResult testResult)
        {
            var edgeCaseImplementationResult = new EdgeCaseImplementationResult
            {
                Level = testResult.TestMethod.Level,
                IsImplemented = testResult.Passed,
                EdgeCaseMessage = testResult.TestMethod.EdgeCaseHint
            };
            return edgeCaseImplementationResult;
        }

        private int GetLastCompletedLevel(ITestResult[] testResults)
        {
            var lastPassingConsecutiveTestResult = GetLastPassingConsecutiveTestResult(testResults);
            if (lastPassingConsecutiveTestResult == null) return 0;
            var testMethod = lastPassingConsecutiveTestResult.TestMethod;
            return testMethod == null ? 0 : testMethod.Level;
        }

        private IEnumerable<ITestResult> GetEdgeCaseTestResultsForLevel(IEnumerable<ITestResult> testMethodResults, int level)
        {
            return testMethodResults
                .Where(result => result.TestMethod.Level <= level)
                .Where(result => IsEdgeCaseTest(result.TestMethod)); 
        }
    }
}