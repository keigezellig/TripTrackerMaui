using UnitsNet;

namespace MauiApp1.Models.TripEvents;

public class FuelStopEvent : Event
{
    public Location Position { get; }
    public Length AtDistance { get; }
    public Volume Quantity { get; }
    public decimal TotalAmount { get; }
    public decimal Price { get; }


    public FuelStopEvent(string tripId, string vehicleId, DateTimeOffset timestamp, Location position, Length atDistance, Volume quantity, decimal totalAmount, decimal price) : base(tripId, vehicleId, timestamp)
    {
        Position = position;
        AtDistance = atDistance;
        Quantity = quantity;
        TotalAmount = totalAmount;
        Price = price;
    }
}
