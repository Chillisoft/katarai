using System;
using Engine;
using Engine.Runners;
using Katarai.Acceptance.Tests;
using Katarai.StringCalculator.Golden;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.Engine.Tests.Runners
{
    [TestFixture]
    public class TestPlayerTestsPlayerImplementationRunner
    {
        [Test]
        public void Construct_WhenPlayerTestFixtureIsNotATestPack_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentException>(() => new PlayerTestsPlayerImplementationRunner<IStringCalculator>(typeof(object), GetType()));

            //---------------Test Result -----------------------
            Assert.AreEqual("playerTestFixtureType", ex.ParamName);
        }

        [Test]
        public void Construct_GivenNullPlayerImplementationType_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var playerTestFixtureType = typeof(PlayerTestStringCalculator);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new PlayerTestsPlayerImplementationRunner<IStringCalculator>(playerTestFixtureType, null);
            });
            //---------------Test Result -----------------------
            Assert.That(exception.ParamName, Is.EqualTo("playerImplementationType"));
        }

        [Test]
        public void Construct_GivenAllParameters_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel0);
            var playerTestFixtureType = typeof(PlayerTestStringCalculator);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() =>
            {
                new PlayerTestsPlayerImplementationRunner<IStringCalculator>(playerTestFixtureType, playerImplementationType);
            });
            //---------------Test Result -----------------------
        }

        [Test]
        public void Run_GivenOneTestResultThatPasses_ShouldReturnEqualTrue()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(StringCalculator001);
            var playerTestFixtureType = typeof(TestStringCalculator001);
            var runner = CreatePlayerTestPlayerImplementationRunner(playerTestFixtureType, playerImplementationType);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = runner.Run();
            //---------------Test Result -----------------------
            Assert.IsTrue(result.Passed);
        }

        [Test]
        public void Run_GivenTestResultsWithOneTestResultThatPasses_ShouldReturnPassedEqualFalse()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(StringCalculator002);
            var playerTestFixtureType = typeof(TestStringCalculator002);
            var runner = CreatePlayerTestPlayerImplementationRunner(playerTestFixtureType, playerImplementationType);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = runner.Run();
            //---------------Test Result -----------------------
            Assert.IsFalse(result.Passed);
        }

        private static PlayerTestsPlayerImplementationRunner<IStringCalculator> CreatePlayerTestPlayerImplementationRunner(
            Type playerTestFixtureType, Type playerImplementationType)
        {
            var runner = new PlayerTestsPlayerImplementationRunner<IStringCalculator>(playerTestFixtureType,
                playerImplementationType);
            return runner;
        }

        public class PlayerTestStringCalculator : TestStringCalculator
        {
        }
    }

    [Ignore("Because nunit 3.x needs this populated")]
    public class TestStringCalculator001 : ITestPack<IStringCalculator>
    {
        public Func<IStringCalculator> CreateSUT { get; set; }

        [Test]
        public void Add_GivenEmptyString_ShouldReturnZero()
        {
            //---------------Set up test pack-------------------
            var calculator = new StringCalculator001();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = calculator.Add("");
            //---------------Test Result -----------------------
            Assert.AreEqual(0, result);
        }

    }

    public class StringCalculator001 : IStringCalculator
    {
        public int Add(string input)
        {
            return 0;
        }
    }

    [Ignore("Because nunit 3.x needs this populated")]
    public class TestStringCalculator002 : ITestPack<IStringCalculator>
    {
        public Func<IStringCalculator> CreateSUT { get; set; }

        [Test]
        public void Add_GivenEmptyString_ShouldReturnZero()
        {
            //---------------Set up test pack-------------------
            var calculator = new StringCalculator002();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = calculator.Add("");
            //---------------Test Result -----------------------
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Add_GivenOneNumber_ShouldReturnNumber()
        {
            //---------------Set up test pack-------------------
            var calculator = new StringCalculator002();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = calculator.Add("1");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, result);
        }
    }

    public class StringCalculator002 : IStringCalculator
    {
        public int Add(string input)
        {
            return 0;
        }
    }
}