using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Engine.Runners;
using Katarai.Runner;
using Katarai.Utils;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Katarai.Wpf
{
    public class SplunkLogger : ISplunkLogger
    {
        private readonly ISplunkAppender _splunkAppender;
        private readonly IKataHelper _kataHelper;

        public SplunkLogger(ISplunkAppender splunkAppender, IKataHelper kataHelper)
        {
            if (splunkAppender == null) throw new ArgumentNullException("splunkAppender");
            if (kataHelper == null) throw new ArgumentNullException("kataHelper");
            _splunkAppender = splunkAppender;
            _kataHelper = kataHelper;
        }

        public void Log(PlayerImplementationRunResult playerImplementationRunResult, 
            PlayerTestsRunResult playerTestsRunResult, 
            PlayerFeedback playerFeedback, 
            IKataraiApp kataraiApp, 
            IFileSystem fileSystem)
        {
            var playerPath = GetPlayerPath(kataraiApp);
            var attemptName = GetAttemptName(playerPath);
            if (!attemptName.Contains("-")) return;
            var jsonObject = CreateJsonObject(playerImplementationRunResult, playerTestsRunResult, playerFeedback, attemptName, kataraiApp, playerPath, fileSystem);
            _splunkAppender.Log(jsonObject, "katarai");
        }

        public void Log(ProgressEvent progressEvent, IKataraiApp kataraiApp)
        {
            var playerPath = GetPlayerPath(kataraiApp);
            var attemptName = GetAttemptName(playerPath);
            if (!attemptName.Contains("-")) return;
            var jsonObject = CreateJsonObject(progressEvent, attemptName);
            _splunkAppender.Log(jsonObject, "katarai_progress");
        }

        public void Log(MonitorEvent monitorEvent)
        {
            var jsonObject = CreateJsonObject(monitorEvent);
            _splunkAppender.Log(jsonObject, "katarai_monitor");
        }

        public void Log(CommandEvent commandEvent)
        {
            var jsonObject = CreateJsonObject(commandEvent);
            _splunkAppender.Log(jsonObject, "katarai_analytics");
        }

        public void LogFeedback(string message)
        {
            var jsonObject = JsonConvert.SerializeObject(new
            {
                userName = GetUserName(),
                timestamp = GetTimestamp(),
                feedback = message,
                appVersion = GetAppVersion(),
            }, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
            _splunkAppender.Log(jsonObject, "katarai_feedback");
        }

        private string GetPlayerPath(IKataraiApp kataraiApp)
        {
            return kataraiApp.SettingsManager.FetchCurrentSettings().PlayerPath;
        }

        private string GetAttemptName(string playerPath)
        {
            if (string.IsNullOrEmpty(playerPath)) return string.Empty;
            var pathElements = playerPath.Split('\\').ToList();
            return pathElements[pathElements.IndexOf("Katarai") + 1];
        }

        private string GetTimestamp()
        {
            return string.Format("{0:yyyy MM dd HH:mm:ss.fff}", DateTime.Now);
        }

        private string GetAppVersion()
        {
            return string.Format("Katarai.{0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        private string GetUserName()
        {
            return Environment.UserDomainName + "\\" + Environment.UserName;
        }

        private DateTime GetAttemptCreationTime(string attemptName)
        {
            return DateTime.ParseExact(attemptName.Split('-')[1], "yyyy_MM_dd_HH_mm_ss", CultureInfo.CurrentCulture);
        }

        private DateTime GetKataStartTime(DateTime attemptCreationTime, IKataraiApp kataraiApp)
        {
            var kataStartTime = attemptCreationTime;
            var kataTimer =  kataraiApp.AttemptGameState.KataTimer;
            if (kataTimer != null && kataTimer.StartTime > DateTime.MinValue)
            {
                kataStartTime = kataTimer.StartTime;
            }
            return kataStartTime;
        }

        private string CreateJsonObject(ProgressEvent progressEvent, string attemptName)
        {
            var kataStartTime = progressEvent.KataStartTime;
            var attemptCreationTime = GetAttemptCreationTime(attemptName);
            if (kataStartTime == DateTime.MinValue)
            {
                kataStartTime = attemptCreationTime;
            }
            return JsonConvert.SerializeObject(new
            {
                userName = GetUserName(),
                attemptName,
                timestamp = GetTimestamp(),
                attemptCreationTime,
                kataStartTime,
                appVersion = GetAppVersion(),
                kataName = _kataHelper.GetKataName(attemptName),
                progressEvent
            }, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        private string CreateJsonObject(PlayerImplementationRunResult playerImplementationRunResult,
            PlayerTestsRunResult playerTestsRunResult,
            PlayerFeedback playerFeedback,
            string attemptName,
            IKataraiApp kataraiApp,
            string playerPath,
            IFileSystem fileSystem)
        {
            var attemptCreationTime = GetAttemptCreationTime(attemptName);

            var kataStartTime = GetKataStartTime(attemptCreationTime, kataraiApp);
            return JsonConvert.SerializeObject(new
            {
                userName = GetUserName(),
                attemptName,
                timestamp = GetTimestamp(),
                attemptCreationTime,
                kataStartTime,
                playerImplementationRunResult,
                playerTestsRunResult,
                overallAnalysisResult = playerFeedback,
                appVersion = GetAppVersion(),
                kataName = _kataHelper.GetKataName(attemptName),
                kata = GetKataFilesContents(playerPath, fileSystem)
            }, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        private string CreateJsonObject(MonitorEvent monitorEvent)
        {
            return JsonConvert.SerializeObject(new
            {
                userName = GetUserName(),
                timestamp = GetTimestamp(),
                appVersion = GetAppVersion(),
                description = monitorEvent.Description,
                logged = string.Format("{0:yyyy MM dd HH:mm:ss.fff}", monitorEvent.Logged),
                settings = monitorEvent.Settings
            }, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        private string CreateJsonObject(CommandEvent commandEvent)
        {
            return JsonConvert.SerializeObject(new
            {
                userName = GetUserName(),
                timestamp = GetTimestamp(),
                appVersion = GetAppVersion(),
                description = commandEvent.Description,
                logged = string.Format("{0:yyyy MM dd HH:mm:ss.fff}", commandEvent.Logged),
            }, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        private KataAttemptCode GetKataFilesContents(string playerPath, IFileSystem fileSystem)
        {
            var path = playerPath.Substring(0, playerPath.IndexOf("PlayerSolution", StringComparison.Ordinal) + "PlayerSolution".Length);
            const string filter = "*.cs";

            var files = fileSystem.GetFiles(path,filter);
            var kataFiles = GetKataFiles(fileSystem, files);

            var kata = JsonConvert.SerializeObject(kataFiles, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
            return new KataAttemptCode { FilesContents = kata };
        }

        private static List<ExpandoObject> GetKataFiles(IFileSystem fileSystem, string[] files)
        {
            var kataFiles = new List<ExpandoObject>();

            foreach (var file in files)
            {
                if (file.Contains("AssemblyInfo") || file.Contains("\\bin\\") || file.Contains("\\obj\\")) continue;
                dynamic item = new ExpandoObject();
                item.Name = new FileInfo(file).Name;
                item.Contents = fileSystem.ReadAllText(file);
                kataFiles.Add(item);
            }
            return kataFiles;
        }
    }
}