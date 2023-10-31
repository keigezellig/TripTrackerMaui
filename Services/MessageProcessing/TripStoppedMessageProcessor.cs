using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.Messaging;
using MauiApp1.Models;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing;

public class TripStoppedMessageProcessor : MessageProcessor<TripStoppedMessage, TripStoppedModel>
{
    
    protected override TripStoppedModel ConvertToModel(TripStoppedMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var location = new Location(deserializedMessage.Data.Position[0], deserializedMessage.Data.Position[1]);
        var duration = TimeSpan.FromSeconds(deserializedMessage.Data.Duration);
        return new TripStoppedModel(timestamp, (int)deserializedMessage.Data.Odometer, location,
            deserializedMessage.Data.TripId,
            deserializedMessage.Data.VehicleId, duration, deserializedMessage.Data.AverageSpeed,
            (int)deserializedMessage.Data.Distance);
    }

    public TripStoppedMessageProcessor(ILogger<MessageProcessor<TripStoppedMessage, TripStoppedModel>> logger) : base(logger)
    {
    }
}