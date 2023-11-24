namespace TripTracker.Services.MessageProcessing;

public interface IMessageProcessor
{
    void Process(string message);
}