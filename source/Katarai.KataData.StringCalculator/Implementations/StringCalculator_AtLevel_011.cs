using System;
using System.Linq;
using Engine;
using Engine.Annotations;

namespace Katarai.KataData.StringCalculator.Implementations
{
    [TestStep(11)]
    public class StringCalculator_AtLevel_011 : StringCalculator_AtLevel_010
    {
        protected override string[] MangleDelimiters(string[] delimiters)
        {
            var delimiterString = delimiters.First();
            var strings = delimiterString.Trim(new[] {'[', ']'})
                .Split(new[] {"]["}, StringSplitOptions.None).ToList();
            if (delimiters.Length > 1)
            {
                strings.AddRange(delimiters.Skip(1));
            }
            return strings
                .ToArray();
        }
    }
}