using Engine.Runners;

namespace Engine.Analysers
{
    public class HighestLevelPassedTestResultAnalyser : TestResultAnalyserBase, ITestResultAnalyser<int>
    {
        public int GetAnalysisResult(ITestResult[] testResults)
        {
            var lastPassingTestResult = GetLastPassingConsecutiveTestResult(testResults);
            var highestConsecutiveLevelPassed = 0;
            if (lastPassingTestResult != null)
            {
                highestConsecutiveLevelPassed = lastPassingTestResult.TestMethod.Level;
            }
            return highestConsecutiveLevelPassed;
        }
    }
}