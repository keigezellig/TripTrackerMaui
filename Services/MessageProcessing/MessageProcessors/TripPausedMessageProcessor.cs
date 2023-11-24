using CoordinateSharp;

using Microsoft.Extensions.Logging;

using TripTracker.Models.TripEvents;
using TripTracker.Services.MessageProcessing.JsonMessages;

namespace TripTracker.Services.MessageProcessing.MessageProcessors;

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