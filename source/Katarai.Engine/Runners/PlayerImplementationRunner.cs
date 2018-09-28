using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Analysers;

namespace Engine.Runners
{
    [Serializable]
    public class PlayerImplementationRunResult
    {
        private List<EdgeCaseImplementationResult> _edgeCaseImplementationResults;
        public int Level { get; set; }
        public int EdgeCasesImplemented { get; private set; }
        public int TotalEdgeCases { get; private set; }

        public List<EdgeCaseImplementationResult> EdgeCaseImplementationResults
        {
            get { return _edgeCaseImplementationResults; }
            set
            {
                _edgeCaseImplementationResults = value;
                TotalEdgeCases = _edgeCaseImplementationResults == null 
                        ? 0 : _edgeCaseImplementationResults.Count;
                EdgeCasesImplemented = _edgeCaseImplementationResults == null 
                        ? 0 : _edgeCaseImplementationResults.Count(result => result.IsImplemented);
            }
        }

        public string StepShouldDo { get; set; }

        public override string ToString()
        {
            return string.Format("Player Implementation: Level={0}, EdgeCasesImplemented={1}, TotalEdgeCases={2}", Level, EdgeCasesImplemented, TotalEdgeCases);
        }
    }

    public interface IPlayerImplementationRunner
    {
        PlayerImplementationRunResult Run();
    }

    public class PlayerImplementationRunner<TKata> : IPlayerImplementationRunner where TKata: class
    {
        private readonly ITestFixtureRunner _testFixtureRunner;
        private readonly ITestMethodsRetriever _testMethodsRetriever;

        private Type GoldenTestType { get; set; }
        
        public PlayerImplementationRunner(Type playerImplementationType, Type goldenTestType)
        {
            if (playerImplementationType == null) throw new ArgumentNullException("playerImplementationType");
            if (goldenTestType == null) throw new ArgumentNullException("goldenTestType");
            GoldenTestType = goldenTestType;
            _testFixtureRunner = new TestFixtureRunner<TKata>(playerImplementationType);
            _testMethodsRetriever = new TestMethodsRetriever();
        }
        
        /// <summary>
        /// Runs the player's sut implementation against the golden tests and determines 
        /// the highest consecutive test step passed
        /// (amongst other things)
        /// </summary>
        public PlayerImplementationRunResult Run()
        {
            var testMethods = GetGoldenTestMethods();
            var testResults = _testFixtureRunner.RunTestFixture(testMethods);
            return GetRunResult(testResults);
        }

        private IOrderedEnumerable<ITestMethod> GetGoldenTestMethods()
        {
            return _testMethodsRetriever.GetTestMethods(GoldenTestType)
                .Where(tm => tm.Level > 0)
                .OrderBy(tm => tm.Level);
        }

        private PlayerImplementationRunResult GetRunResult(ITestResult[] testResults)
        {
            var playerImplementationTestResultAnalyser = new PlayerImplementationTestResultAnalyser();
            return playerImplementationTestResultAnalyser.GetAnalysisResult(testResults);
        }
    }
}