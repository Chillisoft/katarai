using System;
using System.Collections.Generic;
using System.Linq;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.KataData.StringCalculator.Implementations.Final
{
    public class StringCalculator : IStringCalculator
    {
        private readonly IStringSplitter _stringSplitter;
        private readonly INumberSetCheck _numberSetCheck;
        private readonly INumberSetFilter _numberSetFilter;

        public StringCalculator(IStringSplitter stringSplitter, INumberSetCheck numberSetCheck, INumberSetFilter numberSetFilter)
        {
            _numberSetFilter = numberSetFilter;
            _numberSetCheck = numberSetCheck;
            _stringSplitter = stringSplitter;
        }

        public StringCalculator()
            : this(new StringSplitter(new List<IDelimiterParser>
                                      {
                                          new CustomDelimiterParser(),
                                          new DefaultDelimiterParser()
                                      }), 
            new NoNegativesNumberSetCheck(), 
            new ThousandOrLessNumberSetFilter())
        {
        }

        public int Add(string input)
        {
            if (string.IsNullOrEmpty(input)) return 0;
            var values = GetValues(input);
            var filteredValues = _numberSetFilter.FilterValues(values);
            return filteredValues.Sum();
        }

        private IEnumerable<int> GetValues(string input)
        {
            var strings = _stringSplitter.GetStrings(input);
            var values = ParseValues(strings).ToList();
            return GetValidValues(values);
        }

        private IEnumerable<int> GetValidValues(List<int> values)
        {
            _numberSetCheck.Check(values);
            return values;
        }

        private IEnumerable<int> ParseValues(IEnumerable<string> numbers)
        {
            return numbers.Select(int.Parse).ToList();
        }
    }
}
