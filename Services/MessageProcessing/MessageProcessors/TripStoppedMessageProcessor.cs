using CoordinateSharp;

using Microsoft.Extensions.Logging;

using TripTracker.Models.TripEvents;
using TripTracker.Services.MessageProcessing.JsonMessages;

using UnitsNet;

namespace TripTracker.Services.MessageProcessing.MessageProcessors;

public class TripStoppedMessageProcessor : MessageProcessor<TripStoppedMessage, TripStoppedEvent>
{

    protected override TripStoppedEvent ConvertToModel(TripStoppedMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var location = new Coordinate(deserializedMessage.Data.Position[0], deserializedMessage.Data.Position[1]);
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