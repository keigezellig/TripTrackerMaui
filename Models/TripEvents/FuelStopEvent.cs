namespace MauiApp1.Models.TripEvents;

public class FuelStopEvent : Event
{
    public Location Position { get; }
    public double AtDistance { get; }
    public double Quantity { get; }
    public decimal TotalAmount { get; }
    public decimal Price { get; }


    public FuelStopEvent(string tripId, string vehicleId, DateTimeOffset timestamp, Location position, double atDistance, double quantity, decimal totalAmount, decimal price) : base(tripId, vehicleId, timestamp)
    {
        Position = position;
        AtDistance = atDistance;
        Quantity = quantity;
        TotalAmount = totalAmount;
        Price = price;
    }
}
