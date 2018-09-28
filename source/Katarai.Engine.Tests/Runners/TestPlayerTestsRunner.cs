using System;
using Engine;
using Engine.Annotations;
using Engine.Runners;
using Katarai.Acceptance.Tests;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.Engine.Tests.Runners
{
    [TestFixture]
    public class TestPlayerTestsRunner
    {
        [Test]
        public void Construct_WhenIncomingTypeIsNotATestPack_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentException>(() => new PlayerTestsRunner<IStringCalculator>(typeof(object), new Type[] {}));

            //---------------Test Result -----------------------
            Assert.AreEqual("playerTestFixtureType", ex.ParamName);
        }

        [Test]
        public void Construct_WhenPlayerTestFixtureTypeIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentException>(() => new PlayerTestsRunner<IStringCalculator>(null, new Type[] {}));

            //---------------Test Result -----------------------
            StringAssert.Contains("playerTestFixtureType cannot be null", ex.Message);
            Assert.AreEqual("playerTestFixtureType", ex.ParamName);
        }

        [Test]
        public void Construct_WhenNoGoldenImplementationTypes_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentException>(() => new PlayerTestsRunner<IStringCalculator>(typeof(PlayerTestStringCalculator), new Type[] { }));

            //---------------Test Result -----------------------
            StringAssert.Contains("No implementationTypes specified", ex.Message);
            Assert.AreEqual("goldenImplementationTypes", ex.ParamName);
        }

        [Test]
        public void Run_GivenTestMethodWithTestCaseAttribute_ShouldSetHasTestCaseAttributeToTrue()
        {
            //---------------Set up test pack-------------------
            var playerTestsRunner = new PlayerTestsRunner<IStringCalculator>(typeof(PlayerTestStringCalculator),  
                new[] {
                    typeof(Player_StringCalculator_AtLevel0),
                    typeof(Player_StringCalculator_AtLevel1)
                });
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var playerTestsRunResult = playerTestsRunner.Run();
            //---------------Test Result -----------------------
            Assert.IsTrue(playerTestsRunResult.HasTestCaseAttribute);
        }
        
        [Test]
        public void Run_GivenTestMethodWithExpectedExceptionAttribute_ShouldSetHasExpectedExceptionAttributeToTrue()
        {
            //---------------Set up test pack-------------------
            var playerTestsRunner = new PlayerTestsRunner<IStringCalculator>(typeof(PlayerTestStringCalculator),  
                new[] {
                    typeof(Player_StringCalculator_AtLevel0),
                    typeof(Player_StringCalculator_AtLevel1)
                });
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var playerTestsRunResult = playerTestsRunner.Run();
            //---------------Test Result -----------------------
            Assert.IsTrue(playerTestsRunResult.HasExpectedExceptionAttribute);
        }

        public class PlayerStringCalculator : IStringCalculator
        {
            public int Add(string input)
            {
                throw new NotImplementedException("shouldn't be calling this anyway");
            }
        }

        [Ignore("Part of the test pack tests")]
        public class PlayerTestStringCalculator : ITestPack<IStringCalculator>
        {
            public Func<IStringCalculator> CreateSUT { get; set; }

            public PlayerTestStringCalculator()
            {
                CreateSUT = () => new PlayerStringCalculator();
            }



            [TestCase]
            public void Add_GivenSingleNumber_ShouldReturnNumber()
            {
                //---------------Set up test pack-------------------

                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------

                //---------------Test Result -----------------------
                Assert.AreEqual(0, 0);
            }

            [TestCase]
            [ExpectedException]
            public void Add_ThrowsException()
            {
                //---------------Set up test pack-------------------

                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------

                //---------------Test Result -----------------------
                Assert.AreEqual(0, 0);
            }

            [Test]
            [Ignore(
                "WIP: once the player's test level can be determined, the simple console app can also start working out the current player phase (R/G) and how far she is ahead/behind"
                )]
            public void ShouldFindLevel()
            {
                //---------------Set up test pack-------------------
                var determiner = new PlayerTestsRunner<IStringCalculator>(typeof (PlayerTestStringCalculator),
                    new[]
                    {
                        typeof (Player_StringCalculator_AtLevel0),
                        typeof (Player_StringCalculator_AtLevel1)
                    });
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------

                //---------------Test Result -----------------------
                Assert.Fail("Test Not Yet Implemented");
            }
        }
    }
}
