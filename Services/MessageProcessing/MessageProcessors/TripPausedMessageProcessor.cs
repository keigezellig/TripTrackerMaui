using CoordinateSharp;

using MauiApp1.Models.TripEvents;
using MauiApp1.Services.MessageProcessing.JsonMessages;

using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing.MessageProcessors;

public class TripPausedMessageProcessor : MessageProcessor<TripPausedMessage, TripPausedEvent>
{

    protected override TripPausedEvent ConvertToModel(TripPausedMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var pos = new Coordinate(deserializedMessage.Data.Position[0], deserializedMessage.Data.Position[1]);
        return new TripPausedEvent(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId, timestamp,
            pos);
    }


    public TripPausedMessageProcessor(ILogger<MessageProcessor<TripPausedMessage, TripPausedEvent>> logger) : base(logger)
    {
    }
}