using System;
using System.Linq;

namespace Engine.Runners
{
    public class CustomTestMethodsRetriever : TestMethodsRetriever
    {
        private readonly Func<ITestMethod, bool> _filter;

        public CustomTestMethodsRetriever(Func<ITestMethod, bool> filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");
            _filter = filter;
        }

        public override ITestMethod[] GetTestMethods(Type testFixtureClassType)
        {
            return base.GetTestMethods(testFixtureClassType)
                .Where(_filter)
                .ToArray();
        }
    }
}