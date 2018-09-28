using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.Annotations;
using Katarai.KataData.StringCalculator.Implementations.Final;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.KataData.StringCalculator.Implementations
{
    [TestStep(6)]
    public class StringCalculator_AtLevel_006 : IStringCalculator
    {

        public virtual int Add(string input)
        {
            var ints = GetIntsFrom(input);
            return ints.Sum();
        }

        protected IEnumerable<int> GetIntsFrom(string input)
        {
            if (string.IsNullOrEmpty(input)) return new[] {0};
            var parts = input.Split(new[] {'\n'}, StringSplitOptions.None);
            var delimiters = input.StartsWith("//") ? new[] {parts[0].TrimStart('/'), ",", "\n"} : new[] {",", "\n"};
            if (delimiters.Any(d => d.Length > 1 && !(d.Contains("[") && d.Contains("]"))))
            {
                return new List<int>{59315461};
            }
            var skip = input.StartsWith("//") ? 1 : 0;
            var dataPart = string.Join("\n", parts.Skip(skip));
            var ints = dataPart.Split(MangleDelimiters(delimiters), 
                StringSplitOptions.None)
                .Select(int.Parse);
            return ints;
        }

        protected virtual string[] MangleDelimiters(string[] delimiters)
        {
            return delimiters.Select(d => d[0].ToString()).ToArray();
        }

    }
}