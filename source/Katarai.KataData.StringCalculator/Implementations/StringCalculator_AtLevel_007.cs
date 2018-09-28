using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.Annotations;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.KataData.StringCalculator.Implementations
{
    [TestStep(7)]
    public class StringCalculator_AtLevel_007 : StringCalculator_AtLevel_006
    {
        public override int Add(string input)
        {
            var ints = GetIntsFrom(input);
            var toAdd = Process(ints);
            return toAdd.Sum();
        }

        protected virtual IEnumerable<int> Process(IEnumerable<int> values)
        {
            var negatives = values.Where(i => i < 0).ToList();
            if (negatives.Any())
            {
                throw new NegativesNotAllowedException(negatives.First());
            }
            return values;
        }
    }


}