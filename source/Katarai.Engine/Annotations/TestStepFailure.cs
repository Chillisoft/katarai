using System;

namespace Engine.Annotations
{
    public class TestStepFailure : Attribute, IKataAnnotation
    {
        public int StepNumber { get; protected set; }
        public string Message { get; protected set; }

        public TestStepFailure(int stepNumber, string message)
        {
            this.StepNumber = stepNumber;
            Message = message;
        }
    }
}