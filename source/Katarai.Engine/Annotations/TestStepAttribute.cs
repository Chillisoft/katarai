using System;

namespace Engine.Annotations
{
    public class TestStepAttribute : Attribute, IKataAnnotation
    {
        public int StepNumber { get; protected set; }

        public TestStepAttribute(int stepNumber)
        {
            this.StepNumber = stepNumber;
        }
    }
}
