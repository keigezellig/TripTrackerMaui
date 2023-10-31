namespace MauiApp1.Models.TripEvents
{
    public class VehicleSpeedEvent : VehicleEvent
    {
        public VehicleSpeedEvent(string tripId, string vehicleId, DateTimeOffset timestamp, double value) : base(tripId, vehicleId, timestamp, value)
        {
        }
    }
}
