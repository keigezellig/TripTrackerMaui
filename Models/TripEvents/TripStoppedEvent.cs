using UnitsNet;

namespace MauiApp1.Models.TripEvents;

public class TripStoppedEvent : Event
{
    public Location Position { get; }
    
    public Length Odometer { get; }
    
    public TimeSpan Duration { get; }
    
    public Speed AverageSpeed { get; }
    
    public Length Distance { get; }
    
    
    
    

    public TripStoppedEvent(DateTimeOffset timestamp, Length odometer, Location position, string tripId, string vehicleId, TimeSpan duration, Speed averageSpeed, Length distance) : base(tripId, vehicleId, timestamp) 
    {
        Odometer = odometer;
        Position = position;
        Duration = duration;
        AverageSpeed = averageSpeed;
        Distance = distance;
    }

    
}
