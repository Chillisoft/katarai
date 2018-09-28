using System.Linq;
using Engine;
using Engine.Annotations;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.KataData.StringCalculator.Implementations
{
    [TestStep(5)]
    public class StringCalculator_AtLevel_005 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            var parts = input.Split(new[] { ',', '\n' });
            return parts.Select(int.Parse).Sum();
        }
    }
}