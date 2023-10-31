namespace MauiApp1.Models.TripEvents
{
    public abstract class VehicleEvent : Event
    {
        public double Value { get; }
                

        protected VehicleEvent(string tripId, string vehicleId, DateTimeOffset timestamp, double value): base(tripId, vehicleId, timestamp)
        {
            Value = value;
            TripId = tripId;
            VehicleId = vehicleId;
        }

        
    }
}
