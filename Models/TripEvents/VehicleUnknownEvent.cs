namespace MauiApp1.Models.TripEvents;

public class VehicleUnknownEvent : Event
{

    public string Unit { get; }

    public string Quantity { get; }
    
    public double Value { get; }

    public VehicleUnknownEvent(string tripId, string vehicleId, DateTimeOffset timestamp, double value,  string quantity, string unit) : base(
        tripId, vehicleId, timestamp)
    {
        Quantity = quantity;
        Unit = unit;
        Value = Value;
    }
}