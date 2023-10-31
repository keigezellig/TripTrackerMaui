using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.Messaging;
using MauiApp1.Models;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing;

public class FuelStopMessageProcessor : MessageProcessor<FuelStopMessage, FuelStopModel>
{
    protected override FuelStopModel ConvertToModel(FuelStopMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var pos = new Location(deserializedMessage.Data.Position[0], deserializedMessage.Data.Position[1]);
        
        return new FuelStopModel(
            deserializedMessage.Data.TripId, 
            deserializedMessage.Data.VehicleId, 
            timestamp,
            pos, 
            deserializedMessage.Data.AtDistance, 
            deserializedMessage.Data.Quantity,
            (decimal)deserializedMessage.Data.TotalAmount, 
            (decimal)deserializedMessage.Data.Price);
    }


    public FuelStopMessageProcessor(ILogger<MessageProcessor<FuelStopMessage, FuelStopModel>> logger) : base(logger)
    {
    }
}