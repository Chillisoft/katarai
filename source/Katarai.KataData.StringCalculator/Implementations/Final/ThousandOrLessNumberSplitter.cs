using System.Collections.Generic;
using System.Linq;

namespace Katarai.KataData.StringCalculator.Implementations.Final
{
    public interface INumberSetFilter
    {
        IEnumerable<int> FilterValues(IEnumerable<int> values);
    }
    public class ThousandOrLessNumberSetFilter : INumberSetFilter
    {
        public IEnumerable<int> FilterValues(IEnumerable<int> values)
        {
            return values.Where(i => i <= 1000);
        }
    }
}
