using Engine;
using Engine.Analysers;
using Engine.Annotations;
using NUnit.Framework;

namespace Katarai.Engine.Tests.Analysers
{
    [TestFixture]
    public class TestPlayerTestsLevelResult
    {
        [Test]
        public void Level_WhenFirstGoldenImplementationPassingAllTestsIsNull_ShouldReturnMinusOne()
        {
            //---------------Set up test pack-------------------
            var playerTestsLevelResult = CreatePlayerTestsLevelResult();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = playerTestsLevelResult.Level;
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(-1));
        }

        [Test]
        public void Level_WhenHasFirstGoldenImplementationPassingAllTests_ShouldReturnImplemenationStepNumber()
        {
            //---------------Set up test pack-------------------
            var playerTestsLevelResult = CreatePlayerTestsLevelResult();
            playerTestsLevelResult.FirstGoldenImplementationPassingAllTests = new GoldenImplementation(new ClassWithAttributes().GetType());
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var level = playerTestsLevelResult.Level;
            //---------------Test Result -----------------------
            Assert.That(level, Is.EqualTo(2));
        }

        private PlayerTestsLevelResult CreatePlayerTestsLevelResult()
        {
            return new PlayerTestsLevelResult();
        }

        [TestStep(2)]
        private class ClassWithAttributes
        {

        }
    }
}