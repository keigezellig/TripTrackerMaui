namespace TripTracker.Services.DataService
{
    public interface IDataService : IDisposable
    {
        Task Start();
        Task Stop();

        bool IsStarted { get; }
    }
}
