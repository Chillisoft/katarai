using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.Annotations;

namespace Katarai.KataData.StringCalculator.Implementations
{
    [TestStep(10)]
    public class StringCalculator_AtLevel_010 : StringCalculator_AtLevel_009
    {
        protected override string[] MangleDelimiters(string[] delimiters)
        {
            return delimiters
                    .Select(d => d.Trim(new [] { '[', ']' }))
                    .ToArray();
        }
    }
}