using System;
using Engine;
using Engine.Runners;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.Acceptance.Tests
{
    [TestFixture]
    public class TestPlayerImplementationRunner
    {
        [Test]
        public void Construct_GivenNullPlayerImplementationType_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var goldenTestType = typeof(Golden_TestStringCalculator);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new PlayerImplementationRunner<IStringCalculator>(null, goldenTestType);
            });
            //---------------Test Result -----------------------
            Assert.That(exception.ParamName, Is.EqualTo("playerImplementationType"));
        }

        [Test]
        public void Construct_GivenNullGoldenTestType_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel1);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new PlayerImplementationRunner<IStringCalculator>(playerImplementationType, null);
            });
            //---------------Test Result -----------------------
            Assert.That(exception.ParamName, Is.EqualTo("goldenTestType"));
        }

        [Test]
        public void Construct_GivenAllParameters_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel1);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() =>
            {
                new PlayerImplementationRunner<IStringCalculator>(playerImplementationType, goldenTestType);
            });
            //---------------Test Result -----------------------
        }

        [Test]
        public void Run_GivenImplementationAtLevel0_ShouldSetLevelTo0()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel0);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var playerImplementationRunner = new PlayerImplementationRunner<IStringCalculator>(playerImplementationType, goldenTestType);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = playerImplementationRunner.Run();
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(0));
        }

        [Test]
        public void Run_GivenImplementationAtLevel1_ShouldSetLevelTo1()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel1);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var playerImplementationRunner = new PlayerImplementationRunner<IStringCalculator>(playerImplementationType, goldenTestType);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = playerImplementationRunner.Run();
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(1));
        }

        [Test]
        public void Run_GivenImplementationAtLevel2_ShouldSetLevelTo2()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel2);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var playerImplementationRunner = new PlayerImplementationRunner<IStringCalculator>(playerImplementationType, goldenTestType);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = playerImplementationRunner.Run();
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(2));
        }

        [Test]
        public void Run_GivenImplementationAtLevel3_ShouldSetLevelTo3()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel3);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var playerImplementationRunner = new PlayerImplementationRunner<IStringCalculator>(playerImplementationType, goldenTestType);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = playerImplementationRunner.Run();
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(3));
        }

        [Test]
        public void Run_GivenImplementationAtLevel4_ShouldSetLevelTo4()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel4);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var playerImplementationRunner = new PlayerImplementationRunner<IStringCalculator>(playerImplementationType, goldenTestType);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = playerImplementationRunner.Run();
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(4));
        }

        [Test]
        public void Run_GivenImplementationAtLevel5_ShouldSetLevelTo5()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel5);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var playerImplementationRunner = new PlayerImplementationRunner<IStringCalculator>(playerImplementationType, goldenTestType);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = playerImplementationRunner.Run();
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(5));
        }

        [Test]
        public void Run_GivenImplementationAtLevel6_ShouldSetLevelTo6()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel6);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var playerImplementationRunner = new PlayerImplementationRunner<IStringCalculator>(playerImplementationType, goldenTestType);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = playerImplementationRunner.Run();
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(6));
        }

        [Test]
        public void Run_GivenImplementationAtLevel7_ShouldSetLevelTo7()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel7);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var playerImplementationRunner = new PlayerImplementationRunner<IStringCalculator>(playerImplementationType, goldenTestType);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = playerImplementationRunner.Run();
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(7));
        }

        [Test]
        public void Run_GivenImplementationAtLevel8_ShouldSetLevelTo8()
        {
            //---------------Set up test pack-------------------
            var playerImplementationType = typeof(Player_StringCalculator_AtLevel8);
            var goldenTestType = typeof(Golden_TestStringCalculator);
            var playerImplementationRunner = new PlayerImplementationRunner<IStringCalculator>(playerImplementationType, goldenTestType);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = playerImplementationRunner.Run();
            //---------------Test Result -----------------------
            Assert.That(result.Level, Is.EqualTo(8));
        }

        //[Test]
        //public void Run_GivenImplementationAtLevel9_ShouldSetLevelTo9()
        //{
        //    //---------------Set up test pack-------------------
        //    var playerImplementationType = typeof(Player_StringCalculator_AtLevel9);
        //    var goldenTestType = typeof(Golden_TestStringCalculator);
        //    var determiner = new PlayerImplementationLevelDeterminer<IStringCalculator>(playerImplementationType, goldenTestType);
        //    //---------------Assert Precondition----------------
        //    Assert.That(determiner.Level, Is.Not.EqualTo(0));
        //    //---------------Execute Test ----------------------
        //    determiner.Run();
        //    //---------------Test Result -----------------------
        //    Assert.That(determiner.Level, Is.EqualTo(9));
        //}

    }
}