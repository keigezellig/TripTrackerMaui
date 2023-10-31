namespace MauiApp1.Services.DataService
{
    public interface IDataService : IDisposable
    {
        Task Start();
        Task Stop();

        bool IsStarted { get; }
    }
}
