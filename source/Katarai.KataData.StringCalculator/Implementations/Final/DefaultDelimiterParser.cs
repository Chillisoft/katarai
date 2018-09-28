using System.Collections.Generic;

namespace Katarai.KataData.StringCalculator.Implementations.Final
{
    public class DefaultDelimiterParser : IDelimiterParser
    {
        public string GetBody(string input)
        {
            return input;
        }

        public List<string> GetDelimiters(string input)
        {
            return new List<string> {",", "\n"};
        }

        public bool IsDelimiterMatch(string input)
        {
            return true;
        }
    }
}
