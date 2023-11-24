using UnitsNet;

namespace TripTracker.Models.TripEvents
{
    public class VehicleCoolantEvent : Event
    {
        public Temperature Value { get; }
        public VehicleCoolantEvent(string tripId, string vehicleId, DateTimeOffset timestamp, Temperature value) : base(tripId, vehicleId, timestamp)
        {
            Value = value;
        }
    }
}
