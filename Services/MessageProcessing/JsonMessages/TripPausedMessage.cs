﻿using System.Text.Json.Serialization;

namespace TripTracker.Services.MessageProcessing.JsonMessages;

public class TripPausedMessage
{
    [JsonPropertyName("eventtype")]
    public string EventType { get; set; }
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    [JsonPropertyName("data")]
    public TripPausedData Data { get; set; }

}

public class TripPausedData
{
    [JsonPropertyName("trip_id")]
    public string TripId { get; set; }

    [JsonPropertyName("vehicle_id")]
    public string VehicleId { get; set; }

    [JsonPropertyName("position")]
    public double[] Position { get; set; }
}