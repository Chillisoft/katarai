using System;
using System.Linq;
using System.Reflection;

namespace Engine.Runners
{
    public interface ITestMethodsRetriever
    {
        ITestMethod[] GetTestMethods(Type testFixtureClassType);
    }

    public class TestMethodsRetriever : ITestMethodsRetriever
    {
        public virtual ITestMethod[] GetTestMethods(Type testFixtureClassType)
        {
            var methods = testFixtureClassType.GetMethods();
            return methods.Where(HasTestAttribute)
                .Select(mi => new TestMethod(mi))
                .Cast<ITestMethod>()
                .ToArray();
        }

        private static bool HasTestAttribute(MethodInfo methodInfo)
        {
            var testAttribute = methodInfo.CustomAttributes
                                 .FirstOrDefault(data => data.AttributeType.Name == "TestAttribute" || data.AttributeType.Name == "TestCaseAttribute" );
            var ignoreAttribute = methodInfo.CustomAttributes
                        .FirstOrDefault(data => data.AttributeType.Name == "IgnoreAttribute");
            var isTest = testAttribute != null && ignoreAttribute == null;
            return isTest;
        }

    }
}