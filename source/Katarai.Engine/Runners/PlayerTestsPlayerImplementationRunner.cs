using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Runners
{
    public class PlayerTestsPlayerImplementationRunResult
    {
        public bool Passed { get; set; }

        private string TestState
        {
            get
            {
                return Passed ? "Green" : "Red";
            }
        }
        public override string ToString()
        {
            return string.Format("Player Test State: {0}", TestState);
        }
    }

    public interface IPlayerTestsPlayerImplementationRunner
    {
        PlayerTestsPlayerImplementationRunResult Run();
    }

    public class PlayerTestsPlayerImplementationRunner<TKata> : IPlayerTestsPlayerImplementationRunner
        where TKata : class
    {
        private readonly Type _playerTestFixtureType;
        private readonly ITestMethodsRetriever _testMethodsRetriever;
        private readonly ITestFixtureRunner _testFixtureRunner;


        public PlayerTestsPlayerImplementationRunner(Type playerTestFixtureType, Type playerImplementationType)
        {
            CheckTestFixtureType(playerTestFixtureType);
            _playerTestFixtureType = playerTestFixtureType;
            if (playerImplementationType == null) throw new ArgumentNullException("playerImplementationType");
            _testMethodsRetriever = new TestMethodsRetriever();
            _testFixtureRunner = new TestFixtureRunner<TKata>(playerImplementationType);
        }

        public PlayerTestsPlayerImplementationRunResult Run()
        {
            var testMethods = (IEnumerable<ITestMethod>) _testMethodsRetriever.GetTestMethods(_playerTestFixtureType);
            var testResults = _testFixtureRunner.RunTestFixture(testMethods);
            var passed = testResults.All(testResult => testResult.Passed);

            return new PlayerTestsPlayerImplementationRunResult
            {
                Passed = passed
            };
        }

        private void CheckTestFixtureType(Type playerTestFixtureType)
        {
            if (playerTestFixtureType == null)
                throw new ArgumentException("playerTestFixtureType cannot be null", "playerTestFixtureType");
            var shouldImplement = typeof (ITestPack<TKata>);
            if (!shouldImplement.IsAssignableFrom(playerTestFixtureType))
                throw new ArgumentException(
                    "playerTestFixtureType should implement ITestPack<" + typeof (TKata).Name + ">",
                    "playerTestFixtureType");
        }
    }
}