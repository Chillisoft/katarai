using Katarai.Runner;
using Katarai.Wpf.Settings;

namespace Katarai.Wpf.FileWatch
{
    public interface IFileHashesManager
    {
        FileMonitorInformation GetFileHashes(KataraiSettings settings);
    }
}