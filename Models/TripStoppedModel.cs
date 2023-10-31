using Microsoft.Maui.Maps;

namespace MauiApp1.Models;

public class TripStoppedModel
{
    public string TripId { get; }
    public string VehicleId { get;  }
    
    public DateTimeOffset Timestamp { get; }
    public Location Position { get; }
    
    public int Odometer { get; }
    
    public TimeSpan Duration { get; }
    
    public double AverageSpeed { get; }
    
    public int Distance { get; }
    
    
    
    

    public TripStoppedModel(DateTimeOffset timestamp, int odometer, Location position, string tripId, string vehicleId, TimeSpan duration, double averageSpeed, int distance)
    {
        Timestamp = timestamp;
        Odometer = odometer;
        Position = position;
        TripId = tripId;
        VehicleId = vehicleId;
        Duration = duration;
        AverageSpeed = averageSpeed;
        Distance = distance;
    }

    
}
