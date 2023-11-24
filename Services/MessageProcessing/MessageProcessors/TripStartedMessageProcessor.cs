using CoordinateSharp;

using MauiApp1.Models.TripEvents;
using MauiApp1.Services.MessageProcessing.JsonMessages;

using Microsoft.Extensions.Logging;

using UnitsNet;

namespace MauiApp1.Services.MessageProcessing.MessageProcessors;

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