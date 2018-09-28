using System.Linq;
using Engine.Runners;

namespace Katarai.Runner
{
    public class ResultAnalyser
    {
        public string GetStateCode(IOverallAnalysisResult overallAnalysisResult)
        {
            var progress = "";
            if (overallAnalysisResult.PlayerTestsRunResult.HasTestCaseAttribute)
            {
                return "HTC";
            }
            if (overallAnalysisResult.PlayerTestsRunResult.HasExpectedExceptionAttribute)
            {
                return "HEE";
            }
            var implementationResult = overallAnalysisResult.PlayerImplementationRunResult;
            var playerImplementationLevel = implementationResult.Level;
            var playerTestFixtureLevel = overallAnalysisResult.PlayerTestsRunResult.Level;

            var isOnKataStart = playerImplementationLevel == 1;
            var lastKataLevel = overallAnalysisResult.GoldenTestMethods.Max(method => method.Level);
            var isLastLevelImplemented = lastKataLevel == playerImplementationLevel;

            var playerTestsRunResult = overallAnalysisResult.PlayerTestsRunResult;
            var totalEdgeCases = playerTestsRunResult.TotalEdgeCases;
            var edgeCasesCovered = playerTestsRunResult.EdgeCasesCovered;
            var edgeCasesImplemented = implementationResult.EdgeCasesImplemented;
            var minLevelEdgeCasesNotCovered = GetMinLevelEdgeCasesNotCovered(playerTestsRunResult);
            
            var suffix = (isLastLevelImplemented ? " (I=Last)" : "");
            suffix += (isOnKataStart ? " (I=1)" : "");
            if (playerTestFixtureLevel < playerImplementationLevel) progress = "T<I";
            if (playerTestFixtureLevel == playerImplementationLevel + 1)
            {
                progress = "T=I+1";
                if (MustAddTotalEdgeCaseCoverageProgress(totalEdgeCases, playerTestFixtureLevel,
                    minLevelEdgeCasesNotCovered))
                {
                    progress += " (TE>EC)";
                }
                else
                {
                    progress = AddEdgeCaseProgress(totalEdgeCases, edgeCasesCovered, edgeCasesImplemented, progress);
                }
            }
            if (playerTestFixtureLevel > playerImplementationLevel + 1)
            {
                progress = "T>I+1";
                progress = AddTotalEdgeCaseCoverageProgress(totalEdgeCases, playerTestFixtureLevel,
                    minLevelEdgeCasesNotCovered, progress);
            }
            if (playerTestFixtureLevel == playerImplementationLevel)
            {
                progress = "T=I";
                if (totalEdgeCases > 0 && totalEdgeCases > edgeCasesCovered)
                {
                    progress += " (TE>EC)";
                }
                else
                {
                    progress = AddEdgeCaseProgress(totalEdgeCases, edgeCasesCovered, edgeCasesImplemented, progress);
                }
            }
            if (playerTestFixtureLevel == -1) progress = "T=-1";
            var prefix = overallAnalysisResult.PlayerTestsPlayerImplementationRunResult.Passed ? "[G] " : "[R] ";
            return prefix + progress + suffix;
        }

        public int GetMinLevelEdgeCasesNotCovered(PlayerTestsRunResult playerTestsRunResult)
        {
            var playerTestsEdgeCoverageResults = playerTestsRunResult.PlayerTestsEdgeCoverageResults;
            var minLevelEdgeCasesNotCovered = playerTestsRunResult.Level;
            if (playerTestsEdgeCoverageResults != null)
            {
                var edgeCasesNotCoveredResults =
                    playerTestsEdgeCoverageResults.Where(result => !result.IsCovered).ToList();
                if (edgeCasesNotCoveredResults.Any())
                {
                    minLevelEdgeCasesNotCovered = edgeCasesNotCoveredResults.Min(result => result.Level);
                }
            }
            return minLevelEdgeCasesNotCovered;
        }

        private string AddTotalEdgeCaseCoverageProgress(int totalEdgeCases, int playerTestFixtureLevel,
            int minLevelEdgeCasesNoCovered, string progress)
        {
            if (MustAddTotalEdgeCaseCoverageProgress(totalEdgeCases, playerTestFixtureLevel, minLevelEdgeCasesNoCovered))
            {
                progress += " (TE>EC)";
            }
            return progress;
        }

        private bool MustAddTotalEdgeCaseCoverageProgress(int totalEdgeCases, int playerTestFixtureLevel,
            int minLevelEdgeCasesNoCovered)
        {
            if (totalEdgeCases <= 0) return false;
            return playerTestFixtureLevel > minLevelEdgeCasesNoCovered;
        }

        private string AddEdgeCaseProgress(int totalEdgeCases, int edgeCasesCovered, int edgeCasesImplemented,
            string progress)
        {
            if (totalEdgeCases <= 0) return progress;
            if (edgeCasesImplemented == 0)
            {
                progress += " (EI=0)";
                return progress;
            }
            if (totalEdgeCases == edgeCasesCovered && totalEdgeCases == edgeCasesImplemented)
            {
                progress += " (TE=EC) (EC=EI)";
                return progress;
            }
            if (edgeCasesCovered < edgeCasesImplemented)
            {
                progress += " (EC<EI)";
            }
            if (edgeCasesCovered > edgeCasesImplemented)
            {
                progress += " (EC>EI)";
            }
            if (edgeCasesCovered == edgeCasesImplemented)
            {
                progress += " (EC=EI)";
            }
            return progress;
        }
    }
}