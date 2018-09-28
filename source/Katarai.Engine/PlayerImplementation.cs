using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Annotations;

namespace Engine
{
    public class PlayerImplementation
    {
        public Type Type { get; private set; }
        public int StepNumber { get; private set; }
        public IList<IKataAnnotation> KataAnnotations { get; set; }

        public PlayerImplementation(Type type)
        {
            Type = type;
            KataAnnotations = type.GetCustomAttributes(true).OfType<IKataAnnotation>().ToList();
            var testStepAttribute = type.GetCustomAttributes(true).OfType<TestStepAttribute>().FirstOrDefault();
            if (testStepAttribute != null)
            {
                StepNumber = testStepAttribute.StepNumber;
            }
        }
    }
}