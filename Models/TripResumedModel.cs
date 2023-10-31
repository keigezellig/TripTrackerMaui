namespace MauiApp1.Models;

public class TripResumedModel
{
    public string TripId { get; }
    public string VehicleId { get;  }
    public DateTimeOffset Timestamp { get; }
    public Location Position { get; }


    public TripResumedModel(string tripId, string vehicleId, DateTimeOffset timestamp, Location position)
    {
        TripId = tripId;
        VehicleId = vehicleId;
        Timestamp = timestamp;
        Position = position;
    }
}
