using System.Collections.Generic;
using Katarai.StringCalculator.Interfaces;
using NUnit.Framework;

namespace Katarai.StringCalculator.Golden
{
    public class TestNoNegativesNumberSetCheck
    {
        private NoNegativesNumberSetCheck CreateNumberSetCheck()
        {
            return new NoNegativesNumberSetCheck();
        }

        [Test]
        public void Check_WhenNoNegatives_ShouldNotThrowError()
        {
            //---------------Set up test pack-------------------
            var numberSetCheck = CreateNumberSetCheck();
            var input = new List<int> { 1, 2, 3, 4, 6 };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => numberSetCheck.Check(input));
        }

        [Test]
        public void Check_WhenHasNegatives_ShouldThrowErrorWithMessage()
        {
            //---------------Set up test pack-------------------
            var numberSetCheck = CreateNumberSetCheck();
            var input = new List<int> { 1, -2, 3, -4, 6, -7 };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<NegativesNotAllowedException>(() => numberSetCheck.Check(input));
            //---------------Test Result -----------------------
            CollectionAssert.Contains(exception.NegativeNumbers, -2);
            CollectionAssert.Contains(exception.NegativeNumbers, -4);
            CollectionAssert.Contains(exception.NegativeNumbers, -7);
        }

        [Test]
        public void Check_WhenHasOtherNegatives_ShouldThrowErrorWithMessage()
        {
            //---------------Set up test pack-------------------
            var numberSetCheck = CreateNumberSetCheck();
            var input = new List<int> {1,2,-3,-4};
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<NegativesNotAllowedException>(() => numberSetCheck.Check(input));
            //---------------Test Result -----------------------
            CollectionAssert.Contains(exception.NegativeNumbers, -3);
            CollectionAssert.Contains(exception.NegativeNumbers, -4);
        }
    }
}