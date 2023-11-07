using MauiApp1.Models.TripEvents;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;
using UnitsNet;

namespace MauiApp1.Services.MessageProcessing.MessageProcessors;

public class TripStoppedMessageProcessor : MessageProcessor<TripStoppedMessage, TripStoppedEvent>
{
    
    protected override TripStoppedEvent ConvertToModel(TripStoppedMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var location = new Location(deserializedMessage.Data.Position[0], deserializedMessage.Data.Position[1]);
        var duration = TimeSpan.FromSeconds(deserializedMessage.Data.Duration);
        
        return new TripStoppedEvent(
            timestamp, 
            Length.FromMeters(deserializedMessage.Data.Odometer), 
            location,
            deserializedMessage.Data.TripId,
            deserializedMessage.Data.VehicleId, 
            duration, 
            Speed.FromMetersPerSecond(deserializedMessage.Data.AverageSpeed),
            Length.FromMeters(deserializedMessage.Data.Distance));
    }

    public TripStoppedMessageProcessor(ILogger<MessageProcessor<TripStoppedMessage, TripStoppedEvent>> logger) : base(logger)
    {
    }
}