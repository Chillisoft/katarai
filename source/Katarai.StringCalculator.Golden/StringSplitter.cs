using System;
using System.Collections.Generic;
using System.Linq;

namespace Katarai.StringCalculator.Golden
{
    public class StringSplitter : IStringSplitter
    {
        private readonly List<IDelimiterParser> _delimiterParsers;

        public StringSplitter(List<IDelimiterParser> delimiterParsers)
        {
            _delimiterParsers = delimiterParsers;
        }

        public IEnumerable<string> GetStrings(string input)
        {
            var delimiterParser = GetDelimiterParserFor(input);
            return ExtractDelimitedInput(input, delimiterParser);
        }

        private IDelimiterParser GetDelimiterParserFor(string input)
        {
            return _delimiterParsers.First(parser => parser.IsDelimiterMatch(input));
        }

        private IEnumerable<string> ExtractDelimitedInput(string input, IDelimiterParser delimiterParser)
        {
            var delimiter = delimiterParser.GetDelimiters(input);
            input = delimiterParser.GetBody(input);
            return SplitInputOnDelimiters(input, delimiter);
        }

        private IEnumerable<string> SplitInputOnDelimiters(string input, List<string> delimiter)
        {
            return input.Split(delimiter.ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }
    }
}