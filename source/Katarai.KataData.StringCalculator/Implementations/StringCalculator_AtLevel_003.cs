using Engine;
using Engine.Annotations;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.KataData.StringCalculator.Implementations
{
    [TestStep(3)]
    public class StringCalculator_AtLevel_003 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            return int.Parse(input);
        }
    }
}