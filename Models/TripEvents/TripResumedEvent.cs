namespace MauiApp1.Models.TripEvents;

public class TripResumedEvent : Event
{
    public Location Position { get; }


    public TripResumedEvent(string tripId, string vehicleId, DateTimeOffset timestamp, Location position) : base(tripId, vehicleId, timestamp)
    {
        Position = position;
    }
}
