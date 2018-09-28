using System.Collections.Generic;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.StringCalculator.Golden
{
    public static class SUTFactory
    {

        public static IStringCalculator CreateCalculator(IStringSplitter stringSplitter = null, INumberSetCheck numberSetCheck = null, INumberSetFilter numberSetFilter = null)
        {
            if (stringSplitter == null && numberSetCheck == null && numberSetFilter == null)
            {
                return new StringCalculator();

            }
            stringSplitter = stringSplitter ?? new StringSplitter(new List<IDelimiterParser>
                                                                  {
                                                                      new CustomDelimiterParser(),
                                                                      new DefaultDelimiterParser()
                                                                  });
            numberSetCheck = numberSetCheck ?? new NoNegativesNumberSetCheck();
            numberSetFilter = numberSetFilter ?? new ThousandOrLessNumberSetFilter();
            return new StringCalculator(stringSplitter, numberSetCheck, numberSetFilter);
        }
        
    }
}