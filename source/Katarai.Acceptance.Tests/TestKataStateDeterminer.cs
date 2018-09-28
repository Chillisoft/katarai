using System;
using System.Collections.Generic;
using Engine;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.Acceptance.Tests
{
    [TestFixture]
    public class TestKataStateDeterminer
    {
        [Test]
        public void Construct_GivenNullPlayerTestType_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel1);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var goldenImplementationTypes = new List<Type> { typeof(Golden_StringCalculator_AtLevel1) };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var argumentNullException = Assert.Throws<ArgumentNullException>(() =>
            {
                new KataStateDeterminer<IStringCalculator>(null, playerImplementationType, goldenTestType, goldenImplementationTypes);
            });
            //---------------Test Result -----------------------
            Assert.That(argumentNullException.ParamName, Is.EqualTo("playerTestType"));
        }

        [Test]
        public void Construct_GivenNullPlayerImplementationType_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var playerTestType = typeof(Player_TestStringCalculator_AtLevel1);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var goldenImplementationTypes = new List<Type> { typeof(Golden_StringCalculator_AtLevel1) };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new KataStateDeterminer<IStringCalculator>(playerTestType, null, goldenTestType, goldenImplementationTypes);
            });
            //---------------Test Result -----------------------
            Assert.That(exception.ParamName, Is.EqualTo("playerImplementationType"));
        }

        [Test]
        public void Construct_GivenNullGoldenTestType_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var playerTestType = typeof(Player_TestStringCalculator_AtLevel1);
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel1);
            var goldenImplementationTypes = new List<Type> { typeof(Golden_StringCalculator_AtLevel1) };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new KataStateDeterminer<IStringCalculator>(playerTestType, playerImplementationType, null, goldenImplementationTypes);
            });
            //---------------Test Result -----------------------
            Assert.That(exception.ParamName, Is.EqualTo("goldenTestType"));
        }

        [Test]
        public void Construct_GivenNullGoldenImplementationTypesList_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var playerTestType = typeof(Player_TestStringCalculator_AtLevel1);
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel1);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new KataStateDeterminer<IStringCalculator>(playerTestType, playerImplementationType, goldenTestType, null);
            });
            //---------------Test Result -----------------------
            Assert.That(exception.ParamName, Is.EqualTo("goldenImplementationTypes"));
        }

        [Test]
        public void Construct_GivenEmptyGoldenImplementationTypes_ShouldThrowAE()
        {
            //---------------Set up test pack-------------------
            var playerTestType = typeof(Player_TestStringCalculator_AtLevel1);
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel1);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var goldenImplementationTypes = new List<Type>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                new KataStateDeterminer<IStringCalculator>(playerTestType, playerImplementationType, goldenTestType, goldenImplementationTypes);
            });
            //---------------Test Result -----------------------
            Assert.That(exception.ParamName, Is.EqualTo("goldenImplementationTypes"));
        }

        [Test]
        public void Construct_GivenAllParameters_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            var playerTestType = typeof(Player_TestStringCalculator_AtLevel1);
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel1);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var goldenImplementationTypes = new List<Type> { typeof(Golden_StringCalculator_AtLevel1) };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() =>
            {
                new KataStateDeterminer<IStringCalculator>(playerTestType, playerImplementationType, goldenTestType, goldenImplementationTypes);
            });
            //---------------Test Result -----------------------
        }
    }
}