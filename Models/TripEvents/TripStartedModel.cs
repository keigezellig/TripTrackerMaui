namespace MauiApp1.Models.TripEvents;

public class TripStartedModel
{
    public string TripId { get; }
    public string VehicleId { get;  }
    public DateTimeOffset Timestamp { get; }
    public int Odometer { get; }
    public TripPurpose Purpose { get; }
    public Location Position { get; }


    public enum TripPurpose
    {
        Business,
        NonBusiness
    }

    public TripStartedModel(DateTimeOffset timestamp, int odometer, TripPurpose purpose, Location position, string tripId, string vehicleId)
    {
        Timestamp = timestamp;
        Odometer = odometer;
        Purpose = purpose;
        Position = position;
        TripId = tripId;
        VehicleId = vehicleId;
    }
}
