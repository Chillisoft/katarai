using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf
{
    public interface ILogRepository
    {
        Task<List<IAttemptLog>> GetKataAttemptLogs(string userName, int? maxResults = null);
        Task<List<IAttemptLog>> GetKataCompletedAttemptLogs(string userName);
    }

    public class LogRepository : ILogRepository
    {
        private readonly ISplunkSearch _splunkSearch;
        private readonly IKataHelper _kataHelper;

        public LogRepository(ISplunkSearch splunkSearch, IKataHelper kataHelper)
        {
            if (splunkSearch == null) throw new ArgumentNullException("splunkSearch");
            if (kataHelper == null) throw new ArgumentNullException("kataHelper");
            _splunkSearch = splunkSearch;
            _kataHelper = kataHelper;
        }

        public async Task<List<IAttemptLog>> GetKataAttemptLogs(string userName, int? maxResults = null)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("userName");
            var progressSearchLogs = await GetAttemptSearchLogs(userName);
            var attemptLogs = GetAttemptLogs(progressSearchLogs);
            var logs = attemptLogs.OrderByDescending(log => log.AttemptDate);
            if (maxResults == null)
            {
                return new List<IAttemptLog>(logs);
            }
            return new List<IAttemptLog>(logs.Take(maxResults.GetValueOrDefault()));
        }

        public async Task<List<IAttemptLog>> GetKataCompletedAttemptLogs(string userName)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("userName");
            var searchLogs = await GetCompletedSearchLogs(userName);
            var attemptLogs = GetAttemptLogs(searchLogs);
            var logs = attemptLogs.OrderByDescending(log => log.KataName).ThenByDescending(log => log.AttemptDate);
            return new List<IAttemptLog>(logs.Take(40));
        }

        private List<IAttemptLog> GetAttemptLogs(List<ISpunkSearchLog> progressSearchLogs)
        {
            var attemptLogs = new List<IAttemptLog>();
            if (progressSearchLogs == null) return attemptLogs;
            foreach (var progressSearchLog in progressSearchLogs)
            {
                attemptLogs.Add(CreateAttemptLog(progressSearchLog));
            }
            return attemptLogs;
        }

        private async Task<List<ISpunkSearchLog>> GetAttemptSearchLogs(string userName)
        {
            var progressSearchLogs = await _splunkSearch.GetProgressMaxLevelResults(userName);
            var searchLogs = await _splunkSearch.GetMaxLevelResults(userName);
            if (searchLogs == null) return progressSearchLogs;
            var spunkSearchLogs = MergeLogs(searchLogs, progressSearchLogs);
            return spunkSearchLogs;
        }

        private List<ISpunkSearchLog> MergeLogs(List<ISpunkSearchLog> searchLogs, List<ISpunkSearchLog> progressSearchLogs)
        {
            foreach (var spunkSearchLog in searchLogs)
            {
                var attempt = progressSearchLogs.FirstOrDefault(log => log.AttemptName == spunkSearchLog.AttemptName);
                if (attempt != null) continue;
                progressSearchLogs.Add(spunkSearchLog);
            }
            return progressSearchLogs;
        }


        private async Task<List<ISpunkSearchLog>> GetCompletedSearchLogs(string userName)
        {
            var progressSearchLogs = await _splunkSearch.GetCompletedProgressResults(userName);
            var searchLogs = await _splunkSearch.GetCompletedResults(userName);
            if (searchLogs == null) return progressSearchLogs;
            var spunkSearchLogs = MergeLogs(searchLogs, progressSearchLogs);
            return spunkSearchLogs;
        }

        private async Task<List<ISpunkSearchLog>> GetCompletedProgressSearchLogs(string userName)
        {
            var progressSearchLogs = await _splunkSearch.GetProgressMaxLevelResults(userName);
            var searchLogs = await _splunkSearch.GetMaxLevelResults(userName);
            if (searchLogs == null) return progressSearchLogs;
            var spunkSearchLogs = MergeLogs(searchLogs, progressSearchLogs);
            return spunkSearchLogs; 
        }

        private AttemptLog CreateAttemptLog(ISpunkSearchLog spunkSearchLog)
        {
            var attemptLog = new AttemptLog
            {
                AttemptName = spunkSearchLog.AttemptName,
                UserName = spunkSearchLog.UserName,
                KataName = _kataHelper.GetKataName(spunkSearchLog.AttemptName),
                AttemptDate = Convert.ToDateTime(spunkSearchLog.KataStartTime),
                LengthInMinutes = Math.Round(Convert.ToDecimal(spunkSearchLog.TotalDuration)/60, 2,
                    MidpointRounding.AwayFromZero),
                HighestLevelAchieved = Convert.ToInt32(spunkSearchLog.HighestLevelAchieved)
            };
            var kataMaxLevel = _kataHelper.GetKataMaxLevel(attemptLog.KataName);
            if (kataMaxLevel != 0)
            {
                attemptLog.PercentCompleted = Math.Round((Convert.ToDecimal(attemptLog.HighestLevelAchieved) / kataMaxLevel * 100), 2,
                        MidpointRounding.AwayFromZero);                
            }
            if (spunkSearchLog.TotalDuration == null)
            {
                SetDuration(spunkSearchLog, attemptLog);
            }
            if (attemptLog.HighestLevelAchieved == kataMaxLevel)
            {
                attemptLog.Completed = true;
            }
            return attemptLog;
        }

        private void SetDuration(ISpunkSearchLog spunkSearchLog, AttemptLog attemptLog)
        {
            var timestamp = Convert.ToDateTime(spunkSearchLog.Timestamp);
            if (timestamp == DateTime.MinValue) return;
            var totalDuration = (timestamp - attemptLog.AttemptDate).GetValueOrDefault().TotalSeconds;
            attemptLog.LengthInMinutes = Math.Round(Convert.ToDecimal(totalDuration)/60, 2,
                    MidpointRounding.AwayFromZero);
        }
    }
}
