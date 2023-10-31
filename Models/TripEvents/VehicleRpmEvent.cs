namespace MauiApp1.Models.TripEvents
{
    public class VehicleRpmEvent : VehicleEvent
    {
        public VehicleRpmEvent(string tripId, string vehicleId, DateTimeOffset timestamp, double value) : base(tripId, vehicleId, timestamp, value)
        {
        }
    }
}
