﻿namespace MauiApp1.Models.TripEvents
{
    public class GnssEvent : Event
    {

        public enum FixQuality
        {
            None, TwoD, ThreeD
        }
        public Location Location { get; }
        public double GpsSpeed { get; }
        public FixQuality TheFixQuality {  get; }


        public GnssEvent(string tripId, string vehicleId, DateTimeOffset timestamp, Location location, double gpsSpeed, FixQuality theFixQuality) : base(tripId,vehicleId, timestamp)
        {
            Location = location;
            GpsSpeed = gpsSpeed;
            TheFixQuality = theFixQuality;
        }
    }
}