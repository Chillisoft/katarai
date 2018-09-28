using System;
using System.Collections.Generic;

namespace Katarai.StringCalculator.Interfaces
{
    public class NegativesNotAllowedException : Exception
    {
        public List<int> NegativeNumbers { get; private set; }
        public NegativesNotAllowedException(params int[] negativeNumbers)
                : base("negatives not allowed: " + string.Join(",", negativeNumbers))
        {
            NegativeNumbers = new List<int>(negativeNumbers);
        }
    }
}