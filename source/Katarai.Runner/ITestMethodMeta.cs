using System;
namespace Katarai.Runner
{
    public interface ITestMethodMeta
    {
        string EdgeCaseHint { get; set; }
        string StepShoudlDo { get; set; }
        string SuggestedTestName { get; set; }
        string DebugInfo { get; set; }
    }
}
