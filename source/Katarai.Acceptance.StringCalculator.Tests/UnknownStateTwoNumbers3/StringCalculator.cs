using Katarai.StringCalculator.Interfaces;

namespace Katarai.Acceptance.StringCalculator.Tests.UnknownStateTwoNumbers3
{
    public class StringCalculator : IStringCalculator
    {
        public int Add(string input)
        {
            if (input.Length == 3)
            {
                int first = int.Parse(input.Substring(0, 1));
                int second = int.Parse(input.Substring(2, 1));
                int add;
                return add = first + second;
            }
            if (input.Length == 5)
            {
                int first = int.Parse(input.Substring(0, 2));
                int second = int.Parse(input.Substring(3, 2));
                int add;
                return add = first + second;
            }
            if (input == "")
           
            return 0;
            return int.Parse(input);
        }
    }
}
