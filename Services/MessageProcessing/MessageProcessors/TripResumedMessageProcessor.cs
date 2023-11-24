using CoordinateSharp;

using Microsoft.Extensions.Logging;

using TripTracker.Models.TripEvents;
using TripTracker.Services.MessageProcessing.JsonMessages;

namespace TripTracker.Services.MessageProcessing.MessageProcessors;

public class TripResumedMessageProcessor : MessageProcessor<TripResumedMessage, TripResumedEvent>
{

    protected override TripResumedEvent ConvertToModel(TripResumedMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var pos = new Coordinate(deserializedMessage.Data.Position[0], deserializedMessage.Data.Position[1]);
        return new TripResumedEvent(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId, timestamp,
            pos);
    }


    public TripResumedMessageProcessor(ILogger<MessageProcessor<TripResumedMessage, TripResumedEvent>> logger) : base(logger)
    {
    }
}