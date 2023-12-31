﻿using UnitsNet;

namespace TripTracker.Models.TripEvents
{
    public class VehicleRpmEvent : Event
    {
        public RotationalSpeed Value { get; }
        public VehicleRpmEvent(string tripId, string vehicleId, DateTimeOffset timestamp, RotationalSpeed value) : base(tripId, vehicleId, timestamp)
        {
            Value = value;
        }
    }
}
