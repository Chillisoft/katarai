using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Engine.Runners
{
    public interface ITestResult
    {
        ITestMethod TestMethod { get; }
        bool Passed { get; }
    }

    public class TestResult : ITestResult
    {
        public ITestMethod TestMethod { get; set; }
        public bool Passed { get; set; }

        public override string ToString()
        {
            //TODO mark 02 Feb 2015: Test This
            var passedToString = Passed ? "Passed" : "Failed";
            return string.Format("{0} - {1}", passedToString, TestMethod);
        }
    }

    public interface ITestFixtureRunner
    {
        ITestResult[] RunTestFixture(IEnumerable<ITestMethod> testMethods);
    }

    public class TestFixtureRunner<TKata> : ITestFixtureRunner where TKata : class
    {
        public TestFixtureRunner(Type playerImplementationType)
        {
            PlayerImplementationType = playerImplementationType;
        }

        public Type PlayerImplementationType { get; set; }

        public ITestResult[] RunTestFixture(IEnumerable<ITestMethod> testMethods)
        {
            return testMethods
                .Select(GetTestResult)
                .ToArray();
        }

        private ITestResult GetTestResult(ITestMethod testMethod)
        {
            var testPasses = TestPasses(testMethod, CreatePlayerImplementationInstance);
            return new TestResult
            {
                TestMethod = testMethod, 
                Passed = testPasses
            };
        }

        private bool TestPasses(ITestMethod testMethod, Func<TKata> createSUT)
        {
            try
            {
                var method = testMethod.Method;
                var testFixture = CreateTestFixture(method, createSUT);
                InvokeTestMethod(testFixture, method);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private static void InvokeTestMethod(ITestPack<TKata> testFixture, MethodInfo testMethod)
        {
            var parameterInfos = testMethod.GetParameters();
            var parameterValues = parameterInfos.Select(info => info.DefaultValue).ToArray();
            testMethod.Invoke(testFixture, parameterValues);
        }

        private static ITestPack<TKata> CreateTestFixture(MethodInfo method, Func<TKata> createSUT)
        {
            var testFixture = Activator.CreateInstance(method.DeclaringType) as ITestPack<TKata>;
            testFixture.CreateSUT = createSUT;
            return testFixture;
        }

        private TKata CreatePlayerImplementationInstance()
        {
            return (TKata) Activator.CreateInstance(this.PlayerImplementationType);
        }
    }
}