using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.Messaging;
using MauiApp1.Models;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing;

public class VehicleMessageProcessor : MessageProcessor<VehicleDataPointMessage, VehicleModel>
{
    
    
    protected override VehicleModel ConvertToModel(VehicleDataPointMessage deserializedMessage)
    {
        switch (deserializedMessage.Data.Quantity)
        {
            case "speed":
                return new VehicleSpeedModel(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId,
                    deserializedMessage.Data.Value);
            case "rpm":
                return new VehicleRpmModel(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId,
                    deserializedMessage.Data.Value);
            case "coolant_temp":
                return new VehicleCoolantTempModel(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId,
                    deserializedMessage.Data.Value);
        }

        return new VehicleUnknownModel(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId, 
            deserializedMessage.Data.Value, deserializedMessage.Data.Quantity, deserializedMessage.Data.Unit);


    }

    public VehicleMessageProcessor(ILogger<MessageProcessor<VehicleDataPointMessage, VehicleModel>> logger) : base(logger)
    {
    }
}