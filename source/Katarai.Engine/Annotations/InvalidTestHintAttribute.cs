using System;

namespace Engine.Annotations
{
    public class InvalidTestHintAttribute : Attribute, IKataAnnotation
    {
        public string InvalidTestHint { get; private set; }

        public InvalidTestHintAttribute(string invalidTestHint)
        {
            InvalidTestHint = invalidTestHint;
        }
    }
}