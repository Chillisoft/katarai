using System.Collections.Generic;
using NUnit.Framework;

namespace Katarai.StringCalculator.Golden
{
    [TestFixture]
    public class TestDefaultDelimiterParser
    {
        private static DefaultDelimiterParser CreateDefaultDelimiterParser()
        {
            return new DefaultDelimiterParser();
        }

        [TestCase("")]
        [TestCase("1")]
        [TestCase("1,2,3,4")]
        public void IsDelimiterMatch_ShouldAlwaysReturnTrue(string input)
        {
            //---------------Set up test pack-------------------
            var defaultDelimiterParser = CreateDefaultDelimiterParser();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = defaultDelimiterParser.IsDelimiterMatch(input);
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
        }

        [Test]
        public void GetDelimiters_ShouldReturnDefaultDelimiters()
        {
            //---------------Set up test pack-------------------
            var defaultDelimiterParser = CreateDefaultDelimiterParser();
            var expectedDelimiters = new List<string> { ",", "\n" };
            var input = "1,2";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = defaultDelimiterParser.GetDelimiters(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(expectedDelimiters, result);
        }

        [TestCase("1")]
        [TestCase("1,2")]
        [TestCase("1,2,3")]
        [TestCase("1\n2,3")]
        public void GetBody_ShouldReturnInputString(string input)
        {
            //---------------Set up test pack-------------------
            var defaultDelimiterParser = CreateDefaultDelimiterParser();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = defaultDelimiterParser.GetBody(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(input, result);
        }
    }
}