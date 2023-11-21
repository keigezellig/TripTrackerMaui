using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models.TripEvents;
using CommunityToolkit.Mvvm.Messaging;
using CoordinateSharp;
using MauiApp1.Controls.MarkerMap;
using MauiApp1.Helpers;
using Microsoft.Extensions.Logging;
using UnitsNet;
using UnitsNet.Units;

namespace MauiApp1.ViewModels;


public partial class LiveDataViewModel : ObservableRecipient
{
    private readonly ILogger<LiveDataViewModel> _logger;

    [ObservableProperty] 
    private ObservableCollection<LiveDataItemViewModel> _dataItems;


    [ObservableProperty]
    private ObservableCollection<MarkerSet> _markerSets;

    [ObservableProperty] 
    private LiveDataItemViewModel _selectedTrip;


    public LiveDataViewModel(ILogger<LiveDataViewModel> logger)
    {
        _logger = logger;
        DataItems = new ObservableCollection<LiveDataItemViewModel>();
        MarkerSets = new ObservableCollection<MarkerSet>();
        
        IsActive = true;
       
    }
    

    protected override void OnActivated()
    {
        Messenger.Register<LiveDataViewModel, TripStartedEvent>(this, (r, m) => MainThread.BeginInvokeOnMainThread( () => r.UpdateData(m)));
        Messenger.Register<LiveDataViewModel, TripStoppedEvent>(this, (r, m) => MainThread.BeginInvokeOnMainThread( () => r.UpdateData(m)));
        Messenger.Register<LiveDataViewModel, TripResumedEvent>(this, (r, m) => MainThread.BeginInvokeOnMainThread( () => r.UpdateData(m)));
        Messenger.Register<LiveDataViewModel, GnssEvent>(this, (r, m) => MainThread.BeginInvokeOnMainThread( () => r.UpdateData(m)));
        Messenger.Register<LiveDataViewModel, TripPausedEvent>(this, (r, m) => MainThread.BeginInvokeOnMainThread( () => r.UpdateData(m)));
        Messenger.Register<LiveDataViewModel, FuelStopEvent>(this, (r, m) => MainThread.BeginInvokeOnMainThread( () => r.UpdateData(m)));
    }

    [RelayCommand]
    private void TripSelected()
    {
        SelectedTrip.MarkerSet.IsSelected = true;
        SelectedTrip.IsFollowingAllowed = SelectedTrip.IsFollowingAllowed && (SelectedTrip.Status != LiveDataItemViewModel.TripStatus.Finished);

    }

    partial void OnSelectedTripChanging(LiveDataItemViewModel oldValue, LiveDataItemViewModel newValue)
    {
        if (oldValue != null)
        {
            oldValue.MarkerSet.IsSelected = false;
            
        }
    }


    private void UpdateData(Event tripEvent)
    {
        var item = DataItems.FirstOrDefault(item => item.VehicleId == tripEvent.VehicleId && item.TripId == tripEvent.TripId);
        if (item == null && tripEvent is not TripStartedEvent)
        {
            return;
        }
        
        
        switch (tripEvent)
        {
            case TripStartedEvent startedEvent:
                var newItem =
                    new LiveDataItemViewModel(
                        startedEvent.VehicleId, 
                        startedEvent.TripId, 
                        startedEvent.Position, 
                        startedEvent.Timestamp);
                DataItems.Add(newItem);
                MarkerSets.Add(newItem.MarkerSet);
                newItem.MarkerSet.Markers.AddWithId(new StartMarker(startedEvent.Position, newItem.MarkerSet));

                SelectedTrip = newItem;
                
                break;
            case GnssEvent gnssEvent:
                item.CurrentLocation = gnssEvent.Location;
                SetCurrentPositionMarker(item.CurrentLocation, item.MarkerSet);
                SetCalculatedValues(item, gnssEvent);
                item.Speed = gnssEvent.GpsSpeed.ToUnit(SpeedUnit.KilometerPerHour);
                break;
            case TripPausedEvent tripPausedEvent:
                item.MarkerSet.Markers.Add(new PauseMarker(tripPausedEvent.Position, item.MarkerSet));
                item.Status = LiveDataItemViewModel.TripStatus.Paused;
                item.CurrentLocation = tripPausedEvent.Position;
                SetCalculatedValues(item, tripPausedEvent);
                ToggleVisibilityOfCurrentPositionMarker(item.MarkerSet,false);
                break;
            case TripResumedEvent tripResumedEvent:
                item.MarkerSet.Markers.AddWithId(new ResumeMarker(tripResumedEvent.Position, item.MarkerSet));
                item.Status = LiveDataItemViewModel.TripStatus.Active;
                item.CurrentLocation = tripResumedEvent.Position;
                SetCalculatedValues(item, tripResumedEvent);
                break;
            case FuelStopEvent fuelStopEvent:
                item.MarkerSet.Markers.AddWithId(new FuelStopMarker(fuelStopEvent.Position, item.MarkerSet));
                item.Status = LiveDataItemViewModel.TripStatus.FuelStop;
                item.CurrentLocation = fuelStopEvent.Position;
                SetCalculatedValues(item, fuelStopEvent);
                ToggleVisibilityOfCurrentPositionMarker(item.MarkerSet, false);
                break;
            case TripStoppedEvent tripStopEvent:
                item.MarkerSet.Markers.AddWithId(new StopMarker(tripStopEvent.Position, item.MarkerSet));
                item.Status = LiveDataItemViewModel.TripStatus.Finished;
                item.CurrentLocation = tripStopEvent.Position;
                SetCalculatedValues(item, tripStopEvent);
                item.EndTime = tripStopEvent.Timestamp;
                item.EndLocation = tripStopEvent.Position;
                RemoveCurrentPositionMarker(item.MarkerSet);
                break;
        }
    }

