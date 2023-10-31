using MauiApp1.Models.TripEvents;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing.MessageProcessors;

public class TripPausedMessageProcessor : MessageProcessor<TripPausedMessage, TripPausedModel>
{

    protected override TripPausedModel ConvertToModel(TripPausedMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var pos = new Location(deserializedMessage.Data.Position[0], deserializedMessage.Data.Position[1]);
        return new TripPausedModel(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId, timestamp,
            pos);
    }


    public TripPausedMessageProcessor(ILogger<MessageProcessor<TripPausedMessage, TripPausedModel>> logger) : base(logger)
    {
    }
}