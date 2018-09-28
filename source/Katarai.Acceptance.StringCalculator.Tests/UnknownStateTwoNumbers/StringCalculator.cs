using Katarai.StringCalculator.Interfaces;

namespace Katarai.Acceptance.StringCalculator.Tests.UnknownStateTwoNumbers
{
    public class StringCalculator : IStringCalculator
    {
        public int Add(string input)
        {
            if (input.Contains(","))
            {
                return SafeParse(input[0].ToString()) + SafeParse(input[2].ToString());
            }
            return SafeParse(input);
        }

        private static int SafeParse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }
            return int.Parse(input);
        }
    }
}
