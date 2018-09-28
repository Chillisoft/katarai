using Katarai.Runner;
using Katarai.Wpf.FileWatch;

namespace Katarai.Wpf.Settings
{
    public interface ISettingsManager
    {
        void SetSettings(KataraiSettings settings);
        KataraiSettings FetchCurrentSettings();
        void LoadSettings();
        bool PersistSettings();
        void ToggleHint();
        bool IsShowHintOn();
        void UpdateSettings(FileMonitorInformation fileHashes);
        void ToggleIsAlwaysOnTop();
        bool IsAlwaysOnTopOn();
        int GetNotificationVisibilityTime();
        bool PersistSettings(KataraiSettings settings);
    }

}
