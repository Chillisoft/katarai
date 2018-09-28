using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Splunk.Client;

namespace Katarai.Wpf
{

    public interface ISplunkSearch
    {
        Task<List<ISpunkSearchLog>> GetProgressMaxLevelResults(string userName);
        Task<List<ISpunkSearchLog>> GetMaxLevelResults(string userName);
        Task<List<ISpunkSearchLog>> GetCompletedProgressResults(string userName);
        Task<List<ISpunkSearchLog>> GetCompletedResults(string userName);
    }

    public class SplunkSearch : ISplunkSearch
    {
        private ISplunkSettings _settings;

        static SplunkSearch()
        {
            //// TODO: Use WebRequestHandler.ServerCertificateValidationCallback instead
            //// 1. Instantiate a WebRequestHandler
            //// 2. Set its ServerCertificateValidationCallback
            //// 3. Instantiate a Splunk.Client.Context with the WebRequestHandler
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public SplunkSearch(ISplunkSettings settings)
        {
            _settings = settings;
        }

        public async Task<List<ISpunkSearchLog>> GetProgressMaxLevelResults(string userName)
        {
            var escapedUserName = GetEscapedUserName(userName);
            var searchString = @"search index=katarai_progress userName=" + escapedUserName + @" | stats max(progressEvent.toLevel) as ""HighestLevelAchieved"", max(progressEvent.kataDurationInSeconds) as ""TotalDuration""  by userName, attemptName, kataStartTime | sort -kataStartTime | head 60";
            return await GetTask(searchString);
        }

        public async Task<List<ISpunkSearchLog>> GetCompletedProgressResults(string userName)
        {
            var escapedUserName = GetEscapedUserName(userName);
            var searchString = @"search index=katarai_progress progressEvent.kataCompleted=true  userName=" + escapedUserName + @" | stats max(progressEvent.toLevel) as ""HighestLevelAchieved"", max(progressEvent.kataDurationInSeconds) as ""TotalDuration""  by userName, attemptName, kataStartTime | sort -kataStartTime | head 60";
            return await GetTask(searchString);
        }

        public async Task<List<ISpunkSearchLog>> GetCompletedResults(string userName)
        {
            var escapedUserName = GetEscapedUserName(userName);
            var searchString = @"search index=katarai overallAnalysisResult.kataCompleted=true userName=" + escapedUserName + @" | stats max(playerImplementationRunResult.level) as ""HighestLevelAchieved"", max(timestamp) as ""timestamp"" by userName, attemptName, kataStartTime  | sort -kataStartTime | head 60";
            return await GetTask(searchString);
        }

        public async Task<List<ISpunkSearchLog>> GetMaxLevelResults(string userName)
        {
            var escapedUserName = GetEscapedUserName(userName);
            var searchString = @"search index=katarai userName=" + escapedUserName +
                               @" | stats max(playerImplementationRunResult.level) as ""HighestLevelAchieved"", max(timestamp) as ""timestamp"" by userName, attemptName, kataStartTime  | sort -kataStartTime | head 20 ";
            return await GetTask(searchString);
        }

        private static string GetEscapedUserName(string userName)
        {
            return userName.Replace("\\", "\\\\");
        }

        private async Task<List<ISpunkSearchLog>> GetTask(string searchString)
        {
            try
            {
                var service = new Service(Scheme.Https, _settings.Server, _settings.Port);
                await service.LogOnAsync(_settings.Username, _settings.Password);
                return await GetSpunkSearchLogs(service, searchString);
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
            }
            return null;
        }

        private async Task<List<ISpunkSearchLog>> GetSpunkSearchLogs(Service service, string searchString)
        {
            var searchJob = await service.SearchAsync(searchString);
            var searchResultStream = await searchJob.GetSearchResultsAsync();
            var searchResults = searchResultStream.ToList();
            var spunkSearchLogs = new List<ISpunkSearchLog>();
            foreach (var searchResult in searchResults)
            {
                var spunkSearchLog = CreateSpunkSearchLog(searchResult);
                spunkSearchLogs.Add(spunkSearchLog);
            }
            return spunkSearchLogs;
        }

        private ISpunkSearchLog CreateSpunkSearchLog(SearchResult searchResult)
        {
            var spunkSearchLog = new SpunkSearchLog
            {
                AttemptName = searchResult.GetValue("attemptName"),
                UserName = searchResult.GetValue("userName"), 
                KataStartTime = searchResult.GetValue("kataStartTime"),
                TotalDuration = searchResult.GetValue("TotalDuration"),
                HighestLevelAchieved = searchResult.GetValue("HighestLevelAchieved"),
                Timestamp = searchResult.GetValue("timestamp"),
            };
            return spunkSearchLog;
        }
    }

}