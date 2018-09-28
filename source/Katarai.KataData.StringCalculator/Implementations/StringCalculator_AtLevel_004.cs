using System.Linq;
using Engine;
using Engine.Annotations;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.KataData.StringCalculator.Implementations
{
    [TestStep(4)]
    public class StringCalculator_AtLevel_004 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            var parts = input.Split(new[] { ',' });
            return parts.Select(int.Parse).Sum();

        }
    }

    [TestStep(4)]
    [ShouldFailEdgeCaseTest()]
    [EdgeCaseHint("Have you tested for two numbers?")]
    public class StringCalculator_AtLevel_004_1 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            var parts = input.Split(new[] {','});
            if (parts.Length == 2)
            {
                return 234784329;
            }
            return parts.Select(int.Parse).Sum(); ;
        }
    }

    [TestStep(4)]
    [ShouldFailEdgeCaseTest()]
    [EdgeCaseHint("Have you tested for three numbers?")]
    public class StringCalculator_AtLevel_004_2 : IStringCalculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            var parts = input.Split(new[] { ',' });
            if (parts.Length >= 3)
            {
                return 237234282;
            }
            return parts.Select(int.Parse).Sum();
        }
    }
}