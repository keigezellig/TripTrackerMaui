using System.Text.Json.Serialization;

namespace TripTracker.Services.MessageProcessing.JsonMessages;

public class VehicleDataPointMessage
{

    [JsonPropertyName("eventtype")]
    public string EventType { get; set; }
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    [JsonPropertyName("data")]
    public VehicleDataPointData Data { get; set; }

}

public class VehicleDataPointData
{
    [JsonPropertyName("trip_id")]
    public string TripId { get; set; }

    [JsonPropertyName("vehicle_id")]
    public string VehicleId { get; set; }

    [JsonPropertyName("t")]
    public int Timestamp { get; set; }

    [JsonPropertyName("q")]
    public string Quantity { get; set; }

    [JsonPropertyName("v")]
    public double Value { get; set; }

    [JsonPropertyName("u")]
    public string Unit { get; set; }


}
