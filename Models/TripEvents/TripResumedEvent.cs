﻿using CoordinateSharp;

namespace TripTracker.Models.TripEvents;

public class TripResumedEvent : Event
{
    public Coordinate Position { get; }


    public TripResumedEvent(string tripId, string vehicleId, DateTimeOffset timestamp, Coordinate position) : base(tripId, vehicleId, timestamp)
    {
        Position = position;
    }
}
