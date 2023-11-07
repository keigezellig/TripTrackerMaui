using UnitsNet;

namespace MauiApp1.Models.TripEvents;

public class TripStartedEvent : Event
{
    public Length Odometer { get; }
    public TripPurpose Purpose { get; }
    public Location Position { get; }


    public enum TripPurpose
    {
        Business,
        NonBusiness
    }

    public TripStartedEvent(DateTimeOffset timestamp, Length odometer, TripPurpose purpose, Location position, string tripId, string vehicleId) : base(tripId, vehicleId, timestamp)
    {
        Odometer = odometer;
        Purpose = purpose;
        Position = position;
    }
}
