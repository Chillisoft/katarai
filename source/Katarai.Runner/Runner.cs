using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Engine;
using Engine.Annotations;
using Engine.Runners;
using NUnit.Framework;

namespace Katarai.Runner
{
    public class Runner
    {
        private readonly string _pathToKataDataFile;
        private readonly string _pathToPlayerImplementationFile;
        private readonly string _pathToPlayerTestFile;

        public Runner(string pathToKataDataFile, string pathToPlayerImplementationFile, string pathToPlayerTestFile)
        {
            _pathToKataDataFile = GetAbsolutePath(pathToKataDataFile);
            _pathToPlayerImplementationFile = GetAbsolutePath(pathToPlayerImplementationFile);
            _pathToPlayerTestFile = GetAbsolutePath(pathToPlayerTestFile ?? pathToPlayerImplementationFile);
        }

        private static string GetAbsolutePath(string pathToKataDataFile)
        {
            var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), pathToKataDataFile);
            return absolutePath;
        }

        

        public Result Run()
        {
            var kataDataAssembly = LoadAssemblyAt(_pathToKataDataFile);
            var playerImplementationAssembly = LoadAssemblyAt(_pathToPlayerImplementationFile);
            var playerTestsAssembly = LoadAssemblyAt(_pathToPlayerTestFile);

            var kataConfigType = GetKataConfigTypeFrom(kataDataAssembly);
            var config = Activator.CreateInstance(kataConfigType);
            var kataInterfaceType = GetKataInterfaceTypeFrom(config);
            var kataTestsType = GetKataTestsTypeFrom(config);
            var kataImplementations = GetKataImplementationsFrom(config);
            var playerImplementationType = GetPlayerImplementationTypeFrom(playerImplementationAssembly,
                kataInterfaceType);

            var playerTestFixtureType = GetPlayerTestsTypeFrom(playerTestsAssembly, kataInterfaceType);

            return GetResult(kataInterfaceType, kataTestsType, playerImplementationType, playerTestFixtureType, kataImplementations);
        }

        internal Result GetResult(Type kataInterfaceType, Type kataTestsType, Type playerImplementationType,
            Type playerTestFixtureType, Type[] kataImplementations)
        {
            var playerImplementationRunResult = RunPlayerImplementationsFor(kataInterfaceType, kataTestsType,
                playerImplementationType);
            var playerTestsRunResult = RunPlayerTestsFor(kataInterfaceType, playerTestFixtureType, kataImplementations,
                playerImplementationRunResult.Level);
            var playerTestsPlayerImplementationRunResult = RunPlayerTestsPlayerImplementationsFor(kataInterfaceType,
                playerTestFixtureType, playerImplementationType);

            var overallAnalysisResult = GetOverallAnalysisResult(playerImplementationRunResult, playerTestsRunResult,
                playerTestsPlayerImplementationRunResult, kataTestsType);
            var playerFeedback = DeterminePlayerFeedback(overallAnalysisResult);
            return new Result(playerImplementationRunResult, playerTestsRunResult, playerFeedback);
        }

        private PlayerFeedback DeterminePlayerFeedback(OverallAnalysisResult overallAnalysisResult)
        {
            var progressReporter = new FeedbackGenerator();
            return progressReporter.GeneratePlayerFeedback(overallAnalysisResult);
        }

        private static OverallAnalysisResult GetOverallAnalysisResult(
            PlayerImplementationRunResult playerImplementationRunResult, PlayerTestsRunResult playerTestsRunResult,
            PlayerTestsPlayerImplementationRunResult playerTestsPlayerImplementationRunResult, Type kataTestsType)
        {
            var testMethodsRetriever = new TestMethodsRetriever();
            var goldenTestMethods = testMethodsRetriever.GetTestMethods(kataTestsType);

            var overallAnalysisResult = new OverallAnalysisResult(
                playerImplementationRunResult,
                playerTestsRunResult,
                playerTestsPlayerImplementationRunResult,
                goldenTestMethods);
            return overallAnalysisResult;
        }

        private TestMethodMeta GetTestMethodMeta(ITestMethod testMethod)
        {
            if (testMethod == null) return new TestMethodMeta();
            var testStepShouldDo = testMethod.Method.GetCustomAttributes<StepShouldDoAttribute>();
            var testEdgeCaseHint = testMethod.Method.GetCustomAttributes<EdgeCaseHintAttribute>();
            var testSuggestedTestName = testMethod.Method.GetCustomAttributes<SuggestedTestNameAttribute>();
            var stepShouldDoAttribute = testStepShouldDo.FirstOrDefault();
            var stepShouldDoText = string.Empty;
            var edgeCaseHintText = string.Empty;
            var suggestedTestName = string.Empty;

            if (stepShouldDoAttribute != null)
            {
                stepShouldDoText = stepShouldDoAttribute.ShouldDoText;
            }

            var edgeCaseHintAttribute = testEdgeCaseHint.FirstOrDefault();
            if (edgeCaseHintAttribute != null)
            {
                edgeCaseHintText = edgeCaseHintAttribute.EdgeCaseHint;
            }

            var suggestedTestNameAttribute = testSuggestedTestName.FirstOrDefault();
            if (suggestedTestNameAttribute != null)
            {
                suggestedTestName = suggestedTestNameAttribute.SuggestedTestName;
            }

            return new TestMethodMeta
            {
                StepShoudlDo = stepShouldDoText,
                EdgeCaseHint = edgeCaseHintText,
                SuggestedTestName = suggestedTestName
            };
        }

        private bool HasTestAttribute(MethodInfo mi)
        {
            return mi.GetCustomAttributes<TestAttribute>().Any();
        }

        private static PlayerTestsRunResult RunPlayerTestsFor(Type kataInterfaceType, Type playerTestFixtureType, Type[] kataImplementations, int implementationLevel)
        {
            var instance = CreateGenericInstance(typeof (PlayerTestsRunner<>), new[] {kataInterfaceType},
                new object[]
                {
                    playerTestFixtureType,
                    kataImplementations,
                    null,
                    implementationLevel
                });
            var playerTestsRunner = instance as IPlayerTestsRunner;
            if (playerTestsRunner == null) throw new Exception("Cannot get an IPlayerTestFixtureLevelDeterminer");
            return playerTestsRunner.Run();
        }


        private PlayerImplementationRunResult RunPlayerImplementationsFor(Type kataInterfaceType, Type testFixtureType,
            Type implementationType)
        {
            var instance = CreateGenericInstance(typeof (PlayerImplementationRunner<>),
                new[] {kataInterfaceType}, new[] {implementationType, testFixtureType});
            var playerImplementationRunner = instance as IPlayerImplementationRunner;
            if (playerImplementationRunner == null)
                throw new Exception("Cannot get an IPlayerImplementationLevelDeterminer ):");
            return playerImplementationRunner.Run();
        }

        private PlayerTestsPlayerImplementationRunResult RunPlayerTestsPlayerImplementationsFor(Type kataInterfaceType,
            Type playerTestFixtureType, Type playerImplementationType)
        {
            var instance = CreateGenericInstance(typeof (PlayerTestsPlayerImplementationRunner<>),
                new[] {kataInterfaceType}, new[] {playerTestFixtureType, playerImplementationType});
            var playerTestsPlayerImplementationRunner = instance as IPlayerTestsPlayerImplementationRunner;
            if (playerTestsPlayerImplementationRunner == null)
                throw new Exception("Cannot get an IPlayerTestsPlayerImplementationRunner ):");
            return playerTestsPlayerImplementationRunner.Run();
        }

        private static object CreateGenericInstance(Type baseType, Type[] genericArgumentTypes,
            object[] constructorParameters = null)
        {
            var genericType = baseType.MakeGenericType(genericArgumentTypes);
            return Activator.CreateInstance(genericType, constructorParameters);
        }

        private static object BuildCreatorFuncFor(Type kataInterfaceType, Type playerImplementationType)
        {
            var instance = CreateGenericInstance(typeof (Runner.GenericFactory<>), new[] {kataInterfaceType});
            var creatorFunc = instance.GetType()
                .GetMethod("BuildCreatorFunc")
                .Invoke(instance, new[] {playerImplementationType});
            return creatorFunc;
        }

        public class GenericFactory<T>
        {
            // ReSharper disable UnusedMember.Global
            public Func<T> BuildCreatorFunc(Type type)
            {
                return () => (T) Activator.CreateInstance(type);
            }

            // ReSharper restore UnusedMember.Global
        }


        private Type GetPlayerImplementationTypeFrom(Assembly playerImplementationAssembly, Type kataInterfaceType)
        {
            try
            {
                var result = playerImplementationAssembly.GetTypes().FirstOrDefault(kataInterfaceType.IsAssignableFrom);
                if (result == null)
                    throw new Exception("Unable to find player implementationType for '" +
                                        kataInterfaceType.Name + "' in '" + playerImplementationAssembly.FullName + "'");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Type GetPlayerTestsTypeFrom(Assembly playerTestsAssembly, Type testsShouldBeFor)
        {
            try
            {
                var testPackInterfaceType = typeof (ITestPack<>).MakeGenericType(testsShouldBeFor);
                var result = playerTestsAssembly.GetTypes().FirstOrDefault(testPackInterfaceType.IsAssignableFrom);
                if (result == null)
                    throw new Exception("Unable to find player tests fixture for '" +
                                        testsShouldBeFor.Name + "' in '" + playerTestsAssembly.FullName + "'");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Type[] GetKataImplementationsFrom(object config)
        {
            var methodName = "GetKataImplementationTypes";
            var result = InvokeConfig(config, methodName, _pathToKataDataFile);
            var asEnumerable = result as IEnumerable<Type>;
            if (asEnumerable == null)
                throw new Exception("Result from '" +
                                    methodName + "' should have returned an IEnumerable<Type> but didn't ):");
            return asEnumerable.ToArray();
        }

        private Type GetKataConfigTypeFrom(Assembly kataDataAssembly)
        {
            const string kataconfiguration = "Configuration";
            Type[] assemblyTypes = kataDataAssembly.GetTypes();
            var kataConfigType = assemblyTypes.FirstOrDefault(t => t.Name == kataconfiguration);
            if (kataConfigType == null)
                throw new Exception("Unable to find '" + kataconfiguration + "' type in Kata Data assembly at '" +
                                    _pathToKataDataFile + "'");
            return kataConfigType;
        }

        private Type GetKataInterfaceTypeFrom(object config)
        {
            return InvokeConfig(config, "GetKataInterfaceType", _pathToKataDataFile) as Type;
        }

        private Type GetKataTestsTypeFrom(object config)
        {
            return InvokeConfig(config, "GetKataTestsType", _pathToKataDataFile) as Type;
        }

        private static object InvokeConfig(object config, string methodName, string assemblyPath)
        {
            var kataConfigType = config.GetType();
            var method = kataConfigType.GetMethod(methodName);
            if (method == null)
                throw new Exception("KataConfiguration in '" + assemblyPath + "' does not have a " + methodName +
                                    "() method. Aborting.");

            var result = method.Invoke(config, null);
            return result;
        }

        private Assembly LoadAssemblyAt(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Cannot find Kata Data file at '" + path + "'", path);
            return Assembly.LoadFrom(path);
        }
    }
}