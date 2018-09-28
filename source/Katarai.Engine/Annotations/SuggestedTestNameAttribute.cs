using System;

namespace Engine.Annotations
{
    public class SuggestedTestNameAttribute : Attribute, IKataAnnotation
    {
        public string SuggestedTestName { get; protected set; }

        public SuggestedTestNameAttribute(string suggestedTestName)
        {
            this.SuggestedTestName = suggestedTestName;
        }
    }
}