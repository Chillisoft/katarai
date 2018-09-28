using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.Annotations;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.KataData.StringCalculator.Implementations
{
    [TestStep(8)]
    public class StringCalculator_AtLevel_008 : StringCalculator_AtLevel_007
    {
        protected override IEnumerable<int> Process(IEnumerable<int> values)
        {
            ValidateNegatives(values);
            return values;
        }

        protected void ValidateNegatives(IEnumerable<int> values)
        {
            var negatives = values.Where(i => i < 0).ToList();
            if (negatives.Any())
            {
                throw new NegativesNotAllowedException(negatives.ToArray());
            }
        }
    }
}