using MauiApp1.Services.MessageProcessing.MessageProcessors;

using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing;

public class MessageProcessorFactory
{
    private ILogger<MessageProcessorFactory> _logger;
    private readonly IServiceProvider _serviceProvider;

    public MessageProcessorFactory(IServiceProvider serviceProvider, ILogger<MessageProcessorFactory> logger)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

    }
    public IMessageProcessor GetMessageProcessor(string message)
    {
        if (message.Contains("TRIP_STARTED"))
        {
            return _serviceProvider.GetService<TripStartedMessageProcessor>();
        }

        if (message.Contains("TRIP_STOPPED"))
        {
            return _serviceProvider.GetService<TripStoppedMessageProcessor>();
        }

        if (message.Contains("TRIP_PAUSED"))
        {
            return _serviceProvider.GetService<TripPausedMessageProcessor>();
        }

        if (message.Contains("TRIP_RESUMED"))
        {
            return _serviceProvider.GetService<TripResumedMessageProcessor>();
        }

        if (message.Contains("FUEL_STOP"))
        {
            return _serviceProvider.GetService<FuelStopMessageProcessor>();
        }

        if (message.Contains("VEHICLE_DATAPOINT"))
        {
            return _serviceProvider.GetService<VehicleMessageProcessor>();
        }

        if (message.Contains("GNSS_DATAPOINT"))
        {
            return _serviceProvider.GetService<GnssMessageProcessor>();
        }

        return _serviceProvider.GetService<UnknownMessageProcessor>();
    }
}