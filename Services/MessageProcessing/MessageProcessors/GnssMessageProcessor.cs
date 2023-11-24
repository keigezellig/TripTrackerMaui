using CoordinateSharp;

using Microsoft.Extensions.Logging;

using TripTracker.Models.TripEvents;
using TripTracker.Services.MessageProcessing.JsonMessages;

using UnitsNet;

namespace TripTracker.Services.MessageProcessing.MessageProcessors;

public class GnssMessageProcessor : MessageProcessor<GnssDataPointMessage, GnssEvent>
{
    public GnssMessageProcessor(ILogger<MessageProcessor<GnssDataPointMessage, GnssEvent>> logger) : base(logger)
    {
    }

    protected override GnssEvent ConvertToModel(GnssDataPointMessage deserializedMessage)
    {
        var location = new Coordinate(deserializedMessage.Data.Position.Latitude,
            deserializedMessage.Data.Position.Longitude);
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var fixType = (GnssEvent.FixQuality)deserializedMessage.Data.Position.FixQuality;

        return new GnssEvent(deserializedMessage.Data.TripId,
            deserializedMessage.Data.VehicleId, timestamp, location, Speed.FromMetersPerSecond(deserializedMessage.Data.Speed), fixType);
    }
}