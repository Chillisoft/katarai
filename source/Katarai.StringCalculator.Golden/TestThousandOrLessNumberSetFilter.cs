using System.Collections.Generic;
using NUnit.Framework;

namespace Katarai.StringCalculator.Golden
{
    public class TestThousandOrLessNumberSetFilter
    {
        private ThousandOrLessNumberSetFilter CreateNumberSetFilter()
        {
            return new ThousandOrLessNumberSetFilter();
        }

        [Test]
        public void FilterValues_WhenHasNumbersGreaterThan1000_ShouldFilterThemOut()
        {
            //---------------Set up test pack-------------------
            var input = new List<int> { 1001, 2, 4 };
            var expectedResult = new List<int> { 2, 4 };
            var numberSetFilter = CreateNumberSetFilter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = numberSetFilter.FilterValues(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(expectedResult, result);
        }

        [Test]
        public void FilterValues_WhenHasNumbersMuchGreaterThan1000_ShouldFilterThemOut()
        {
            //---------------Set up test pack-------------------
            var input = new List<int> { 1001, 2, 1002, 1006, 4, 1100, 2000 };
            var expectedResult = new List<int> { 2, 4 };
            var numberSetFilter = CreateNumberSetFilter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = numberSetFilter.FilterValues(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(expectedResult, result);
        }

        [Test]
        public void FilterValues_WhenHasNumbersEqualTo1000_ShouldNotFilterThemOut()
        {
            //---------------Set up test pack-------------------
            var input = new List<int> { 1, 2, 4, 1000 };
            var expectedResult = input;
            var numberSetFilter = CreateNumberSetFilter();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = numberSetFilter.FilterValues(input);
            //---------------Test Result -----------------------
            CollectionAssert.AreEquivalent(expectedResult, result);
        }
    }
}