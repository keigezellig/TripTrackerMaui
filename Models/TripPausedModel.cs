namespace MauiApp1.Models;

public class TripPausedModel
{
    public string TripId { get; }
    public string VehicleId { get;  }
    public DateTimeOffset Timestamp { get; }
    public Location Position { get; }


    public TripPausedModel(string tripId, string vehicleId, DateTimeOffset timestamp, Location position)
    {
        TripId = tripId;
        VehicleId = vehicleId;
        Timestamp = timestamp;
        Position = position;
    }
}
