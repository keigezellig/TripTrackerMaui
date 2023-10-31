namespace MauiApp1.Models.TripEvents;

public class TripPausedEvent : Event
{
    public Location Position { get; }


    public TripPausedEvent(string tripId, string vehicleId, DateTimeOffset timestamp, Location position): base(tripId, vehicleId, timestamp)
    {
        Position = position;
    }
}
