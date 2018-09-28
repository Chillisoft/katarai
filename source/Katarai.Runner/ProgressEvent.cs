using System;

namespace Katarai.Runner
{
    public class ProgressEvent
    {
        public int DurationInSeconds { get; set; }
        public int FromLevel { get; set; }
        public int ToLevel { get; set; }
        public string FromLevelDescription { get; set; }
        public string ToLevelDescription { get; set; }
        public bool KataCompleted { get; set; }
        public DateTime KataCompletedTime { get; set; }
        public DateTime KataStartTime { get; set; }
        public int KataDurationInSeconds { get; set; }
        public string LevelsCompleted { get; set; }
        public bool AllLevelsCompleted { get; set; }
        public bool KataAbandoned { get; set; }
    }
}