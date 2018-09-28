namespace Katarai.Runner
{
    public class TestMethodMeta : ITestMethodMeta
    {
        public string StepShoudlDo { get; set; }
        public string EdgeCaseHint { get; set; }
        public string SuggestedTestName { get; set; }
        public string DebugInfo { get; set; }
    }
}
