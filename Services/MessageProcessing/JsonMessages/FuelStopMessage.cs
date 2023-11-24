using System.Text.Json.Serialization;

namespace TripTracker.Services.MessageProcessing.JsonMessages;

public class FuelStopMessage
{
    [JsonPropertyName("eventtype")]
    public string EventType { get; set; }
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    [JsonPropertyName("data")]
    public FuelStopMessageData Data { get; set; }

}

public class FuelStopMessageData
{
    [JsonPropertyName("trip_id")]
    public string TripId { get; set; }

    [JsonPropertyName("vehicle_id")]
    public string VehicleId { get; set; }

    [JsonPropertyName("position")]
    public double[] Position { get; set; }

    [JsonPropertyName("at_distance")]
    public double AtDistance { get; set; }

    [JsonPropertyName("quantity_in_liters")]
    public double Quantity { get; set; }

    [JsonPropertyName("total_amount")]
    public double TotalAmount { get; set; }

    [JsonPropertyName("price_per_liter")]
    public double Price { get; set; }
}