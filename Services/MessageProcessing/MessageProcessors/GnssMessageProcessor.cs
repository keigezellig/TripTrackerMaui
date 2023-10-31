using MauiApp1.Models.TripEvents;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing.MessageProcessors;

public class GnssMessageProcessor : MessageProcessor<GnssDataPointMessage, GpsModel>
{
    public GnssMessageProcessor(ILogger<MessageProcessor<GnssDataPointMessage, GpsModel>> logger) : base(logger)
    {
    }

    protected override GpsModel ConvertToModel(GnssDataPointMessage deserializedMessage)
    {
        var location = new Location(deserializedMessage.Data.Position.Latitude,
            deserializedMessage.Data.Position.Longitude);
        var fixType = (GpsModel.FixQuality)deserializedMessage.Data.Position.FixQuality;

        return new GpsModel( deserializedMessage.Data.TripId,
            deserializedMessage.Data.VehicleId, location, deserializedMessage.Data.Speed, fixType);
    }
}