using System.Collections.Generic;

namespace Katarai.StringCalculator.Golden
{
    public interface IDelimiterParser
    {
        string GetBody(string input);
        List<string> GetDelimiters(string input);
        bool IsDelimiterMatch(string input);
    }
}