using CoordinateSharp;

using UnitsNet;

namespace TripTracker.Models.TripEvents;

public class FuelStopEvent : Event
{
    public Coordinate Position { get; }
    public Length AtDistance { get; }
    public Volume Quantity { get; }
    public decimal TotalAmount { get; }
    public decimal Price { get; }


    public FuelStopEvent(string tripId, string vehicleId, DateTimeOffset timestamp, Coordinate position, Length atDistance, Volume quantity, decimal totalAmount, decimal price) : base(tripId, vehicleId, timestamp)
    {
        Position = position;
        AtDistance = atDistance;
        Quantity = quantity;
        TotalAmount = totalAmount;
        Price = price;
    }
}
