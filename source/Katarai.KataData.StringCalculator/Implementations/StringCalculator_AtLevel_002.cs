using System;
using Engine;
using Engine.Annotations;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.KataData.StringCalculator.Implementations
{
    [TestStep(2)]
    public class StringCalculator_AtLevel_002 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "")
                return 0;
            throw new NotImplementedException("DIE");
        }
    }
}