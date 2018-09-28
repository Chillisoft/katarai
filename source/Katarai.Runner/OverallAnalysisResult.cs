using Engine;
using Engine.Runners;

namespace Katarai.Runner
{
    public interface IOverallAnalysisResult
    {
        PlayerImplementationRunResult PlayerImplementationRunResult { get; }
        PlayerTestsRunResult PlayerTestsRunResult { get; }
        PlayerTestsPlayerImplementationRunResult PlayerTestsPlayerImplementationRunResult { get; }
        ITestMethod[] GoldenTestMethods { get; }
    }

    public class OverallAnalysisResult : IOverallAnalysisResult
    {
        public PlayerImplementationRunResult PlayerImplementationRunResult { get; private set; }
        public PlayerTestsRunResult PlayerTestsRunResult { get; private set; }
        public PlayerTestsPlayerImplementationRunResult PlayerTestsPlayerImplementationRunResult { get; private set; }
        public ITestMethod[] GoldenTestMethods { get; private set; }

        public OverallAnalysisResult(PlayerImplementationRunResult playerImplementationRunResult, PlayerTestsRunResult playerTestsRunResult, PlayerTestsPlayerImplementationRunResult playerTestsPlayerImplementationRunResult, ITestMethod[] goldenTestMethods)
        {
            this.PlayerImplementationRunResult = playerImplementationRunResult;
            this.PlayerTestsRunResult = playerTestsRunResult;
            this.PlayerTestsPlayerImplementationRunResult = playerTestsPlayerImplementationRunResult;
            this.GoldenTestMethods = goldenTestMethods;
        }
    }
}