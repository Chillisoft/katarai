using System.Collections.Generic;
using System.Linq;
using Katarai.Wpf.PackagedKata;

namespace Katarai.Wpf.Utils
{

    public interface IKataHelper
    {
        int GetKataMaxLevel(string kataName);
        string GetKataName(string attemptName);
    }

    public class KataHelper : IKataHelper
    {
        private readonly List<string> _kataNames = new List<string>()
        {
            KataName.StringCalculator.ToString(),
            KataName.FizzBuzz.ToString()
        };

        private readonly Dictionary<string, int> _kataMaxLevels = new Dictionary<string, int>
        {
            {KataName.StringCalculator.ToString(), 11},
            {KataName.FizzBuzz.ToString(), 13}
        };

        public int GetKataMaxLevel(string kataName)
        {
            int kataMaxLevel;
            _kataMaxLevels.TryGetValue(kataName, out kataMaxLevel);
            return kataMaxLevel;
        }

        public string GetKataName(string attemptName)
        {
            if (attemptName == null) return string.Empty;
            var indexOfDash = attemptName.IndexOf('-');
            var name = attemptName.Substring(0, indexOfDash);
            var kataName = _kataNames.FirstOrDefault(s => s == name);
            return kataName == null ? attemptName : name;
        }
    }

}