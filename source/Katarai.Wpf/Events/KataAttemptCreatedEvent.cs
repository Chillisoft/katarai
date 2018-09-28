namespace Katarai.Wpf.Events
{
    public class KataAttemptCreatedEvent
    {
        public IKataAttempt KataAttempt { get; private set; }
        public KataAttemptCreatedEvent(IKataAttempt kataAttempt)
        {
            KataAttempt = kataAttempt;
        }
    }
}