using System;

namespace Engine.Annotations
{
    public class EdgeCaseHintAttribute : Attribute, IKataAnnotation
    {
        public string EdgeCaseHint { get; protected set; }

        public EdgeCaseHintAttribute(string hintText)
        {
            this.EdgeCaseHint = hintText;
        }
    }
}