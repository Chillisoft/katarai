using System;
using System.Collections.Generic;

namespace Katarai.StringCalculator.Golden
{
    public class CustomDelimiterParser : IDelimiterParser
    {
        private readonly List<string> _defaultDelimiters = new List<string> {",", "\n"};

        public string GetBody(string input)
        {
            CheckIsParserMatchedInput(input);
            var parts = SplitOnNewLine(input);
            return parts[1];
        }

        private string[] SplitOnNewLine(string input)
        {
            return input.Split(new[] {'\n'}, 2);
        }

        public List<string> GetDelimiters(string input)
        {
            CheckIsParserMatchedInput(input);
            var customDelimiters = GetCustomDelimitersFromInput(input);
            var delimiters = new List<string>(_defaultDelimiters);
            delimiters.AddRange(customDelimiters);
            return delimiters;
        }

        private IEnumerable<string> GetCustomDelimitersFromInput(string input)
        {
            var parts = SplitOnNewLine(input);
            var delimiterSection = parts[0];
            var delimiterString = RemoveDelimiterPrefix(delimiterSection);
            return GetCustomDelimiters(delimiterString);
        }

        private IEnumerable<string> GetCustomDelimiters(string delimiterString)
        {
            return delimiterString.Split(new[]{'[',']'}, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string RemoveDelimiterPrefix(string delimiterSection)
        {
            return delimiterSection.Substring(2);
        }

        private void CheckIsParserMatchedInput(string input)
        {
            if (!IsDelimiterMatch(input))
                throw new ArgumentException("Parser not matched: no custom delimiter found.", "input");
        }

        public bool IsDelimiterMatch(string input)
        {
            return input.StartsWith("//");
        }
    }
}