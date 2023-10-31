﻿using MauiApp1.Models.TripEvents;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing.MessageProcessors;

public class GnssMessageProcessor : MessageProcessor<GnssDataPointMessage, GnssEvent>
{
    public GnssMessageProcessor(ILogger<MessageProcessor<GnssDataPointMessage, GnssEvent>> logger) : base(logger)
    {
    }

    protected override GnssEvent ConvertToModel(GnssDataPointMessage deserializedMessage)
    {
        var location = new Location(deserializedMessage.Data.Position.Latitude,
            deserializedMessage.Data.Position.Longitude);
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(deserializedMessage.Timestamp);
        var fixType = (GnssEvent.FixQuality)deserializedMessage.Data.Position.FixQuality;

        return new GnssEvent( deserializedMessage.Data.TripId,
            deserializedMessage.Data.VehicleId, timestamp, location, deserializedMessage.Data.Speed, fixType);
    }
}