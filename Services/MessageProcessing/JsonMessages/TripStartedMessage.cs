using System.Text.Json.Serialization;

namespace TripTracker.Services.MessageProcessing.JsonMessages;

public class TripStartedMessage
{
    [JsonPropertyName("eventtype")]
    public string EventType { get; set; }
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    [JsonPropertyName("data")]
    public TripStartedData Data { get; set; }

}

public class TripStartedData
{
    [JsonPropertyName("trip_id")]
    public string TripId { get; set; }

    [JsonPropertyName("vehicle_id")]
    public string VehicleId { get; set; }

    [JsonPropertyName("odometer_reading")]
    public int Odometer { get; set; }

    [JsonPropertyName("trip_type")]
    public int TripType { get; set; }

    [JsonPropertyName("position")]
    public double[] Position { get; set; }
}