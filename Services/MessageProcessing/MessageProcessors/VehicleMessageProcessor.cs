using MauiApp1.Models.TripEvents;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing.MessageProcessors;

public class VehicleMessageProcessor : MessageProcessor<VehicleDataPointMessage, VehicleEvent>
{
    
    
    protected override VehicleEvent ConvertToModel(VehicleDataPointMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        switch (deserializedMessage.Data.Quantity)
        {
            case "speed":
                return new VehicleSpeedEvent(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId, timestamp,
                    deserializedMessage.Data.Value);
            case "rpm":
                return new VehicleRpmEvent(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId,timestamp,
                    deserializedMessage.Data.Value);
            case "coolant_temp":
                return new VehicleCoolantEvent(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId, timestamp,
                    deserializedMessage.Data.Value);
        }

        return new VehicleUnknownEvent(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId, timestamp, 
            deserializedMessage.Data.Value, deserializedMessage.Data.Quantity, deserializedMessage.Data.Unit);


    }

    public VehicleMessageProcessor(ILogger<MessageProcessor<VehicleDataPointMessage, VehicleEvent>> logger) : base(logger)
    {
    }
}