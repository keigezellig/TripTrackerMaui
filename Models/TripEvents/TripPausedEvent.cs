using CoordinateSharp;

namespace MauiApp1.Models.TripEvents;

public class TripPausedEvent : Event
{
    public Coordinate Position { get; }


    public TripPausedEvent(string tripId, string vehicleId, DateTimeOffset timestamp, Coordinate position): base(tripId, vehicleId, timestamp)
    {
        Position = position;
    }
}
