using System;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Annotations;
using Katarai.StringCalculator.Interfaces;

namespace Katarai.KataData.StringCalculator.Implementations
{
    [TestStep(1)]
    public class StringCalculator_AtLevel_001 : IStringCalculator
    {
        public int Add(string input)
        {
            throw new NotImplementedException();
        }
    }
}
