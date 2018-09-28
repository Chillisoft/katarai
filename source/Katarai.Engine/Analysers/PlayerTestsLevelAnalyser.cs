using System.Collections.Generic;
using System.Linq;
using Engine.Annotations;
using Engine.Runners;

namespace Engine.Analysers
{
    public class PlayerTestsLevelResult
    {
        public GoldenImplementation FirstGoldenImplementationPassingAllTests { get; set; }

        public int Level
        {
            get
            {
                return FirstGoldenImplementationPassingAllTests == null
                    ? -1
                    : FirstGoldenImplementationPassingAllTests.StepNumber;
            }
        }
    }

    public interface IPlayerTestsLevelAnalyser
    {
        PlayerTestsLevelResult GetAnalysisResult(Dictionary<GoldenImplementation, ITestResult[]> implementationTestResults);
    }

    public class PlayerTestsLevelAnalyser : IPlayerTestsLevelAnalyser
    {
        public PlayerTestsLevelResult GetAnalysisResult(Dictionary<GoldenImplementation, ITestResult[]> implementationTestResults)
        {
            return new PlayerTestsLevelResult
            {
                FirstGoldenImplementationPassingAllTests = GetFirstGoldenImplementationPassingAllTests(implementationTestResults)
            };
        }

        private GoldenImplementation GetFirstGoldenImplementationPassingAllTests(Dictionary<GoldenImplementation, ITestResult[]> implementationTestResults)
        {
            var primaryImplementationTestResults = implementationTestResults
                .Where(pair => !pair.Key.KataAnnotations.OfType<ShouldFailEdgeCaseTestAttribute>().Any());
            foreach (var implementationResult in primaryImplementationTestResults)
            {
                var goldenImplementation = implementationResult.Key;
                var testResults = implementationResult.Value;
                var passesAll = testResults.All(result => result.Passed);
                if (passesAll)
                {
                    return goldenImplementation;
                }
            }
            return null;
        }
    }


}