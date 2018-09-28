using System.Collections.Generic;
using Engine.Analysers;
using Engine.Runners;
using NUnit.Framework;

namespace Katarai.Engine.Tests.Runners
{
    [TestFixture]
    public class TestPlayerImplementationRunResult
    {
        [Test]
        public void TotalEdgeCases_GivenEdgeCaseImplementationResultsIsNull_ShouldReturnZero()
        {
            //---------------Set up test pack-------------------
            var playerImplementationRunResult = CreatePlayerImplementationRunResult();
            playerImplementationRunResult.EdgeCaseImplementationResults = null;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var totalEdgeCases = playerImplementationRunResult.TotalEdgeCases;
            //---------------Test Result -----------------------
            Assert.That(totalEdgeCases, Is.EqualTo(0));
        }

        [Test]
        public void TotalEdgeCases_GivenOneEdgeCaseImplementationResults_ShouldReturnOne()
        {
            //---------------Set up test pack-------------------
            var playerImplementationRunResult = CreatePlayerImplementationRunResult();
            playerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult> { new EdgeCaseImplementationResult() };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var totalEdgeCases = playerImplementationRunResult.TotalEdgeCases;
            //---------------Test Result -----------------------
            Assert.That(totalEdgeCases, Is.EqualTo(1));
        }

        [Test]
        public void TotalEdgeCases_GivenManyEdgeCaseImplementationResults_ShouldReturnCaseCount()
        {
            //---------------Set up test pack-------------------
            var playerImplementationRunResult = CreatePlayerImplementationRunResult();
            playerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult> { new EdgeCaseImplementationResult(), new EdgeCaseImplementationResult(), new EdgeCaseImplementationResult() };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var totalEdgeCases = playerImplementationRunResult.TotalEdgeCases;
            //---------------Test Result -----------------------
            Assert.That(totalEdgeCases, Is.EqualTo(3));
        }

        [Test]
        public void EdgeCasesImplemented_GivenEdgeCaseImplementationResultsIsNull_ShouldReturnZero()
        {
            //---------------Set up test pack-------------------
            var playerImplementationRunResult = CreatePlayerImplementationRunResult();
            playerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult> { new EdgeCaseImplementationResult() };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var edgeCasesImplemented = playerImplementationRunResult.EdgeCasesImplemented;
            //---------------Test Result -----------------------
            Assert.That(edgeCasesImplemented, Is.EqualTo(0));
        }

        [Test]
        public void EdgeCasesImplemented_GivenOneEdgeCaseImplementationResultsThatIsImplemented_ShouldReturnOne()
        {
            //---------------Set up test pack-------------------
            var playerImplementationRunResult = CreatePlayerImplementationRunResult();
            playerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult> { new EdgeCaseImplementationResult { IsImplemented = true } };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var edgeCasesImplemented = playerImplementationRunResult.EdgeCasesImplemented;
            //---------------Test Result -----------------------
            Assert.That(edgeCasesImplemented, Is.EqualTo(1));
        }

        [Test]
        public void EdgeCasesImplemented_GivenOneEdgeCaseImplementationResultsThatIsNotCovered_ShouldReturnZero()
        {
            //---------------Set up test pack-------------------
            var playerImplementationRunResult = CreatePlayerImplementationRunResult();
            playerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult> { new EdgeCaseImplementationResult { IsImplemented = false } };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var edgeCasesImplemented = playerImplementationRunResult.EdgeCasesImplemented;
            //---------------Test Result -----------------------
            Assert.That(edgeCasesImplemented, Is.EqualTo(0));
        }

        [Test]
        public void EdgeCasesImplemented_GivenTotalEdgeCasesEQ5AndThreeCovered_ShouldReturnThree()
        {
            //---------------Set up test pack-------------------
            var playerImplementationRunResult = CreatePlayerImplementationRunResult();
            playerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult>
            {
                new EdgeCaseImplementationResult{IsImplemented = true}, 
                new EdgeCaseImplementationResult{IsImplemented = false}, 
                new EdgeCaseImplementationResult{IsImplemented = false}, 
                new EdgeCaseImplementationResult{IsImplemented = true}, 
                new EdgeCaseImplementationResult{IsImplemented = true}, 
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var edgeCasesImplemented = playerImplementationRunResult.EdgeCasesImplemented;
            //---------------Test Result -----------------------
            Assert.That(edgeCasesImplemented, Is.EqualTo(3));
        }

        [Test]
        public void ToString_GivenNullEdgeCaseImplementationResults_ShouldReturnCorrectString()
        {
            //---------------Set up test pack-------------------
            var playerImplementationRunResult = CreatePlayerImplementationRunResult();
            playerImplementationRunResult.Level = 5;
            playerImplementationRunResult.EdgeCaseImplementationResults = null;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var toString = playerImplementationRunResult.ToString();
            //---------------Test Result -----------------------
            var expectedMessage = "Player Implementation: Level=5, EdgeCasesImplemented=0, TotalEdgeCases=0";
            Assert.That(toString, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void ToString_GivenNoEdgeCaseImplementationResults_ShouldReturnCorrectString()
        {
            //---------------Set up test pack-------------------
            var playerImplementationRunResult = CreatePlayerImplementationRunResult();
            playerImplementationRunResult.Level = 2;
            playerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var toString = playerImplementationRunResult.ToString();
            //---------------Test Result -----------------------
            var expectedMessage = "Player Implementation: Level=2, EdgeCasesImplemented=0, TotalEdgeCases=0";
            Assert.That(toString, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void ToString_GivenHasEdgeCaseImplementationResults_ShouldReturnCorrectString()
        {
            //---------------Set up test pack-------------------
            var playerImplementationRunResult = CreatePlayerImplementationRunResult();
            playerImplementationRunResult.Level = 7;
            playerImplementationRunResult.EdgeCaseImplementationResults = new List<EdgeCaseImplementationResult>
            {
                new EdgeCaseImplementationResult{IsImplemented = true, EdgeCaseMessage = "Should not filter out 999"}, 
                new EdgeCaseImplementationResult{IsImplemented = false, EdgeCaseMessage = "Should filter out 1000"}, 
                new EdgeCaseImplementationResult{IsImplemented = false, EdgeCaseMessage = "Should not filter out 1001"}, 
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var toString = playerImplementationRunResult.ToString();
            //---------------Test Result -----------------------
            var expectedMessage = "Player Implementation: Level=7, EdgeCasesImplemented=1, TotalEdgeCases=3";
            Assert.That(toString, Is.EqualTo(expectedMessage));
        }

        private PlayerImplementationRunResult CreatePlayerImplementationRunResult()
        {
            return new PlayerImplementationRunResult();
        }
    }
}