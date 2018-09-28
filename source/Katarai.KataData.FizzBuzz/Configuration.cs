using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Engine;
using Engine.Annotations;
using Katarai.FizzBuzz.Interfaces;
using Katarai.KataData.FizzBuzz.Tests;

namespace Katarai.KataData.FizzBuzz
{
    public class Configuration
    {
        public Type GetKataInterfaceType()
        {
            return typeof(IFizzBuzz);
        }

        public Type GetKataTestsType()
        {
            return typeof(TestFizzBuzz);
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
