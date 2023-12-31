﻿using UnitsNet;

namespace TripTracker.Models.TripEvents
{
    public class VehicleSpeedEvent : Event
    {
        public Speed Value { get; }
        public VehicleSpeedEvent(string tripId, string vehicleId, DateTimeOffset timestamp, Speed value) : base(tripId, vehicleId, timestamp)
        {
            Value = value;
        }
    }
}
