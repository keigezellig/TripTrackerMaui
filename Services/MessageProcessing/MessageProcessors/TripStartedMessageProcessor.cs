using CoordinateSharp;

using Microsoft.Extensions.Logging;

using TripTracker.Models.TripEvents;
using TripTracker.Services.MessageProcessing.JsonMessages;

using UnitsNet;

namespace TripTracker.Services.MessageProcessing.MessageProcessors;

public class TripStartedMessageProcessor : MessageProcessor<TripStartedMessage, TripStartedEvent>
{

    protected override TripStartedEvent ConvertToModel(TripStartedMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var location = new Coordinate(deserializedMessage.Data.Position[0], deserializedMessage.Data.Position[1]);
        return new TripStartedEvent(timestamp, Length.FromMeters(deserializedMessage.Data.Odometer),
            (TripStartedEvent.TripPurpose)deserializedMessage.Data.TripType, location, deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId);
    }

    public TripStartedMessageProcessor(ILogger<MessageProcessor<TripStartedMessage, TripStartedEvent>> logger) : base(logger)
    {
    }
}