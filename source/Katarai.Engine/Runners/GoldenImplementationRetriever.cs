using System;
using System.Linq;

namespace Engine.Runners
{
    public class GoldenImplementationRetriever
    {
        public GoldenImplementation[] GetGoldenImplementations(Type[] goldenImplementationTypes)
        {
            return goldenImplementationTypes.Select(t => new GoldenImplementation(t))
                .Where(o => o.StepNumber > 0)
                .ToArray();
        }
    }
}