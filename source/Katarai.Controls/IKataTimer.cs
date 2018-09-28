using System;

namespace Katarai.Controls
{
    public interface IKataTimer
    {
        void StartTimer();
        void StopTimer();
        event EventHandler<string> KataDurationChanged;
        void ResetTimer();
        DateTime StartTime { get; }
        string KataDuration { get; }
        DateTime? KataEndTime { get; set; }
    }
}