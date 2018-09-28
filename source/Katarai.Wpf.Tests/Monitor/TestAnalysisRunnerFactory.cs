using System;
using Katarai.Wpf.AnalysisContainers;
using Katarai.Wpf.Monitor;
using Katarai.Wpf.Settings;
using NSubstitute;
using NUnit.Framework;

namespace Katarai.Wpf.Tests.Monitor
{
    [TestFixture]
    public class TestAnalysisRunnerFactory
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            var analysisRunnerFactory = new AnalysisRunnerFactory();
            //---------------Test Result -----------------------
            Assert.IsNotNull(analysisRunnerFactory);
        }

        [Test]
        public void CreateAnalysisRunner_WhenNullSettingsManager_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var analysisRunnerFactory = CreateAnalysisRunnerFactory();
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            var result = Assert.Throws<ArgumentNullException>(() => analysisRunnerFactory.CreateAnalysisRunner(null, Substitute.For<IAnalysisContainer>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("settingsManager", result.ParamName);
        }

        [Test]
        public void CreateAnalysisRunner_WhenNullAnalysisContainer_ShouldThrowANE()
        {
            //---------------Set up test pack-------------------
            var analysisRunnerFactory = CreateAnalysisRunnerFactory();
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            var result = Assert.Throws<ArgumentNullException>(() => analysisRunnerFactory.CreateAnalysisRunner(Substitute.For<ISettingsManager>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("analysisContainer", result.ParamName);
        }

        [Test]
        public void CreateAnalysisRunner_ShouldReturnFileMonitor()
        {
            //---------------Set up test pack-------------------
            var analysisRunnerFactory = CreateAnalysisRunnerFactory();
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            var fileMonitor = analysisRunnerFactory.CreateAnalysisRunner(Substitute.For<ISettingsManager>(), Substitute.For<IAnalysisContainer>());
            //---------------Test Result -----------------------
            Assert.IsNotNull(fileMonitor);
        }

        private AnalysisRunnerFactory CreateAnalysisRunnerFactory()
        {
            return new AnalysisRunnerFactory();
        }

    }
}