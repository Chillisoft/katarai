using Katarai.Runner;

namespace Katarai.Wpf.FileWatch
{
    public interface IAnalysisRunner
    {
        Result Run(out string errorMessage);
    }

}
