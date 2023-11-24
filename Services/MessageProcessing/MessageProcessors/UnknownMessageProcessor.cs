using Microsoft.Extensions.Logging;

namespace TripTracker.Services.MessageProcessing.MessageProcessors;

public class UnknownMessageProcessor : IMessageProcessor
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