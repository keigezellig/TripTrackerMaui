namespace TripTracker.Models
{
    public class StatusMessage
    {
        public bool IsStarted { get; }

        public StatusMessage(bool isStarted)
        {
            IsStarted = isStarted;
        }
    }
}
