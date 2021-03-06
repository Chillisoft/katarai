﻿using System;
using System.Collections.Generic;
using System.Linq;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.StringCalculator.Golden
{
    public class NoNegativesNumberSetCheck : INumberSetCheck
    {

        public void Check(IEnumerable<int> values)
        {
            var negatives = values.Where(i => i < 0);
            if (negatives.Any())
            {
                throw new NegativesNotAllowedException(negatives.ToArray());
            }
        }
    }


}