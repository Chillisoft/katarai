using System.Collections.Generic;
using System.Reflection;
using Engine.Annotations;

namespace Engine
{
    public interface ITestMethod
    {
        MethodInfo Method { get; }
        int Level { get; }
        IList<IKataAnnotation> KataAnnotations { get; set; }
        bool HasTestCaseAttribute { get; }
        string EdgeCaseHint { get; }
        string StepShouldDo { get; }
        bool HasDoNotShowImplementedTooMuchMessageAttribute { get; }
        bool HasExpectedExceptionAttribute { get; }
    }
}