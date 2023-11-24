using CoordinateSharp;

using UnitsNet;

namespace TripTracker.Models.TripEvents
{
    public class GnssEvent : Event
    {

        public enum FixQuality
        {
            None, TwoD, ThreeD
        }
        public Coordinate Location { get; }
        public Speed GpsSpeed { get; }
        public FixQuality TheFixQuality { get; }


        public GnssEvent(string tripId, string vehicleId, DateTimeOffset timestamp, Coordinate location, Speed gpsSpeed, FixQuality theFixQuality) : base(tripId, vehicleId, timestamp)
        {
            Location = location;
            GpsSpeed = gpsSpeed;
            TheFixQuality = theFixQuality;
        }
    }
}
