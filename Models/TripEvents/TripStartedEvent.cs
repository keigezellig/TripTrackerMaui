namespace MauiApp1.Models.TripEvents;

public class TripStartedEvent : Event
{
    public int Odometer { get; }
    public TripPurpose Purpose { get; }
    public Location Position { get; }


    public enum TripPurpose
    {
        Business,
        NonBusiness
    }

    public TripStartedEvent(DateTimeOffset timestamp, int odometer, TripPurpose purpose, Location position, string tripId, string vehicleId) : base(tripId, vehicleId, timestamp)
    {
        Odometer = odometer;
        Purpose = purpose;
        Position = position;
    }
}
