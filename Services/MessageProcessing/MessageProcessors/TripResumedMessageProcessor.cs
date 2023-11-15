using CoordinateSharp;
using MauiApp1.Models.TripEvents;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing.MessageProcessors;

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