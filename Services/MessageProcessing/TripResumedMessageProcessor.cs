using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.Messaging;
using MauiApp1.Models;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing;

public class TripResumedMessageProcessor : MessageProcessor<TripResumedMessage, TripResumedModel>
{

    protected override TripResumedModel ConvertToModel(TripResumedMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var pos = new Location(deserializedMessage.Data.Position[0], deserializedMessage.Data.Position[1]);
        return new TripResumedModel(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId, timestamp,
            pos);
    }


    public TripResumedMessageProcessor(ILogger<MessageProcessor<TripResumedMessage, TripResumedModel>> logger) : base(logger)
    {
    }
}