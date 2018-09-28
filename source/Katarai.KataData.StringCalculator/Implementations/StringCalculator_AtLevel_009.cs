using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.Annotations;

namespace Katarai.KataData.StringCalculator.Implementations
{
    [TestStep(9)]
    public class StringCalculator_AtLevel_009 : StringCalculator_AtLevel_008
    {
        protected override IEnumerable<int> Process(IEnumerable<int> values)
        {
            ValidateNegatives(values);
            return values.Where(v => v <= 1000);
        }
    }

    [TestStep(9)]
    [ShouldFailEdgeCaseTest()]
    //[FailingEdgeCaseThatIncorrectly("filters out 1000")]
    [EdgeCaseHint("Have you tested that 1000 is not filtered out?")]
    public class StringCalculator_AtLevel_009_1 : StringCalculator_AtLevel_008
    {
        protected override IEnumerable<int> Process(IEnumerable<int> values)
        {
            ValidateNegatives(values);
            return values.Where(v => v < 1000);
        }
    }

    [TestStep(9)]
    [ShouldFailEdgeCaseTest()]
    [EdgeCaseHint("Have you tested that 1001 is filtered out as well?")]
    public class StringCalculator_AtLevel_009_2 : StringCalculator_AtLevel_008
    {
        protected override IEnumerable<int> Process(IEnumerable<int> values)
        {
            ValidateNegatives(values);
            return values.Where(v => v <= 1001);
        }
    }
}