    private void SetCurrentPositionMarker(Coordinate currentLocation, MarkerSet markerSet)
    {
        var marker = markerSet.Markers.SingleOrDefault(m => m is CurrentPositionMarker);
        if (marker == null)
        {
            markerSet.Markers.AddWithId(new CurrentPositionMarker(currentLocation, markerSet));
        }
        else
        {
            marker.Position = currentLocation;
            marker.IsVisible = markerSet.IsVisible;
        }
    }
    
    private void RemoveCurrentPositionMarker(MarkerSet markerSet)
    {
        var marker = markerSet.Markers.SingleOrDefault(m => m is CurrentPositionMarker);
        if (marker != null)
        {
            markerSet.Markers.Remove(marker);
        }
    }
    private void ToggleVisibilityOfCurrentPositionMarker(MarkerSet markerSet, bool value)
    {
        var marker = markerSet.Markers.SingleOrDefault(m => m is CurrentPositionMarker);
        if (marker != null)
        {
            marker.IsVisible = value;
        }
    }


    private static void SetCalculatedValues(LiveDataItemViewModel item, Event tripEvent)
    {
        item.Duration = tripEvent.Timestamp - item.StartTime;
        var dist = new Distance(item.StartLocation, item.CurrentLocation, Shape.Ellipsoid);
        item.Distance = Length.From(dist.Kilometers, LengthUnit.Kilometer).ToUnit(LengthUnit.Kilometer);
    }
}



public partial class LiveDataItemViewModel : ObservableObject
{
    [ObservableProperty] private string _vehicleId;
    [ObservableProperty] private string _tripId;
    [ObservableProperty] private TripStatus _status;
    [ObservableProperty] private Coordinate _startLocation;
    [ObservableProperty] private Coordinate _endLocation;
    [ObservableProperty] private Coordinate _currentLocation;
    [ObservableProperty] private DateTimeOffset _startTime;
    [ObservableProperty] private DateTimeOffset _endTime;
    [ObservableProperty] private Length _distance;
    [ObservableProperty] private TimeSpan _duration;
    [ObservableProperty] private Speed _speed;
    [ObservableProperty] private bool _isVisibleOnMap;
    [ObservableProperty] private bool _isFollowingOnMap;
    [ObservableProperty] private bool _isFollowingAllowed;
    [ObservableProperty] private MarkerSet _markerSet;

    
    
    public enum TripStatus
    {
        Active, Paused, FuelStop, Finished
    }
    

    public LiveDataItemViewModel(string vehicleId, string tripId, Coordinate startLocation, DateTimeOffset startTime)
    {
        VehicleId = vehicleId;
        TripId = tripId;
        Status = TripStatus.Active;
        StartLocation = startLocation;
        StartLocation.FormatOptions.Format = CoordinateFormatType.Degree_Minutes_Seconds;
        StartLocation.FormatOptions.Round = 3;
        CurrentLocation = startLocation;
        StartTime = startTime;
        MarkerSet = new MarkerSet(tripId);
        IsVisibleOnMap = true;

    }

    partial void OnIsVisibleOnMapChanged(bool value)
    {
        MarkerSet.IsVisible = value;
        IsFollowingAllowed = IsFollowingAllowed && IsVisibleOnMap && MarkerSet.IsSelected;
    }

    partial void OnIsFollowingOnMapChanged(bool value)
    {
        var marker = this.MarkerSet.Markers.Single(m => m is CurrentPositionMarker);
        marker.IsFollowing = value;
    }
}


public class StartMarker : Marker
{
    public StartMarker(Coordinate position, MarkerSet markerSet, string description = "") : base(position, Colors.Green, "Started", description, true, false, markerSet)
    {
    }
}

public class CurrentPositionMarker : Marker
{
    public CurrentPositionMarker(Coordinate location, MarkerSet markerSet, string description = "") : base(location, Colors.Blue, "Current position",  description, true, false, markerSet)
    {
    }
}

public class PauseMarker : Marker
{
    public PauseMarker(Coordinate location, MarkerSet markerSet, string description = "") : base(location, Colors.Yellow, "Paused", description, true, false, markerSet)
    {
    }
}

public class ResumeMarker : Marker
{
    public ResumeMarker(Coordinate location, MarkerSet markerSet, string description = "") : base(location, Colors.Olive, "Resumed", description, true, false, markerSet)
    {
    }
}
public class FuelStopMarker : Marker
{
    public FuelStopMarker(Coordinate location, MarkerSet markerSet, string description =  "") : base(location, Colors.Purple, "Fuel stop", description, true, false, markerSet)
    {
    }
}

public class StopMarker : Marker
{
    public StopMarker(Coordinate location, MarkerSet markerSet, string description = "") : base(location, Colors.Red, "Stopped at", description, true, false, markerSet)
    {
    }
}