using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.StringCalculator.Golden
{
    [TestFixture]
    public class TestStringCalculator_Integration
    {

        [Test]
        public void Add_ShouldSumNumbersFromStringSplitter()
        {
            //---------------Set up test pack-------------------
            var stringSplitter = Substitute.For<IStringSplitter>();
            var stringCalculator = SUTFactory.CreateCalculator(stringSplitter);
            var input = "1,2,3";
            var listOfNumbersToSum = new List<string> { "10", "3" };
            stringSplitter.GetStrings(input).Returns(listOfNumbersToSum);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(13, result);
        }

        [Test]
        public void Add_ShouldCheckNumbersWithNumberSetCheck()
        {
            //---------------Set up test pack-------------------
            var numberSetCheck = Substitute.For<INumberSetCheck>();
            var stringCalculator = SUTFactory.CreateCalculator(numberSetCheck: numberSetCheck);
            var input = "1,2,3";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            stringCalculator.Add(input);
            //---------------Test Result -----------------------
            numberSetCheck.Received().Check(Arg.Any<List<int>>());
        }

        [Test]
        public void Add_ShouldCallNumberSetFilter()
        {
            //---------------Set up test pack-------------------
            var numberSetFilter = Substitute.For<INumberSetFilter>();
            var stringCalculator = SUTFactory.CreateCalculator(numberSetFilter: numberSetFilter);
            var input = "1,2,3";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            stringCalculator.Add(input);
            //---------------Test Result -----------------------
            numberSetFilter.Received().FilterValues(Arg.Any<List<int>>());
        }

        [Test]
        public void Add_ShouldFilterNumbersWithNumberSetFilter()
        {
            //---------------Set up test pack-------------------
            const string input = "1,2,3";
            var filteredList = new List<int> { 2 };
            var numberSetFilter = Substitute.For<INumberSetFilter>();
            numberSetFilter.FilterValues(Arg.Any<List<int>>()).Returns(filteredList);
            var stringCalculator = SUTFactory.CreateCalculator(numberSetFilter: numberSetFilter);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = stringCalculator.Add(input);
            //---------------Test Result -----------------------
            var expectedResult = filteredList.Sum();
            Assert.AreEqual(expectedResult, result);
        }

    }
}