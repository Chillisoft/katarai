using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Engine.Annotations;
using NUnit.Framework;

namespace Engine
{
    public class TestMethod : ITestMethod
    {
        public MethodInfo Method { get; private set; }
        public int Level { get; private set; }
        public IList<IKataAnnotation> KataAnnotations { get; set; }
        public bool HasTestCaseAttribute { get; private set; }
        public string EdgeCaseHint { get; private set; }   //TODO: test this
        public string StepShouldDo { get; private set; }  //TODO: test this
        public bool HasDoNotShowImplementedTooMuchMessageAttribute { get; private set; }
        public bool HasExpectedExceptionAttribute { get; set; }

        public TestMethod(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException("methodInfo");
            Method = methodInfo;
            KataAnnotations = methodInfo.GetCustomAttributes().OfType<IKataAnnotation>().ToList();
            var testStepAttrib = KataAnnotations.OfType<TestStepAttribute>().FirstOrDefault();
            if (testStepAttrib != null)
            {
                Level = testStepAttrib.StepNumber;
            }
            SetEdgeCaseHint();
            HasTestCaseAttribute = methodInfo.GetCustomAttributes().OfType<TestCaseAttribute>().ToList().Any();
            HasExpectedExceptionAttribute = methodInfo.GetCustomAttributes().OfType<ExpectedException>().ToList().Any();
            HasDoNotShowImplementedTooMuchMessageAttribute = methodInfo.GetCustomAttributes().OfType<DoNotShowImplementedTooMuchMessage>().ToList().Any();
            SetStepShouldDo();
        }

        private void SetEdgeCaseHint()
        {
            var edgeCaseHintAttribute = KataAnnotations.OfType<EdgeCaseHintAttribute>().FirstOrDefault();
            if (edgeCaseHintAttribute != null)
            {
                EdgeCaseHint = edgeCaseHintAttribute.EdgeCaseHint;
            }
        }

        private void SetStepShouldDo()
        {
            var stepShouldDoAttribute = KataAnnotations.OfType<StepShouldDoAttribute>().FirstOrDefault();
            if (stepShouldDoAttribute != null)
            {
                StepShouldDo = stepShouldDoAttribute.ShouldDoText;
            }
        }
        
        public override string ToString()
        {
            //TODO mark 02 Feb 2015: Test This
            var levelToString = string.Format("Level {0}", Level);
            return string.Format("{0}: '{1}'", levelToString, Method == null ? "[Unknown]" : Method.Name);
        }
    }
}