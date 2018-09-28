using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.StringCalculator.Golden
{
    [TestFixture]
    public class TestStringSplitter
    {
        [Test]
        public void Construct()
        {
            new StringSplitter(GetStandardDelimiterParsers());
        }



        [Test]
        public void GetStrings_WhenEmpty_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var stringSplitter = CreateStringSplitter();
            var input = "";
            var expectedResult = new List<string>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringSplitter.GetStrings(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(expectedResult, result);
        }



        [Test]
        public void GetStrings_WhenTwoNumbers_ShouldReturnList()
        {
            //---------------Set up test pack-------------------
            var stringSplitter = CreateStringSplitter();
            var input = "1,2";
            var expectedResult = new List<string>{"1","2"};
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringSplitter.GetStrings(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(expectedResult, result);
        }

        [Test]
        public void GetStrings_WhenManyNumbers_ShouldReturnList()
        {
            //---------------Set up test pack-------------------
            var stringSplitter = CreateStringSplitter();
            var input = "1,2,3";
            var expectedResult = new List<string> { "1", "2", "3" };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringSplitter.GetStrings(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetStrings_WhenManyNumbers_WithNewline_ShouldReturnList()
        {
            //---------------Set up test pack-------------------
            var stringSplitter = CreateStringSplitter();
            var input = "1,2,5\n3,4";
            var expectedResult = new List<string> { "1","2","5","3","4" };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringSplitter.GetStrings(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetStrings_WhenManyNumbers_WithCustomDelimiter_ShouldReturnList()
        {
            //---------------Set up test pack-------------------
            var stringSplitter = CreateStringSplitter();
            var input = "//*\n1,2,5*3,4";
            var expectedResult = new List<string> { "1", "2", "5", "3", "4" };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringSplitter.GetStrings(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetStrings_WhenManyNumbers_WithDifferentCustomDelimiter_ShouldReturnList()
        {
            //---------------Set up test pack-------------------
            var stringSplitter = CreateStringSplitter();
            var input = "//%\n1,2%3,4";
            var expectedResult = new List<string> { "1", "2", "3", "4" };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringSplitter.GetStrings(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetStrings_ShouldCallMethodsOnMatchingDelimiterParser()
        {
            //---------------Set up test pack-------------------
            var delimiterParsers = new List<IDelimiterParser>
                                   {
                                       Substitute.For<IDelimiterParser>()
                                   };
            var matchingParser = delimiterParsers[0];
            var stringSplitter = CreateStringSplitter(delimiterParsers);
            var input = "";
            matchingParser.IsDelimiterMatch(input).Returns(true);
            matchingParser.GetBody(input).Returns(input);
            matchingParser.GetDelimiters(input).Returns(new List<string>());
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            stringSplitter.GetStrings(input);
            //---------------Test Result -----------------------
            matchingParser.Received(1).IsDelimiterMatch(input);
            matchingParser.Received(1).GetBody(input);
            matchingParser.Received(1).GetDelimiters(input);
        }

        [Test]
        public void GetStrings_ShouldCallMethodsOnFirstMatchingDelimiterParser()
        {
            //---------------Set up test pack-------------------
            var delimiterParsers = new List<IDelimiterParser>
                                   {
                                       Substitute.For<IDelimiterParser>(),
                                       Substitute.For<IDelimiterParser>(),
                                       Substitute.For<IDelimiterParser>()
                                   };
            var nonMatchingParser = delimiterParsers[0];
            var matchingParser = delimiterParsers[1];
            var remainingParser = delimiterParsers[2];
            var stringSplitter = CreateStringSplitter(delimiterParsers);
            var input = "";
            nonMatchingParser.IsDelimiterMatch(input).Returns(false);
            matchingParser.IsDelimiterMatch(input).Returns(true);
            matchingParser.GetBody(input).Returns(input);
            matchingParser.GetDelimiters(input).Returns(new List<string>());
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            stringSplitter.GetStrings(input);
            //---------------Test Result -----------------------
            nonMatchingParser.Received(1).IsDelimiterMatch(input);
            matchingParser.Received(1).IsDelimiterMatch(input);
            remainingParser.DidNotReceive().IsDelimiterMatch(input);
        }

        private static StringSplitter CreateStringSplitter(List<IDelimiterParser> delimiterParsers = null)
        {
            if (delimiterParsers == null)
            {
                delimiterParsers = GetStandardDelimiterParsers();
            }
            return new StringSplitter(delimiterParsers);
        }

        private static List<IDelimiterParser> GetStandardDelimiterParsers()
        {
            return new List<IDelimiterParser>
                   {
                       new CustomDelimiterParser(),
                       new DefaultDelimiterParser()
                   };
        }
    }
}