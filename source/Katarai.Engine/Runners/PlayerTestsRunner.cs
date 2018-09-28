using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Analysers;

namespace Engine.Runners
{
    [Serializable]
    public class PlayerTestsRunResult
    {
        public int Level { get; set; }
        public int NumberOfTestsImplemented { get; set; }
        public List<EdgeCaseCoverageResult> PlayerTestsEdgeCoverageResults { get; set; }

        public override string ToString()
        {
            return string.Format("Player Test Fixture : Level={1}, NumberOfTestsImplemented={2}{0}{3}", 
                Environment.NewLine,  Level,
                NumberOfTestsImplemented, GetEdgeCasesMessage());
        }

        private string GetEdgeCasesMessage()
        {
            var messagesForEdgeCasesNotCovered = GetMessagesForEdgeCasesNotCovered();
            var edgeCasesMissed = string.Empty;
            if (messagesForEdgeCasesNotCovered.Any())
            {
                edgeCasesMissed = "Edge Cases Missed:" + Environment.NewLine +
                                  string.Join(Environment.NewLine, messagesForEdgeCasesNotCovered);
            }
            return string.Format("Edge Cases Covered = {1} of {2}{0}{3}", Environment.NewLine, EdgeCasesCovered,
                TotalEdgeCases, edgeCasesMissed);
        }

        public List<string> GetMessagesForEdgeCasesNotCovered()
        {
            var messagesForEdgeCasesNotCovered = new List<string>();
            if (PlayerTestsEdgeCoverageResults != null)
            {
                messagesForEdgeCasesNotCovered = PlayerTestsEdgeCoverageResults.Where(result => !result.IsCovered)
                    .Select(result => result.EdgeCaseMessage)
                    .ToList();
            }
            return messagesForEdgeCasesNotCovered;
        }

        public int TotalEdgeCases
        {
            get { return PlayerTestsEdgeCoverageResults == null ? 0 : PlayerTestsEdgeCoverageResults.Count; }
        }

        public int EdgeCasesCovered
        {
            get
            {
                if (PlayerTestsEdgeCoverageResults == null) return 0;
                return PlayerTestsEdgeCoverageResults.Count(result => result.IsCovered);
            }
        }

        public bool HasTestCaseAttribute { get; set; }
        public bool HasExpectedExceptionAttribute { get; set; }
    }

    public interface IPlayerTestsRunner
    {
        PlayerTestsRunResult Run();
    }

    public class PlayerTestsRunner<TKata> : IPlayerTestsRunner where TKata: class
    {
        private readonly Type _playerTestFixtureType;
        private readonly int _implementationLevel;
        private readonly GoldenImplementation[] _goldenImplementations;
        private readonly ITestMethodsRetriever _testMethodsRetriever;

        public PlayerTestsRunner(Type playerTestFixtureType, Type[] goldenImplementationTypes, ITestMethodsRetriever testMethodsRetriever = null, int implementationLevel = 0)
        {
            CheckTestFixtureType(playerTestFixtureType);
            if (goldenImplementationTypes.Length == 0) throw new ArgumentException("No implementationTypes specified", "goldenImplementationTypes");
            _playerTestFixtureType = playerTestFixtureType;
            _implementationLevel = implementationLevel;
            _testMethodsRetriever = testMethodsRetriever ?? new TestMethodsRetriever();
            var goldenImplementationRetriever = new GoldenImplementationRetriever();
            _goldenImplementations = goldenImplementationRetriever.GetGoldenImplementations(goldenImplementationTypes);
        }

        /// <summary>
        /// Runs the player's tests against the golden sut implementations and determines 
        /// golden implementation with the highest test step attribute which passes all the 
        /// player tests (amongst other things)
        /// </summary>
        public PlayerTestsRunResult Run()
        {

            var testMethods = _testMethodsRetriever.GetTestMethods(_playerTestFixtureType);
            var goldenImplementationTestResults = GetGoldenImplementationTestResults(testMethods);

            var testLevelAnalyser = new PlayerTestsLevelAnalyser();
            var playerTestsLevelResult = testLevelAnalyser.GetAnalysisResult(goldenImplementationTestResults);
            var edgeCaseCoverageAnalyser = new PlayerTestsEdgeCaseCoverageAnalyser(testLevelAnalyser);
            var testsEdgeCoverageResults = edgeCaseCoverageAnalyser.GetAnalysisResult(goldenImplementationTestResults, _implementationLevel);

            var numberOfTestsImplemented = testMethods.Count();
            var hasTestCaseAttribute = testMethods.Any(method => method.HasTestCaseAttribute);
            var hasExpectedExceptionAttribute = testMethods.Any(method => method.HasExpectedExceptionAttribute);
            return new PlayerTestsRunResult
            {
                Level = playerTestsLevelResult.Level,
                NumberOfTestsImplemented = numberOfTestsImplemented,
                PlayerTestsEdgeCoverageResults = testsEdgeCoverageResults,
                HasTestCaseAttribute = hasTestCaseAttribute,
                HasExpectedExceptionAttribute = hasExpectedExceptionAttribute
            };
        }

        private Dictionary<GoldenImplementation, ITestResult[]> GetGoldenImplementationTestResults(ITestMethod[] testMethods)
        {
            var goldenImplementationTestResults = new Dictionary<GoldenImplementation, ITestResult[]>();
            foreach (var goldenImplementation in _goldenImplementations.OrderBy(i => i.StepNumber))
            {
                var testFixtureRunner = new TestFixtureRunner<TKata>(goldenImplementation.Type);
                var testResults = testFixtureRunner.RunTestFixture(testMethods);
                goldenImplementationTestResults.Add(goldenImplementation, testResults);
            }
            return goldenImplementationTestResults;
        }

        private void CheckTestFixtureType(Type playerTestFixtureType)
        {
            if (playerTestFixtureType == null) throw new ArgumentException("playerTestFixtureType cannot be null", "playerTestFixtureType");
            var shouldImplement = typeof(ITestPack<TKata>);
            if (!shouldImplement.IsAssignableFrom(playerTestFixtureType))
                throw new ArgumentException("playerTestFixtureType should implement ITestPack<" + typeof(TKata).Name + ">", "playerTestFixtureType");

        }
    }
}
