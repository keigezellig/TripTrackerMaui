namespace MauiApp1.Models.TripEvents
{
    public class VehicleCoolantEvent : VehicleEvent
    {
        public VehicleCoolantEvent(string tripId, string vehicleId, DateTimeOffset timestamp, double value) : base(tripId, vehicleId, timestamp, value)
        {
        }
    }
}
