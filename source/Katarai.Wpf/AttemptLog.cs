using System;

namespace Katarai.Wpf
{
    public interface IAttemptLog
    {
        string KataName { get; set; }
        string AttemptName { get; set; }
        string UserName { get; set; }
        DateTime? AttemptDate { get; set; }
        decimal? LengthInMinutes { get; set; }
        int? HighestLevelAchieved { get; set; }
        decimal? PercentCompleted { get; set; }
        bool Completed { get; set; }
    }

    public class AttemptLog : IAttemptLog
    {
        public string KataName { get; set; }
        public string AttemptName { get; set; }
        public string UserName { get; set; }
        public DateTime? AttemptDate { get; set; }
        public decimal? LengthInMinutes { get; set; }
        public int? HighestLevelAchieved { get; set; }
        public decimal? PercentCompleted { get; set; }
        public bool Completed { get; set; }
    }

    public interface ISpunkSearchLog
    {
        string AttemptName { get; set; }
        string KataStartTime { get; set; }
        string TotalDuration { get; set; }
        string HighestLevelAchieved { get; set; }
        string UserName { get; set; }
        string Timestamp { get; set; }
    }

    public class SpunkSearchLog : ISpunkSearchLog
    {
        public string AttemptName { get; set; }
        public string KataStartTime { get; set; }
        public string TotalDuration { get; set; }
        public string HighestLevelAchieved { get; set; }
        public string UserName { get; set; }
        public string Timestamp { get; set; }
    }
}
