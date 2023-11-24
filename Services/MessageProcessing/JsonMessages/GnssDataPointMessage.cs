using System.Text.Json.Serialization;

namespace MauiApp1.Services.MessageProcessing.JsonMessages;

public class GnssDataPointMessage
{

    [JsonPropertyName("eventtype")]
    public string EventType { get; set; }
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    [JsonPropertyName("data")]
    public GnssDataPointData Data { get; set; }

}

public class GnssDataPointData
{
    [JsonPropertyName("trip_id")]
    public string TripId { get; set; }

    [JsonPropertyName("vehicle_id")]
    public string VehicleId { get; set; }

    [JsonPropertyName("t")]
    public int Timestamp { get; set; }

    [JsonPropertyName("v")]
    public double Speed { get; set; }

    [JsonPropertyName("p")]
    public PositionData Position { get; set; }


}

public class PositionData
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
    [JsonPropertyName("fixtype")]
    public int FixQuality { get; set; }

}
