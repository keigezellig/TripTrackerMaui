using CoordinateSharp;

using MauiApp1.Models.TripEvents;
using MauiApp1.Services.MessageProcessing.JsonMessages;

using Microsoft.Extensions.Logging;

using UnitsNet;

namespace MauiApp1.Services.MessageProcessing.MessageProcessors;

public class FuelStopMessageProcessor : MessageProcessor<FuelStopMessage, FuelStopEvent>
{
    protected override FuelStopEvent ConvertToModel(FuelStopMessage deserializedMessage)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var pos = new Coordinate(deserializedMessage.Data.Position[0], deserializedMessage.Data.Position[1]);

        return new FuelStopEvent(
            deserializedMessage.Data.TripId,
            deserializedMessage.Data.VehicleId,
            timestamp,
            pos,
            Length.FromMeters(deserializedMessage.Data.AtDistance),
            Volume.FromLiters(deserializedMessage.Data.Quantity),
            (decimal)deserializedMessage.Data.TotalAmount,
            (decimal)deserializedMessage.Data.Price);
    }


    public FuelStopMessageProcessor(ILogger<MessageProcessor<FuelStopMessage, FuelStopEvent>> logger) : base(logger)
    {
    }
}