namespace MauiApp1.Models
{
    public abstract class VehicleModel
    {
        public string TripId { get; }
        public string VehicleId { get; }
        
        public double Value { get; }
                

        protected VehicleModel(string tripId, string vehicleId, double value)
        {
            Value = value;
            TripId = tripId;
            VehicleId = vehicleId;
        }

        
    }
}
