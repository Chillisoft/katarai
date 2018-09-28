using System;

namespace Katarai.Runner
{
    [Serializable]
    public class PlayerFeedback 
    {
        public string Progress { get;  set; } // Message
        public string Hint { get; set; }
        public string DebugInfo { get; set; }
        public string PlayerTestState { get; set; }
        public string KataStateCode { get; set; }
        // v ... not used ... v
        public string StepShouldDo { get;  set; } // Instruction
        public string EdgeCaseHint { get;  set; } // Hint
        public string SuggestedTestName { get;  set; }
        public bool KataCompleted { get; set; }
        public bool AllLevelsCompleted { get; set; }
    }
}