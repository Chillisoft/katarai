using System;
using System.Collections.Generic;
using Engine.Analysers;
using Engine.Runners;
using NUnit.Framework;

namespace Katarai.Engine.Tests.Runners
{
    [TestFixture]
    public class TestPlayerTestsRunResult
    {
        [Test]
        public void TotalEdgeCases_GivenPlayerTestsEdgeCaseResultsIsNull_ShouldReturnZero()
        {
            //---------------Set up test pack-------------------
            var playerTestsRunResult = CreatePlayerTestsRunResult();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var totalEdgeCases = playerTestsRunResult.TotalEdgeCases;
            //---------------Test Result -----------------------
            Assert.That(totalEdgeCases, Is.EqualTo(0));
        }

        [Test]
        public void TotalEdgeCases_GivenOnePlayerTestsEdgeCaseResults_ShouldReturnOne()
        {
            //---------------Set up test pack-------------------
            var playerTestsRunResult = CreatePlayerTestsRunResult();
            playerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult>{new EdgeCaseCoverageResult()};
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var totalEdgeCases = playerTestsRunResult.TotalEdgeCases;
            //---------------Test Result -----------------------
            Assert.That(totalEdgeCases, Is.EqualTo(1));
        }

        [Test]
        public void TotalEdgeCases_GivenManyPlayerTestsEdgeCaseResults_ShouldReturnCaseCount()
        {
            //---------------Set up test pack-------------------
            var playerTestsRunResult = CreatePlayerTestsRunResult();
            playerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult> { new EdgeCaseCoverageResult(), new EdgeCaseCoverageResult(), new EdgeCaseCoverageResult() };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var totalEdgeCases = playerTestsRunResult.TotalEdgeCases;
            //---------------Test Result -----------------------
            Assert.That(totalEdgeCases, Is.EqualTo(3));
        }

        [Test]
        public void EdgeCasesCovered_GivenPlayerTestsEdgeCaseResultsIsNull_ShouldReturnZero()
        {
            //---------------Set up test pack-------------------
            var playerTestsRunResult = CreatePlayerTestsRunResult();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var edgeCasesCovered = playerTestsRunResult.EdgeCasesCovered;
            //---------------Test Result -----------------------
            Assert.That(edgeCasesCovered, Is.EqualTo(0));
        }
        
        [Test]
        public void EdgeCasesCovered_GivenOnePlayerTestsEdgeCaseResultsThatIsCovered_ShouldReturnOne()
        {
            //---------------Set up test pack-------------------
            var playerTestsRunResult = CreatePlayerTestsRunResult();
            playerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult> { new EdgeCaseCoverageResult{IsCovered = true} };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var edgeCasesCovered = playerTestsRunResult.EdgeCasesCovered;
            //---------------Test Result -----------------------
            Assert.That(edgeCasesCovered, Is.EqualTo(1));
        }

        [Test]
        public void EdgeCasesCovered_GivenOnePlayerTestsEdgeCaseResultsThatIsNotCovered_ShouldReturnZero()
        {
            //---------------Set up test pack-------------------
            var playerTestsRunResult = CreatePlayerTestsRunResult();
            playerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult> { new EdgeCaseCoverageResult{IsCovered = false} };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var edgeCasesCovered = playerTestsRunResult.EdgeCasesCovered;
            //---------------Test Result -----------------------
            Assert.That(edgeCasesCovered, Is.EqualTo(0));
        }

        [Test]
        public void EdgeCasesCovered_GivenTotalEdgeCasesEQ5AndThreeCovered_ShouldReturnThree()
        {
            //---------------Set up test pack-------------------
            var playerTestsRunResult = CreatePlayerTestsRunResult();
            playerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult>
            {
                new EdgeCaseCoverageResult{IsCovered = true}, 
                new EdgeCaseCoverageResult{IsCovered = false}, 
                new EdgeCaseCoverageResult{IsCovered = false}, 
                new EdgeCaseCoverageResult{IsCovered = true}, 
                new EdgeCaseCoverageResult{IsCovered = true}, 
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var edgeCasesCovered = playerTestsRunResult.EdgeCasesCovered;
            //---------------Test Result -----------------------
            Assert.That(edgeCasesCovered, Is.EqualTo(3));
        }

        [Test]
        public void ToString_GivenNullPlayerTestsEdgeCaseResults_ShouldReturnCorrectString()
        {
            //---------------Set up test pack-------------------
            var playerTestsRunResult = CreatePlayerTestsRunResult();
            playerTestsRunResult.Level = 5;
            playerTestsRunResult.NumberOfTestsImplemented = 6;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var toString = playerTestsRunResult.ToString();
            //---------------Test Result -----------------------
            var expectedMessage = "Player Test Fixture : Level=5, NumberOfTestsImplemented=6" + Environment.NewLine +
                                  "Edge Cases Covered = 0 of 0" + Environment.NewLine;
           Assert.That(toString, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void ToString_GivenNoPlayerTestsEdgeCaseResults_ShouldReturnCorrectString()
        {
            //---------------Set up test pack-------------------
            var playerTestsRunResult = CreatePlayerTestsRunResult();
            playerTestsRunResult.Level = 2;
            playerTestsRunResult.NumberOfTestsImplemented = 3;
            playerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var toString = playerTestsRunResult.ToString();
            //---------------Test Result -----------------------
            var expectedMessage = "Player Test Fixture : Level=2, NumberOfTestsImplemented=3" + Environment.NewLine +
                                  "Edge Cases Covered = 0 of 0" + Environment.NewLine;
           Assert.That(toString, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void ToString_GivenHasPlayerTestsEdgeCaseResults_ShouldReturnCorrectString()
        {
            //---------------Set up test pack-------------------
            var playerTestsRunResult = CreatePlayerTestsRunResult();
            playerTestsRunResult.Level = 7;
            playerTestsRunResult.NumberOfTestsImplemented = 7;
            playerTestsRunResult.PlayerTestsEdgeCoverageResults = new List<EdgeCaseCoverageResult>
            {
                new EdgeCaseCoverageResult{IsCovered = true, EdgeCaseMessage = "Should not filter out 999"}, 
                new EdgeCaseCoverageResult{IsCovered = false, EdgeCaseMessage = "Should filter out 1000"}, 
                new EdgeCaseCoverageResult{IsCovered = false, EdgeCaseMessage = "Should not filter out 1001"}, 
            };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var toString = playerTestsRunResult.ToString();
            //---------------Test Result -----------------------
            var expectedMessage = "Player Test Fixture : Level=7, NumberOfTestsImplemented=7" + Environment.NewLine +
                                  "Edge Cases Covered = 1 of 3" + Environment.NewLine +
                                  "Edge Cases Missed:" + Environment.NewLine +
                                  "Should filter out 1000" + Environment.NewLine +
                                  "Should not filter out 1001";
           Assert.That(toString, Is.EqualTo(expectedMessage));
        }

        private PlayerTestsRunResult CreatePlayerTestsRunResult()
        {
            return new PlayerTestsRunResult();
        }
    }
}