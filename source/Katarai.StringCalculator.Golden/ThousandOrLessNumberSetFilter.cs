using System.Collections.Generic;
using System.Linq;

namespace Katarai.StringCalculator.Golden
{
    public class ThousandOrLessNumberSetFilter : INumberSetFilter
    {
        public IEnumerable<int> FilterValues(IEnumerable<int> values)
        {
            return values.Where(i => i <= 1000);
        }
    }
}