using System;
using System.Collections.Generic;
using System.Linq;
using Katarai.Controls;
using Katarai.Runner;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf.Monitor
{
    public class KataProgressEventArgs : EventArgs
    {
        public ProgressEvent ProgressEvent { get; private set; }

        public KataProgressEventArgs(ProgressEvent progressEvent)
        {
            ProgressEvent = progressEvent;
        }
    }

    public interface IAttemptGameState
    {
        bool IsPlayerTestStateGreen();
        string KataName { get; }
        void SetAttemptAbandoned();
    }

    public class AttemptGameState : IAttemptGameState
    {
        private readonly IKataHelper _kataHelper;
        public event EventHandler KataStarted;
        public event EventHandler KataCompleted;
        public event EventHandler<KataProgressEventArgs> KataProgress;

        private Result _previousResult;
        private List<Result> _results;
        private readonly IKataTimer _kataTimer;
        private Result _latestResult;
        public string KataName { get; set; }

        public Result LatestResult
        {
            get { return _latestResult; }
            internal set
            {
                _previousResult = LatestResult;
                _latestResult = value;
            }
        }

        public AttemptGameState(List<Result> results, IKataHelper kataHelper)
        {
            if (kataHelper == null) throw new ArgumentNullException("kataHelper");
            _kataHelper = kataHelper;
            _results = results ?? new List<Result>();
            _kataTimer = new KataTimer();
        }

        public IKataTimer KataTimer
        {
            get { return _kataTimer; }
        }

        public int KataMaxLevel
        {
            get
            {
                var kataName = this.KataName;
                if (kataName != null) return _kataHelper.GetKataMaxLevel(kataName.Replace(" ", ""));
                return 0;
            }
        }

        private void SetKataStarted()
        {
            _kataTimer.StartTimer();
            if (KataStarted != null)
            {
                KataStarted(this, new EventArgs());
            }
        }

        private void SetKataCompleted()
        {
            _kataTimer.StopTimer();
            if (KataCompleted != null)
            {
                KataCompleted(this, new EventArgs());
            }
        }

        private void SetKataProgress(ProgressEvent progressEvent)
        {
            if (KataProgress != null)
            {
                KataProgress(this, new KataProgressEventArgs(progressEvent));
            }
        }

        public void SetLatestResult(Result result)
        {
            _results.Add(result);
            _previousResult = LatestResult;
            LatestResult = result;
            CheckKataStarted();
            CheckKataCompleted();
            CheckKataProgress();
        }

        public void SetAttemptAbandoned()
        {
            var progressEvent = CreateProgressEvent(this.LatestResult, _results, this.LatestResult);
            progressEvent.DurationInSeconds = (int) ((DateTime.Now - this.LatestResult.Time).TotalMilliseconds/1000);
            progressEvent.KataAbandoned = true;
            SetKataProgress(progressEvent);
        }

        internal ProgressEvent CheckImplementationLevel()
        {
            if (!LatestResult.IsPlayerTestStateGreen()) return null;
            var currentLevel = LatestResult.PlayerImplementationLevel;
            var passingResults = _results.Where(result => result.IsPlayerTestStateGreen()).ToList();
            if (passingResults.Count(result => result.PlayerImplementationLevel == currentLevel) > 1 &&
                !IsKataCompleted())
            {
                return null;
            }
            var previousPassingLevelResult = passingResults
                .Where(result => result != LatestResult)
                .OrderByDescending(result => result.PlayerImplementationLevel)
                .ThenBy(result => result.Time)
                .FirstOrDefault(result => result.PlayerImplementationLevel < currentLevel);
            if (previousPassingLevelResult == null) return null;
            return CreateProgressEvent(LatestResult, _results, previousPassingLevelResult);
        }

        private ProgressEvent CreateProgressEvent(Result result, List<Result> results,
            Result previousImplementationResult)
        {
            var implementationlevels = GetImplementationLevels(results).ToArray();
            SetAllLevelsCompletedOnPlayerFeedback(result, implementationlevels);
            var progressEvent = CreateProgressEvent(result, previousImplementationResult, implementationlevels);
            if (progressEvent.KataCompleted)
            {
                progressEvent.KataCompletedTime = KataTimer.KataEndTime.GetValueOrDefault();
            }

            var kataStartTime = KataTimer.StartTime;
            var kataTimeNow = DateTime.Now;
            if (progressEvent.KataCompleted)
            {
                kataTimeNow = progressEvent.KataCompletedTime;
            }
            progressEvent.KataDurationInSeconds = (int)((kataTimeNow - kataStartTime).TotalMilliseconds / 1000);
            progressEvent.KataStartTime = kataStartTime;
            return progressEvent;
        }

        private ProgressEvent CreateProgressEvent(Result result, Result previousImplementationResult,
            int[] implementationlevels)
        {
            var progressEvent = new ProgressEvent
            {
                DurationInSeconds = (int) ((result.Time - previousImplementationResult.Time).TotalMilliseconds/1000),
                FromLevel = previousImplementationResult.PlayerImplementationLevel,
                ToLevel = result.PlayerImplementationLevel,
                FromLevelDescription = previousImplementationResult.PlayerImplementationRunResult.StepShouldDo,
                ToLevelDescription = result.PlayerImplementationRunResult.StepShouldDo,
                KataCompleted = result.PlayerFeedback.KataCompleted,
                AllLevelsCompleted = result.PlayerFeedback.AllLevelsCompleted,
                LevelsCompleted = string.Join(",", implementationlevels)
            };
            return progressEvent;
        }

        private void SetAllLevelsCompletedOnPlayerFeedback(Result result, int[] implementationlevels)
        {
            if (result.PlayerFeedback.KataCompleted)
            {
                var allLevelsCompleted = GetAllLevelsCompleted(KataMaxLevel, implementationlevels);
                result.PlayerFeedback.AllLevelsCompleted = allLevelsCompleted;
            }
            else
            {
                var allLevelsCompleted = GetAllLevelsCompleted(result.PlayerImplementationLevel, implementationlevels);
                result.PlayerFeedback.AllLevelsCompleted = allLevelsCompleted;
            }
        }

        private bool GetAllLevelsCompleted(int level, int[] implementationlevels)
        {
            var levels = implementationlevels.Where(i => i >= 2 && i <= level);
            var allLevelsCompleted = levels.Count() == level - 1;
            return allLevelsCompleted;
        }

        private IEnumerable<int> GetImplementationLevels(List<Result> results)
        {
            var testStates = results.Select(result1 => result1.PlayerTestLevel).Distinct().ToArray();
            var greenStates =
                results.Where(result1 => result1.IsPlayerTestStateGreen())
                    .Select(result1 => result1.PlayerImplementationLevel)
                    .Distinct()
                    .ToArray();
            return greenStates.Intersect(testStates);
        }

        private Result GetFirstResult(List<Result> results)
        {
            var firstResult =
                results.FirstOrDefault(result => result.PlayerTestLevel == 1 && result.IsPlayerTestStateGreen());
            if (firstResult == null)
            {
                firstResult =
                    results.FirstOrDefault(result => result.PlayerTestLevel == 2 && !result.IsPlayerTestStateGreen());
            }
            return firstResult;
        }

        public bool IsWarningIcon()
        {
            var result = LatestResult;
            var isRedIcon = (result.PlayerImplementationLevel > result.PlayerTestLevel ||
                             result.PlayerTestLevel > (result.PlayerImplementationLevel + 1) ||
                             HasImplementationBeenWrittenWithoutFailingTest());
            return isRedIcon;
        }

        public bool IsPlayerTestStateGreen()
        {
            return LatestResult.IsPlayerTestStateGreen();
        }

        private bool PlayerTestStateHasNotChanged()
        {
            var result = LatestResult;
            var previousResult = _previousResult;
            return result.PlayerFeedback.PlayerTestState.Equals(previousResult.PlayerFeedback.PlayerTestState);
        }

        private bool PlayerTestStateHasChanged()
        {
            var result = LatestResult;
            var previousResult = _previousResult;
            return !result.PlayerFeedback.PlayerTestState.Equals(previousResult.PlayerFeedback.PlayerTestState);
        }

        public bool HasImplementationBeenWrittenWithoutFailingTest()
        {
            var result = LatestResult;
            var previousResult = _previousResult;

            var hasImplementationBeenWrittenWithoutFailingTest = (previousResult != null) &&
                                                                 PlayerTestStateHasNotChanged() &&
                                                                 (result.PlayerImplementationLevel >
                                                                  previousResult.PlayerImplementationLevel) &&
                                                                 (result.PlayerTestLevel >
                                                                  previousResult.PlayerTestLevel) &&
                                                                 previousResult.IsPlayerTestStateGreen();
            if (hasImplementationBeenWrittenWithoutFailingTest)
            {
                LatestResult = previousResult;
            }
            return hasImplementationBeenWrittenWithoutFailingTest;
        }

        public bool HasTestBeenWrittenWithoutAPassingTestForPreviousImplementation()
        {
            var result = LatestResult;
            var previousResult = _previousResult;

            var hasTestBeenWrittenWithoutRunningTestsForPreviousImplementation = (previousResult != null) &&
                                                                 PlayerTestStateHasNotChanged() &&
                                                                 (result.PlayerImplementationLevel >
                                                                  previousResult.PlayerImplementationLevel) &&
                                                                 (result.PlayerTestLevel >
                                                                  previousResult.PlayerTestLevel) &&
                                                                 previousResult.IsPlayerTestStateRed();
            if (hasTestBeenWrittenWithoutRunningTestsForPreviousImplementation)
            {
                LatestResult = previousResult;
            }
            return hasTestBeenWrittenWithoutRunningTestsForPreviousImplementation;
        }

        public bool HasTestBeenWrittenWithoutAFailingTestForPreviousImplementation()
        {
            var result = LatestResult;
            var previousResult = _previousResult;

            var hasPlayerWrittenATestWithoutRunningTestsForPreviousImplementation = (previousResult != null) &&
                                                                                    PlayerTestStateHasChanged() &&
                                                                                    (result.PlayerImplementationLevel >
                                                                                     previousResult
                                                                                         .PlayerImplementationLevel) &&
                                                                                    (result.PlayerTestLevel >
                                                                                     previousResult.PlayerTestLevel);
            if (hasPlayerWrittenATestWithoutRunningTestsForPreviousImplementation)
            {
                LatestResult = previousResult;
            }

            return hasPlayerWrittenATestWithoutRunningTestsForPreviousImplementation;
        }

        public bool KataTimerHasNotStarted()
        {
            var kataTimerHasNotStarted = LatestResult.PlayerTestLevel > 2 && KataTimer.StartTime == DateTime.MinValue;
            if (kataTimerHasNotStarted) _results.Remove(LatestResult);
            return kataTimerHasNotStarted;
        }

        public bool ImplementationLevelHasFallen()
        {
            var result = LatestResult;
            var previousResult = _previousResult;

            if (previousResult == null) return false;
            var currentImplementationLevel = result.PlayerImplementationLevel;
            var previousResultImplementationLevel = previousResult.PlayerImplementationLevel;
            var implementationLevelHasFallen = previousResultImplementationLevel > currentImplementationLevel &&
                                               result.IsPlayerTestStateRed();
            return implementationLevelHasFallen;
        }

        private bool IsKataCompleted()
        {
            return LatestResult.PlayerFeedback.KataCompleted;
        }

        private void CheckKataStarted()
        {
            var results = _results;
            var result = LatestResult;
            if ((result.PlayerTestLevel == 1 || result.PlayerTestLevel == 2) && results.Count == 1)
            {
                SetKataStarted();
            }
        }

        private void CheckKataCompleted()
        {
            if (IsKataCompleted())
            {
                SetKataCompleted();
            }
        }

        private void CheckKataProgress()
        {
            var progressEvent = CheckImplementationLevel();
            if (progressEvent != null)
            {
                SetKataProgress(progressEvent);
            }
        }
    }
}