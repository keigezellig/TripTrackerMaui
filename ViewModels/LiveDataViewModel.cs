using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Models.TripEvents;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Maps;

namespace MauiApp1.ViewModels;


public partial class LiveDataViewModel : ObservableRecipient
{
    [ObservableProperty] 
    private ObservableCollection<LiveDataItemViewModel> _dataItems;
    
    public LiveDataViewModel()
    {
        DataItems = new ObservableCollection<LiveDataItemViewModel>();
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
                DataItems.Add(
                    new LiveDataItemViewModel(
                        startedEvent.VehicleId, 
                        startedEvent.TripId, 
                        startedEvent.Position, 
                        startedEvent.Timestamp));
                break;
            case GnssEvent gnssEvent:
                item.CurrentLocation = gnssEvent.Location;
                SetCalculatedValues(item, gnssEvent);
                item.Speed = gnssEvent.GpsSpeed;
                break;
            case TripPausedEvent tripPausedEvent:
                item.Status = LiveDataItemViewModel.TripStatus.Paused;
                item.CurrentLocation = tripPausedEvent.Position;
                SetCalculatedValues(item, tripPausedEvent);
                break;
            case TripResumedEvent tripResumedEvent:
                item.Status = LiveDataItemViewModel.TripStatus.Active;
                item.CurrentLocation = tripResumedEvent.Position;
                SetCalculatedValues(item, tripResumedEvent);
                break;
            case FuelStopEvent fuelStopEvent:
                item.Status = LiveDataItemViewModel.TripStatus.FuelStop;
                item.CurrentLocation = fuelStopEvent.Position;
                SetCalculatedValues(item, fuelStopEvent);
                break;
            case TripStoppedEvent tripStopEvent:
                item.Status = LiveDataItemViewModel.TripStatus.Finished;
                item.CurrentLocation = tripStopEvent.Position;
                SetCalculatedValues(item, tripStopEvent);
                item.EndTime = tripStopEvent.Timestamp;
                item.EndLocation = tripStopEvent.Position;
                break;
        }
        
    }

    private static void SetCalculatedValues(LiveDataItemViewModel item, Event tripEvent)
    {
        item.Duration = tripEvent.Timestamp - item.StartTime;
        Distance dist = Distance.BetweenPositions(item.StartLocation, item.CurrentLocation);
        item.Distance = dist.Kilometers;
    }
}



public partial class LiveDataItemViewModel : ObservableObject
{
    [ObservableProperty] private string _vehicleId;
    [ObservableProperty] private string _tripId;
    [ObservableProperty] private TripStatus _status;
    [ObservableProperty] private Location _startLocation;
    [ObservableProperty] private Location _endLocation;
    [ObservableProperty] private Location _currentLocation;
    [ObservableProperty] private DateTimeOffset _startTime;
    [ObservableProperty] private DateTimeOffset _endTime;
    [ObservableProperty] private double _distance;
    [ObservableProperty] private TimeSpan _duration;
    [ObservableProperty] private double _speed;
    
    
    public enum TripStatus
    {
        Active, Paused, FuelStop, Finished
    }
    

    public LiveDataItemViewModel(string vehicleId, string tripId, Location startLocation, DateTimeOffset startTime)
    {
        VehicleId = vehicleId;
        TripId = tripId;
        Status = TripStatus.Active;
        StartLocation = startLocation;
        CurrentLocation = startLocation;
        StartTime = startTime;

    }
}