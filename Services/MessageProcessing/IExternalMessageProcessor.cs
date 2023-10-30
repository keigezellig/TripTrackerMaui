using MauiApp1.Helpers;
using Microsoft.Extensions.Logging;


namespace MauiApp1.Services.MessageProcessing;

public interface IExternalMessageProcessor
{
    void Process(string message);
}

public class ExternalMessageProcessorFactory
{
    private ILogger<ExternalMessageProcessorFactory> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ExternalMessageProcessorFactory(IServiceProvider serviceProvider, ILogger<ExternalMessageProcessorFactory> logger)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

    }
    public IExternalMessageProcessor GetMessageProcessor(string message)
    {
        if (message.Contains("TRIP_STARTED"))
        {
            return _serviceProvider.GetService<TripStartedMessageProcessor>();   
            
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