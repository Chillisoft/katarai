using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Katarai.Wpf.Utils;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace Katarai.Wpf.Tests
{
    [TestFixture]
    public class TestLogRepository
    {
        [Test]
        public void Construct_GivenNullSplunkSearch_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new LogRepository(null, Substitute.For<IKataHelper>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("splunkSearch", exception.ParamName);
        }

        [Test]
        public void Construct_GivenNullKataHelper_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new LogRepository(Substitute.For<ISplunkSearch>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("kataHelper", exception.ParamName);
        }
        
        [Test]
        public void GetKataAttemptLogs_GivenNullUserName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Test Result -----------------------
            //---------------Execute Test ----------------------
            Assert.That(async () => await ThrowAsyncWhenFetchingKataAttemptLogsWithNullOrEmptyUserName(null), Throws.TypeOf<ArgumentNullException>());

        }

        [Test]
        public void GetKataAttemptLogs_GivenEmptyUserName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Test Result -----------------------
            //---------------Execute Test ----------------------
            Assert.That(async () => await ThrowAsyncWhenFetchingKataAttemptLogsWithNullOrEmptyUserName(""), Throws.TypeOf<ArgumentNullException>());
        }

        private async Task<List<IAttemptLog>> ThrowAsyncWhenFetchingKataAttemptLogsWithNullOrEmptyUserName(string username)
        {
            var logRepository = CreateLogRepository();
            return await logRepository.GetKataAttemptLogs(username, 20);
        }

        [Test]
        public async Task GetKataAttemptLogs_GivenNoDuplicateAttemptName_ShouldMergeResults()
        {
            //---------------Set up test pack-------------------
            var userName = RandomValueGen.GetRandomString();
            
            var splunkSearch = Substitute.For<ISplunkSearch>();
            splunkSearch.GetProgressMaxLevelResults(userName).Returns(info => CreateTask(DateTime.Now.AddDays(-2)));
            splunkSearch.GetMaxLevelResults(userName).Returns(info => CreateTask(DateTime.Now.AddDays(-1)));
            var logRepository = CreateLogRepository(splunkSearch);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var attemptLogs = await logRepository.GetKataAttemptLogs(userName, 20);
            //---------------Test Result -----------------------
            Assert.IsNotNull(attemptLogs);
            Assert.AreEqual(2, attemptLogs.Count);
        }

        [Test]
        public async Task GetKataAttemptLogs_GivenDuplicateAttemptName_ShouldNotBeAddedTwice()
        {
            //---------------Set up test pack-------------------
            var userName = RandomValueGen.GetRandomString();
            var attemptName = RandomValueGen.GetRandomString();
            var splunkSearch = Substitute.For<ISplunkSearch>();
            var kataStartTime = DateTime.Now.AddDays(-2);
            splunkSearch.GetProgressMaxLevelResults(userName).Returns(info => CreateTask(kataStartTime, attemptName));
            splunkSearch.GetMaxLevelResults(userName).Returns(info => CreateTask(kataStartTime.AddDays(1), attemptName));
            var logRepository = CreateLogRepository(splunkSearch);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var attemptLogs = await logRepository.GetKataAttemptLogs(userName, 20);
            //---------------Test Result -----------------------
            Assert.IsNotNull(attemptLogs);
            Assert.AreEqual(1, attemptLogs.Count);
        }

        [Test]
        public async Task GetKataAttemptLogs_ShouldUpdateAttemptLog()
        {
            //---------------Set up test pack-------------------
            var userName = RandomValueGen.GetRandomString();
            var kataName = RandomValueGen.GetRandomString();
            var splunkSearch = Substitute.For<ISplunkSearch>();
            var kataStartTime = new DateTime(2015, 3, 19, 9, 50, 20);
            const int totalDuration = 1801;
            var spunkSearchLog = CreateSpunkSearchLogWithData(kataStartTime, RandomValueGen.GetRandomString(10, 10), totalDuration, 5);
            var kataHelper = Substitute.For<IKataHelper>();
            kataHelper.GetKataName(spunkSearchLog.AttemptName).Returns(kataName);
            kataHelper.GetKataMaxLevel(kataName).Returns(11);
            splunkSearch.GetProgressMaxLevelResults(userName).Returns(info => CreateTask(spunkSearchLog));
            splunkSearch.GetMaxLevelResults(userName).Returns(info => CreateTaskWithNoLogs());
            var logRepository = CreateLogRepository(splunkSearch, kataHelper);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var attemptLogs = await logRepository.GetKataAttemptLogs(userName, 20);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, attemptLogs.Count);
            var attemptLog = attemptLogs[0];
            Assert.AreEqual(spunkSearchLog.AttemptName, attemptLog.AttemptName);
            Assert.AreEqual(spunkSearchLog.UserName, attemptLog.UserName);
            Assert.AreEqual(spunkSearchLog.HighestLevelAchieved, attemptLog.HighestLevelAchieved.ToString());
            Assert.AreEqual(kataName, attemptLog.KataName);
            Assert.AreEqual(kataStartTime, attemptLog.AttemptDate);
            Assert.AreEqual(30.02m, attemptLog.LengthInMinutes);
            Assert.AreEqual(45.45m, attemptLog.PercentCompleted);
            Assert.IsFalse(attemptLog.Completed);
        }

        [Test]
        public async Task GetKataAttemptLogs_WhenCompleted_ShouldUpdateAttemptLogAsCompleted()
        {
            //---------------Set up test pack-------------------
            var splunkSearch = Substitute.For<ISplunkSearch>();
            var spunkSearchLog = CreateSpunkSearchLogWithData(new DateTime(2015, 3, 19, 9, 50, 20), RandomValueGen.GetRandomString(10, 10), highestLevelAchieved: 11);
            var kataHelper = Substitute.For<IKataHelper>();
            const int maxLevel = 11;
            kataHelper.GetKataMaxLevel(Arg.Any<string>()).Returns(maxLevel);
            splunkSearch.GetProgressMaxLevelResults(Arg.Any<string>()).Returns(info => CreateTask(spunkSearchLog));
            splunkSearch.GetMaxLevelResults(Arg.Any<string>()).Returns(info => CreateTaskWithNoLogs());
            var logRepository = CreateLogRepository(splunkSearch, kataHelper);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var attemptLogs = await logRepository.GetKataAttemptLogs(RandomValueGen.GetRandomString(), 20);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, attemptLogs.Count);
            var attemptLog = attemptLogs[0];
            Assert.AreEqual(maxLevel, attemptLog.HighestLevelAchieved);
            Assert.AreEqual(100m, attemptLog.PercentCompleted);
            Assert.IsTrue(attemptLog.Completed);
        }

        [Test]
        public async Task GetKataAttemptLogs_WhenMaxLevelIsZero_ShouldUpdateAttemptLogPercentCompleted()
        {
            //---------------Set up test pack-------------------
            var splunkSearch = Substitute.For<ISplunkSearch>();
            var spunkSearchLog = CreateSpunkSearchLogWithData(new DateTime(2015, 3, 19, 9, 50, 20), RandomValueGen.GetRandomString(10, 10), highestLevelAchieved: 11);
            var kataHelper = Substitute.For<IKataHelper>();
            kataHelper.GetKataMaxLevel(Arg.Any<string>()).Returns(0);
            splunkSearch.GetProgressMaxLevelResults(Arg.Any<string>()).Returns(info => CreateTask(spunkSearchLog));
            splunkSearch.GetMaxLevelResults(Arg.Any<string>()).Returns(info => CreateTaskWithNoLogs());
            var logRepository = CreateLogRepository(splunkSearch, kataHelper);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var attemptLogs = await logRepository.GetKataAttemptLogs(RandomValueGen.GetRandomString(), 20);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, attemptLogs.Count);
            var attemptLog = attemptLogs[0];
            Assert.IsNull(attemptLog.PercentCompleted);
        }

        [Test]
        public async Task GetKataAttemptLogs_WhenTotalDurationIsNull_ShouldUpdateDurationUsingTimestamp()
        {
            //---------------Set up test pack-------------------
            var splunkSearch = Substitute.For<ISplunkSearch>();
            var kataStartTime = new DateTime(2015, 3, 19, 9, 50, 20);
            var spunkSearchLog = CreateSpunkSearchLogWithData(kataStartTime, RandomValueGen.GetRandomString(10, 10));
            spunkSearchLog.TotalDuration = null;
            spunkSearchLog.Timestamp = kataStartTime.AddSeconds(1250).ToString(CultureInfo.InvariantCulture);
            splunkSearch.GetProgressMaxLevelResults(Arg.Any<string>()).Returns(info => CreateTask(spunkSearchLog));
            splunkSearch.GetMaxLevelResults(Arg.Any<string>()).Returns(info => CreateTaskWithNoLogs());
            var logRepository = CreateLogRepository(splunkSearch);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var attemptLogs = await logRepository.GetKataAttemptLogs(RandomValueGen.GetRandomString(), 20);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, attemptLogs.Count);
            var attemptLog = attemptLogs[0];
            Assert.AreEqual(20.83m, attemptLog.LengthInMinutes);
        }

        [Test]
        public async Task GetKataAttemptLogs_WhenTotalDurationIsNullAndTimeStampIsMinDate_ShouldNotUpdateDurationUsingTimestamp()
        {
            //---------------Set up test pack-------------------
            var splunkSearch = Substitute.For<ISplunkSearch>();
            var kataStartTime = new DateTime(2015, 3, 19, 9, 50, 20);
            var spunkSearchLog = CreateSpunkSearchLogWithData(kataStartTime, RandomValueGen.GetRandomString(10, 10));
            spunkSearchLog.TotalDuration = null;
            spunkSearchLog.Timestamp = DateTime.MinValue.ToString(CultureInfo.InvariantCulture);
            splunkSearch.GetProgressMaxLevelResults(Arg.Any<string>()).Returns(info => CreateTask(spunkSearchLog));
            splunkSearch.GetMaxLevelResults(Arg.Any<string>()).Returns(info => CreateTaskWithNoLogs());
            var logRepository = CreateLogRepository(splunkSearch);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var attemptLogs = await logRepository.GetKataAttemptLogs(RandomValueGen.GetRandomString(), 20);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, attemptLogs.Count);
            var attemptLog = attemptLogs[0];
            Assert.AreEqual(0m, attemptLog.LengthInMinutes);
        }

        [Test]
        public async Task GetKataAttemptLogs_GivenNullSearchLogsReturned_ShouldReturnProgressResults()
        {
            //---------------Set up test pack-------------------
            var userName = RandomValueGen.GetRandomString();
            var splunkSearch = Substitute.For<ISplunkSearch>();
            splunkSearch.GetProgressMaxLevelResults(userName).Returns(info => CreateTaskThatReturnsNull());
            splunkSearch.GetMaxLevelResults(userName).Returns(info => CreateTaskThatReturnsNull());
            var logRepository = CreateLogRepository(splunkSearch);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var attemptLogs = await logRepository.GetKataAttemptLogs(userName, 20);
            //---------------Test Result -----------------------
            Assert.IsNotNull(attemptLogs);
            Assert.AreEqual(0, attemptLogs.Count);
        }

        private static Task<List<ISpunkSearchLog>> CreateTaskThatReturnsNull()
        {
            var task = new Task<List<ISpunkSearchLog>>(() => null);
            task.Start();
            return task;
        }

        [Test]
        public async Task GetKataAttemptLogs_GivenNullLogsReturned_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var userName = RandomValueGen.GetRandomString();
            var splunkSearch = Substitute.For<ISplunkSearch>();
            splunkSearch.GetProgressMaxLevelResults(userName).Returns(info => CreateTask(DateTime.Now.AddDays(-2)));
            splunkSearch.GetMaxLevelResults(userName).Returns(info => {
                        var task = new Task<List<ISpunkSearchLog>>(() => null);
                        task.Start();
                        return task; });
            var logRepository = CreateLogRepository(splunkSearch);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var attemptLogs = await logRepository.GetKataAttemptLogs(userName, 20);
            //---------------Test Result -----------------------
            Assert.IsNotNull(attemptLogs);
            Assert.AreEqual(1, attemptLogs.Count);
        }

        private Task<List<ISpunkSearchLog>> CreateTask(SpunkSearchLog spunkSearchLog)
        {
            var task =
                new Task<List<ISpunkSearchLog>>(() => new List<ISpunkSearchLog> { spunkSearchLog });
            task.Start();
            return task;
        }

        private Task<List<ISpunkSearchLog>> CreateTask(DateTime kataStartTime, string attemptName = null)
        {
            var task =
                new Task<List<ISpunkSearchLog>>(() => new List<ISpunkSearchLog> {CreateSpunkSearchLog(kataStartTime, attemptName)});
            task.Start();
            return task;
        }

        private Task<List<ISpunkSearchLog>> CreateTaskWithNoLogs()
        {
            var task =
                new Task<List<ISpunkSearchLog>>(() => new List<ISpunkSearchLog>());
            task.Start();
            return task;
        }

        private SpunkSearchLog CreateSpunkSearchLogWithData(DateTime kataStartTime, string attemptName, int totalDuration = 2000, int highestLevelAchieved = 4)
        {
            return new SpunkSearchLog
            {
                AttemptName = attemptName,
                UserName = RandomValueGen.GetRandomString(10, 10),
                KataStartTime = kataStartTime.ToString(CultureInfo.InvariantCulture),
                TotalDuration = totalDuration.ToString(CultureInfo.InvariantCulture),
                HighestLevelAchieved = highestLevelAchieved.ToString(CultureInfo.InvariantCulture)
            };
        }

        private SpunkSearchLog CreateSpunkSearchLog(DateTime kataStartTime, string attemptName)
        {
            if (string.IsNullOrEmpty(attemptName)) attemptName = RandomValueGen.GetRandomString();
            return new SpunkSearchLog{KataStartTime = kataStartTime.ToString(CultureInfo.InvariantCulture), AttemptName = attemptName};
        }

        private LogRepository CreateLogRepository(ISplunkSearch splunkSearch = null, IKataHelper kataHelper = null)
        {
            splunkSearch = splunkSearch ?? Substitute.For<ISplunkSearch>();
            kataHelper = kataHelper ?? Substitute.For<IKataHelper>();
            return new LogRepository(splunkSearch, kataHelper);
        }
    }
}