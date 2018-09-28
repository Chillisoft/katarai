using System;

namespace Engine.Annotations
{
    public class StepShouldDoAttribute : Attribute, IKataAnnotation
    {
        public string ShouldDoText { get; protected set; }

        public StepShouldDoAttribute(string shouldDoText)
        {
            this.ShouldDoText = shouldDoText;
        }
    }
}