namespace MauiApp1.Models;

public class FuelStopModel
{
    public string TripId { get; }
    public string VehicleId { get;  }
    public DateTimeOffset Timestamp { get; }
    public Location Position { get; }
    public double AtDistance { get; }
    public double Quantity { get; }
    public decimal TotalAmount { get; }
    public decimal Price { get; }


    public FuelStopModel(string tripId, string vehicleId, DateTimeOffset timestamp, Location position, double atDistance, double quantity, decimal totalAmount, decimal price)
    {
        TripId = tripId;
        VehicleId = vehicleId;
        Timestamp = timestamp;
        Position = position;
        AtDistance = atDistance;
        Quantity = quantity;
        TotalAmount = totalAmount;
        Price = price;
    }
}
