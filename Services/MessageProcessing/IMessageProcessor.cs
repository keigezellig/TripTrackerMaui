using MauiApp1.Helpers;


namespace MauiApp1.Services.MessageProcessing;

public interface IMessageProcessor
{
    void Process(string message);
}