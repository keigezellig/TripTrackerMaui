using MauiApp1.Models.TripEvents;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;
using UnitsNet;

namespace MauiApp1.Services.MessageProcessing.MessageProcessors;

public class VehicleMessageProcessor : MessageProcessor<VehicleDataPointMessage, Event>
{
    
    
    protected override Event ConvertToModel(VehicleDataPointMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        switch (deserializedMessage.Data.Quantity)
        {
            case "speed":
                return new VehicleSpeedEvent(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId, timestamp,
                    (Speed)Quantity.FromUnitAbbreviation(deserializedMessage.Data.Value, deserializedMessage.Data.Unit));
            case "rpm":
                return new VehicleRpmEvent(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId,timestamp,
                    (RotationalSpeed)Quantity.FromUnitAbbreviation(deserializedMessage.Data.Value, deserializedMessage.Data.Unit));
            case "coolant_temp":
                return new VehicleCoolantEvent(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId, timestamp,
                    (Temperature)Quantity.FromUnitAbbreviation(deserializedMessage.Data.Value, deserializedMessage.Data.Unit));;
        }

        return new VehicleUnknownEvent(deserializedMessage.Data.TripId, deserializedMessage.Data.VehicleId, timestamp, 
            deserializedMessage.Data.Value, deserializedMessage.Data.Quantity, deserializedMessage.Data.Unit);


    }

    public VehicleMessageProcessor(ILogger<MessageProcessor<VehicleDataPointMessage, Event>> logger) : base(logger)
    {
    }
}