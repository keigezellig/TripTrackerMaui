namespace MauiApp1.Models.TripEvents;

public class VehicleUnknownModel : VehicleModel
{

    public string Unit { get; }

    public string Quantity { get; }

    public VehicleUnknownModel(string tripId, string vehicleId, double value, string quantity, string unit) : base(
        tripId, vehicleId, value)
    {
        Quantity = quantity;
        Unit = unit;
    }
}