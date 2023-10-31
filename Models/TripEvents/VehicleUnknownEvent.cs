namespace MauiApp1.Models.TripEvents;

public class VehicleUnknownEvent : VehicleEvent
{

    public string Unit { get; }

    public string Quantity { get; }

    public VehicleUnknownEvent(string tripId, string vehicleId, DateTimeOffset timestamp, double value,  string quantity, string unit) : base(
        tripId, vehicleId, timestamp, value)
    {
        Quantity = quantity;
        Unit = unit;
    }
}