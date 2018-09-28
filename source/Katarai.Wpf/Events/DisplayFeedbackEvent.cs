namespace Katarai.Wpf.Events
{
    public class DisplayFeedbackEvent
    {
        public string Message { get; private set; }
        public DisplayFeedbackEvent(string message)
        {
            Message = message;
        }
    }
}