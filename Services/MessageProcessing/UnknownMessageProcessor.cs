using MetroLog;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing;

public class UnknownMessageProcessor : IExternalMessageProcessor
{

    private readonly ILogger<UnknownMessageProcessor> _logger;

    public UnknownMessageProcessor(ILogger<UnknownMessageProcessor> logger)
    {
        _logger = logger;
    }

    public void Process(string message)
    {
        _logger.LogWarning("Unsupported message. Ignoring");
    }

    
}