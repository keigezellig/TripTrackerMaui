namespace MauiApp1.Models.TripEvents;

public class TripStoppedEvent : Event
{
    public Location Position { get; }
    
    public int Odometer { get; }
    
    public TimeSpan Duration { get; }
    
    public double AverageSpeed { get; }
    
    public int Distance { get; }
    
    
    
    

    public TripStoppedEvent(DateTimeOffset timestamp, int odometer, Location position, string tripId, string vehicleId, TimeSpan duration, double averageSpeed, int distance) : base(tripId, vehicleId, timestamp) 
    {
        Odometer = odometer;
        Position = position;
        Duration = duration;
        AverageSpeed = averageSpeed;
        Distance = distance;
    }

    
}
