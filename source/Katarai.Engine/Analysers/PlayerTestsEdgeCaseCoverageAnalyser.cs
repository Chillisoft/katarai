using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Annotations;
using Engine.Runners;

namespace Engine.Analysers
{
    public class PlayerTestsEdgeCaseCoverageAnalyser
    {
        private readonly IPlayerTestsLevelAnalyser _playerTestsLevelAnalyser;

        public PlayerTestsEdgeCaseCoverageAnalyser(IPlayerTestsLevelAnalyser playerTestsLevelAnalyser)
        {
            if (playerTestsLevelAnalyser == null) throw new ArgumentNullException("playerTestsLevelAnalyser");
            _playerTestsLevelAnalyser = playerTestsLevelAnalyser;
        }

        public List<EdgeCaseCoverageResult> GetAnalysisResult(Dictionary<GoldenImplementation, ITestResult[]> implementationTestResults, int implementationLevel = 0)
        {
            var highestPassedLevel = GetHighestLevel(implementationTestResults);
            if (highestPassedLevel == -1)
            {
                highestPassedLevel = implementationLevel;
            }
            return implementationTestResults
                .Where(pair => pair.Key.StepNumber <= highestPassedLevel)
                .GroupBy(pair => pair.Key.StepNumber)
                .SelectMany(GetPlayerTestsEdgeCaseCoverageResults)
                .ToList();
        }

        private IEnumerable<EdgeCaseCoverageResult> GetPlayerTestsEdgeCaseCoverageResults(IEnumerable<KeyValuePair<GoldenImplementation, ITestResult[]>> implementationTestResults)
        {
            var levelImplementationResults = implementationTestResults.ToList();
            var primaryLevelImplementationResult = levelImplementationResults.First(pair => !HasEdgeCaseFailureAnnotation(pair.Key));
            var edgeCaseFailureImplementationsResults = levelImplementationResults.Where(pair => HasEdgeCaseFailureAnnotation(pair.Key));
            return edgeCaseFailureImplementationsResults.Select(pair => GetPlayerTestsEdgeCaseCoverageResult(primaryLevelImplementationResult, pair));
        }

        private EdgeCaseCoverageResult GetPlayerTestsEdgeCaseCoverageResult(
            KeyValuePair<GoldenImplementation, ITestResult[]> primaryLevelImplementationResult, 
            KeyValuePair<GoldenImplementation, ITestResult[]> edgeCaseResult)
        {
            var primaryPassingTestResults = primaryLevelImplementationResult.Value.Where(result => result.Passed);
            var edgeCaseImplementation = edgeCaseResult.Key;
            var equivalentEdgeCaseTestResults = edgeCaseResult.Value.Where(result => primaryPassingTestResults.Any(testResult => testResult.TestMethod == result.TestMethod));
            var failsAnyPrimaryTest = equivalentEdgeCaseTestResults.Any(result => !result.Passed);
            return new EdgeCaseCoverageResult
            {
                Level = primaryLevelImplementationResult.Key.StepNumber,
                EdgeCaseMessage = GetEdgeCaseHint(edgeCaseImplementation),
                IsCovered = failsAnyPrimaryTest
            };
        }

        private string GetEdgeCaseHint(GoldenImplementation edgeCaseImplementation)
        {
            var edgeCaseHintAttribute = edgeCaseImplementation.KataAnnotations.OfType<EdgeCaseHintAttribute>().FirstOrDefault();
            return edgeCaseHintAttribute == null ? null : edgeCaseHintAttribute.EdgeCaseHint;
        }

        private static bool HasEdgeCaseFailureAnnotation(GoldenImplementation goldenImplementation)
        {
            return goldenImplementation.KataAnnotations.OfType<ShouldFailEdgeCaseTestAttribute>().Any();
        }

        private int GetHighestLevel(Dictionary<GoldenImplementation, ITestResult[]> implementationTestResults)
        {
            var playerTestsLevelResult = _playerTestsLevelAnalyser.GetAnalysisResult(implementationTestResults);
            return playerTestsLevelResult.Level;
        }

    }

    [Serializable]
    public class EdgeCaseCoverageResult
    {
        public int Level { get; set; }
        public string EdgeCaseMessage { get; set; }
        public bool IsCovered { get; set; }
    }
}