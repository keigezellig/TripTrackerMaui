using System.Text.Json.Serialization;

namespace MauiApp1.Services.MessageProcessing.JsonMessages;

public class TripStoppedMessage
{
    [JsonPropertyName("eventtype")]
    public string EventType { get; set; }
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    [JsonPropertyName("data")]
    public TripStoppedData Data { get; set; }

}

public class TripStoppedData
{
    [JsonPropertyName("trip_id")]
    public string TripId { get; set; }

    [JsonPropertyName("vehicle_id")]
    public string VehicleId { get; set; }

    [JsonPropertyName("new_odometer_value")]
    public double Odometer { get; set; }

    [JsonPropertyName("duration")]
    public double Duration { get; set; }

    [JsonPropertyName("distance")]
    public double Distance { get; set; }

    [JsonPropertyName("avg_speed")]
    public double AverageSpeed { get; set; }

    [JsonPropertyName("position")]
    public double[] Position { get; set; }
}