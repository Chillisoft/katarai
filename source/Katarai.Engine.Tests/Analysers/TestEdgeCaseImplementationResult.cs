using Engine.Analysers;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace Katarai.Engine.Tests.Analysers
{
    [TestFixture]
    public class TestEdgeCaseImplementationResult
    {
        [Test]
        public void Level_WhenSet_ShouldReturnSameValue()
        {
            //---------------Set up test pack-------------------
            var edgeCaseImplementationResult = CreateEdgeCaseImplementationResult();
            var expected = RandomValueGen.GetRandomInt(0, 50);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            edgeCaseImplementationResult.Level = expected;
            //---------------Test Result -----------------------
            Assert.That(edgeCaseImplementationResult.Level, Is.EqualTo(expected));
        }
        
        [Test]
        public void EdgeCaseMessage_WhenSet_ShouldReturnSameValue()
        {
            //---------------Set up test pack-------------------
            var edgeCaseImplementationResult = CreateEdgeCaseImplementationResult();
            var expected = RandomValueGen.GetRandomString(10, 30);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            edgeCaseImplementationResult.EdgeCaseMessage = expected;
            //---------------Test Result -----------------------
            Assert.That(edgeCaseImplementationResult.EdgeCaseMessage, Is.EqualTo(expected));
        }

        [Test]
        public void IsImplemented_WhenSet_ShouldReturnSameValue()
        {
            //---------------Set up test pack-------------------
            var edgeCaseImplementationResult = CreateEdgeCaseImplementationResult();
            var expected = RandomValueGen.GetRandomBoolean();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            edgeCaseImplementationResult.IsImplemented = expected;
            //---------------Test Result -----------------------
            Assert.That(edgeCaseImplementationResult.IsImplemented, Is.EqualTo(expected));
        }
        
        private static EdgeCaseImplementationResult CreateEdgeCaseImplementationResult()
        {
            return new EdgeCaseImplementationResult();
        }
    }
}