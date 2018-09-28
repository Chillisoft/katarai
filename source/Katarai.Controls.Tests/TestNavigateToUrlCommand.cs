using NSubstitute;
using NUnit.Framework;

namespace Katarai.Controls.Tests
{
    [TestFixture]
    public class TestNavigateToUrlCommand
    {
        [Test]
        public void Construct_WithNoStartProcess_ShouldSetLauncher()
        {
            //---------------Set up test pack-------------------
           
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var command = new NavigateToUrlCommand();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ProcessLauncher>(command.Launcher);
        }

        [Test]
        public void CanExecute_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var command = CreateCommand();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var canExecute = command.CanExecute(null);
            //---------------Test Result -----------------------
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void Execute_ShouldCallStartMethodOfIStartProcess()
        {
            //---------------Set up test pack-------------------
            var launcher = Substitute.For<IStartProcess>();
            var command = CreateCommand(launcher);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            command.Execute("http://www.google.co.za");
            //---------------Test Result -----------------------
            launcher.Received().StartProcess("http://www.google.co.za");
        }

        private static NavigateToUrlCommand CreateCommand(IStartProcess launcher)
        {
            var command = new NavigateToUrlCommand(launcher);
            return command;
        }
        private static NavigateToUrlCommand CreateCommand()
        {
            var launcher = Substitute.For<IStartProcess>();
            var command = new NavigateToUrlCommand(launcher);
            return command;
        }
    }
}