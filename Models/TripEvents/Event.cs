namespace TripTracker.Models.TripEvents;

public abstract class Event
{
    public string TripId { get; protected set; }
    public string VehicleId { get; protected set; }
    public DateTimeOffset Timestamp { get; protected set; }

    protected Event(string tripId, string vehicleId, DateTimeOffset timestamp)
    {
        TripId = tripId;
        VehicleId = vehicleId;
        Timestamp = timestamp;
    }
}