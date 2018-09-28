using Katarai.Runner;
using Katarai.Wpf.Settings;

namespace Katarai.Wpf.AnalysisContainers
{
    public interface IAnalysisContainer
    {
        string ExecuteShadowLocation { get; }

        Result Execute(KataraiSettings settings);
    }

}
