namespace MauiApp1.Models
{
    public class GpsModel
    {
        public string TripId { get;  }
        public string VehicleId { get;  }

        public enum FixQuality
        {
            None, TwoD, ThreeD
        }
        public Location Location { get; }
        public double GpsSpeed { get; }
        public FixQuality TheFixQuality {  get; }
        

        public GpsModel(string tripId, string vehicleId, Location location, double gpsSpeed, FixQuality theFixQuality)
        {
            Location = location;
            GpsSpeed = gpsSpeed;
            TripId = tripId;
            VehicleId = vehicleId;
            TheFixQuality = theFixQuality;
        }
    }
}
