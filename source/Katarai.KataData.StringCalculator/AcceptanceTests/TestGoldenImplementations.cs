using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Engine;
using Engine.Annotations;
using Engine.Runners;
using Katarai.KataData.StringCalculator.Implementations;
using Katarai.KataData.StringCalculator.Tests;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.KataData.StringCalculator.AcceptanceTests
{
    [TestFixture]
    public class TestGoldenImplementations
    {
        private static int GetImplementationLevel<TImplementationType>()
        {
            return GetImplementationLevel(typeof(TImplementationType));
        }

        private static int GetImplementationLevel(Type playerImplementationType)
        {
            var result = GetImplementationRunResult(playerImplementationType);
            var level = result.Level;
            return level;
        }

        private static PlayerImplementationRunResult GetImplementationRunResult<TImplementationType>()
        {
            return GetImplementationRunResult(typeof(TImplementationType));
        }

        private static PlayerImplementationRunResult GetImplementationRunResult(Type playerImplementationType)
        {
            var goldenTestType = typeof (TestStringCalculator);
            var playerImplementationRunner = new PlayerImplementationRunner<IStringCalculator>(playerImplementationType,
                goldenTestType);
            var result = playerImplementationRunner.Run();
            return result;
        }

        [Test]
        public void Run_GivenImplementationAtLevel1_ShouldSetLevelTo1()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_001>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(1));
        }

        [Test]
        public void Run_GivenImplementationAtLevel2_ShouldSetLevelTo2()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_002>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(2));
        }

        [Test]
        public void Run_GivenImplementationAtLevel3_ShouldSetLevelTo3()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_003>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(3));
        }

        [Test]
        public void Run_GivenImplementationAtLevel4_ShouldSetLevelTo4()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_004>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(4));
        }

        [Test]
        public void Run_GivenImplementationAtLevel5_ShouldSetLevelTo5()
        {
            //------------
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_005>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(5));
        }

        [Test]
        public void Run_GivenImplementationAtLevel6_ShouldSetLevelTo6()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_006>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(6));
        }

        [Test]
        public void Run_GivenImplementationAtLevel7_ShouldSetLevelTo7()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_007>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(7));
        }

        [Test]
        public void Run_GivenImplementationAtLevel8_ShouldSetLevelTo8()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_008>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(8));
        }

        [Test]
        public void Run_GivenImplementationAtLevel9_ShouldSetLevelTo9()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_009>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(9));
        }

        [Test]
        public void Run_GivenImplementationAtLevel9_WithEdgeCaseFailure1_ShouldSetLevelTo9()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_009_1>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(9));
        }

        [Test]
        public void Run_GivenImplementationAtLevel9_WithEdgeCaseFailure2_ShouldSetLevelTo9()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_009_2>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(9));
        }

        [Test]
        public void Run_GivenImplementationAtLevel10_ShouldSetLevelTo10()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_010>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(10));
        }

        [Test]
        public void Run_GivenImplementationAtLevel11_ShouldSetLevelTo11()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = GetImplementationLevel<StringCalculator_AtLevel_011>();
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(11));
        }


        [Test]
        public void DataTestFixture_ShouldHaveCorrespondingDataImplementationsForAllTestLevels()
        {
            //---------------Set up test pack-------------------
            var testFixture = new TestStringCalculator();
            var methodsInfos = GetMethodsInfos(testFixture);
            var implementations = GetDataImplementations();
            Func<int, IStringCalculator> GetInstanceOfImplementationAtLevel = (level) =>
            {
                var matchingType = implementations.FirstOrDefault(i => i.Level == level);
                if (matchingType == null)
                    return null;
                return (IStringCalculator) Activator.CreateInstance(matchingType.Type);
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var dontHaveImplementationsFor = methodsInfos.Aggregate(new List<int>(), (implementationLevelsNotFoundFor, method) =>
            {
                var instanceOfImplementationAtLevel = GetInstanceOfImplementationAtLevel(method.Level);
                if (instanceOfImplementationAtLevel == null)
                {
                    implementationLevelsNotFoundFor.Add(method.Level);
                }
                return implementationLevelsNotFoundFor;
            });

            //---------------Test Result -----------------------
            var distinct = dontHaveImplementationsFor.Distinct().ToArray();
            if (distinct.Any())
                Assert.Fail("The following levels have no implementations: " + string.Join(",", distinct));
        }

        [Test]
        public void ExistingDataImplementations_ShouldAllPassCorrespondingTestLevelInDataTests()
        {
            //---------------Set up test pack-------------------
            var testFixture = new TestStringCalculator();
            var methodsInfos = GetMethodsInfos(testFixture);
            var implementations = GetDataImplementations();
            Func<int, IStringCalculator> GetInstanceOfImplementationAtLevel = (level) =>
            {
                var matchingType = implementations.FirstOrDefault(i => i.Level == level);
                if (matchingType == null)
                    return null;
                return (IStringCalculator) Activator.CreateInstance(matchingType.Type);
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var failed = methodsInfos.Aggregate(new List<int>(), (f, method) =>
            {
                var instanceOfImplementationAtLevel = GetInstanceOfImplementationAtLevel(method.Level);
                if (instanceOfImplementationAtLevel == null)
                    return f;
                try
                {
                    testFixture.CreateSUT = () => instanceOfImplementationAtLevel;
                    method.MethodInfo.Invoke(testFixture, null);
                }
                catch (Exception)
                {
                    f.Add(method.Level);
                }
                return f;
            });

            //---------------Test Result -----------------------
            var distinct = failed.Distinct().ToArray();
            if (distinct.Any())
                Assert.Fail("The following levels have crap implementations: " + string.Join(",", distinct));
        }

        private static IEnumerable<TypeWithLevel> GetDataImplementations()
        {
            var implementationTypes = new[]
            {
                typeof (StringCalculator_AtLevel_001),
                typeof (StringCalculator_AtLevel_002),
                typeof (StringCalculator_AtLevel_003),
                typeof (StringCalculator_AtLevel_004),
                typeof (StringCalculator_AtLevel_005),
                typeof (StringCalculator_AtLevel_006),
                typeof (StringCalculator_AtLevel_007),
                typeof (StringCalculator_AtLevel_008),
                typeof (StringCalculator_AtLevel_009),
                typeof (StringCalculator_AtLevel_010),
                typeof (StringCalculator_AtLevel_011)
            };
            var implementations = implementationTypes.Select(t =>
            {
                return new TypeWithLevel(t, t.GetCustomAttributes().OfType<TestStepAttribute>().FirstOrDefault().StepNumber);
            });
            return implementations;
        }

        private class MethodInfoWithLevel
        {
            public MethodInfo MethodInfo { get; protected set; }
            public int Level { get; protected set; }

            public MethodInfoWithLevel(MethodInfo mi, int level)
            {
                MethodInfo = mi;
                Level = level;
            }
        }

        private class TypeWithLevel
        {
            public Type Type { get; protected set; }
            public int Level { get; protected set; }

            public TypeWithLevel(Type type, int level)
            {
                Type = type;
                Level = level;
            }
        }

        private static IEnumerable<MethodInfoWithLevel> GetMethodsInfos(TestStringCalculator testFixture)
        {
            var methodsInfos = testFixture.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Select(mi =>
                {
                    var testStepAttribute = mi.GetCustomAttributes().OfType<TestStepAttribute>().ToArray();
                    if (!testStepAttribute.Any())
                        return null;
                    var level = testStepAttribute.First().StepNumber;
                    return new MethodInfoWithLevel(mi, level);
                });
            return methodsInfos.Where(mi => mi != null);
        }
    }
}