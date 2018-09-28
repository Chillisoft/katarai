using System.Linq;
using Engine.Runners;

namespace Engine.Analysers
{
    public class PlayerImplementationTestResultAnalyser : ITestResultAnalyser<PlayerImplementationRunResult>
    {
        public PlayerImplementationRunResult GetAnalysisResult(ITestResult[] testResults)
        {
            var edgeCaseImplementationResults = new EdgeCaseTestResultAnalyser().GetAnalysisResult(testResults);
            var level = new HighestLevelPassedTestResultAnalyser().GetAnalysisResult(testResults);
            var highestTestResultPassed = testResults.FirstOrDefault(testResult => testResult.TestMethod.Level == level);
            var stepShouldDo = highestTestResultPassed == null ? "" : highestTestResultPassed.TestMethod.StepShouldDo;
            return new PlayerImplementationRunResult
            {
                Level = level,
                EdgeCaseImplementationResults = edgeCaseImplementationResults,
                StepShouldDo = stepShouldDo
            };
        }


    }
}