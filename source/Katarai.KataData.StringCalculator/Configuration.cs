using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Engine;
using Engine.Annotations;
using Katarai.KataData.StringCalculator.Tests;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.KataData.StringCalculator
{
    public class Configuration
    {
        public Type GetKataInterfaceType()
        {
            return typeof(IStringCalculator);
        }

        public Type GetKataTestsType()
        {
            return typeof(TestStringCalculator);
        }

        public IEnumerable<Type> GetKataImplementationTypes()
        {
            var interfaceType = GetKataInterfaceType();
            return Assembly.GetExecutingAssembly()
                            .GetTypes()
                            .Where(t => interfaceType.IsAssignableFrom(t))
                            .Where(t => t.GetCustomAttributes(true).OfType<TestStepAttribute>().FirstOrDefault() != null)
                            .OrderBy(t => t.GetCustomAttributes(true).OfType<TestStepAttribute>().FirstOrDefault().StepNumber);
        }
    }
}
