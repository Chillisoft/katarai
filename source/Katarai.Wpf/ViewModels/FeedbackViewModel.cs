using System;
using Katarai.Wpf.Commands;
using Katarai.Wpf.Utils;

namespace Katarai.Wpf.ViewModels
{
    public interface IFeedbackViewModel : IViewModel
    {
    }

    public class FeedbackViewModel: SingleInstanceViewModel, IFeedbackViewModel
    {
        public event EventHandler RequestClose;
        public CommandWithLogging LogFeedbackCommand { get; private set; }
        public string Feedback { get; set; }

        public FeedbackViewModel(): this(new LogFeedbackCommand(new SplunkLogger(new SplunkAppender(new SplunkSettings()), new KataHelper())))
        {
        }

        public FeedbackViewModel(ILogFeedbackCommand logFeedbackCommand)
        {
            if (logFeedbackCommand == null) throw new ArgumentNullException("logFeedbackCommand");
            LogFeedbackCommand = new CommandWithLogging(logFeedbackCommand);
            ((ILogFeedbackCommand)LogFeedbackCommand.WrappedCommand).FeedbackLogged += (sender, args) =>
            {
                if (this.RequestClose != null)
                {
                    this.RequestClose(this, EventArgs.Empty);
                }
            };
        }

    }

}
