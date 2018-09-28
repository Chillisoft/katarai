using System.Collections.Generic;

namespace Katarai.StringCalculator.Golden
{
    public interface INumberSetFilter
    {
        IEnumerable<int> FilterValues(IEnumerable<int> values);
    }
}