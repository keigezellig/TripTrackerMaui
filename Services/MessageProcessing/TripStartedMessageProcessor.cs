using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.Messaging;
using MauiApp1.Models;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing;

public class TripStartedMessageProcessor : MessageProcessor<TripStartedMessage, TripStartedModel>
{
    
    protected override TripStartedModel ConvertToModel(TripStartedMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var location = new Location(deserializedMessage.Data.Position[0], deserializedMessage.Data.Position[1]);
        return new TripStartedModel(timestamp, deserializedMessage.Data.Odometer,
            (TripStartedModel.TripPurpose)deserializedMessage.Data.TripType, location, deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId); 
    }

    public TripStartedMessageProcessor(ILogger<MessageProcessor<TripStartedMessage, TripStartedModel>> logger) : base(logger)
    {
    }
}