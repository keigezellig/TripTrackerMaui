using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Models.TripEvents;
using CommunityToolkit.Mvvm.Messaging;
using Mapsui.UI.Maui;
using UnitsNet;
using UnitsNet.Units;
using Distance = Microsoft.Maui.Maps.Distance;

namespace MauiApp1.ViewModels;


public partial class LiveDataViewModel : ObservableRecipient
{
    [ObservableProperty] 
    private ObservableCollection<LiveDataItemViewModel> _dataItems;

    [ObservableProperty] private ObservableCollection<Marker> _markers;

    public LiveDataViewModel()
    { 
        DataItems = new ObservableCollection<LiveDataItemViewModel>();
        Markers = new ObservableCollection<Marker>();
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
                Markers.Clear();
                
                DataItems.Add(
                    new LiveDataItemViewModel(
                        startedEvent.VehicleId, 
                        startedEvent.TripId, 
                        startedEvent.Position, 
                        startedEvent.Timestamp));
                Markers.Add(new StartMarker(startedEvent.Position));
                break;
            case GnssEvent gnssEvent:
                item.CurrentLocation = gnssEvent.Location;
                SetMarker(item.CurrentLocation);
                SetCalculatedValues(item, gnssEvent);
                item.Speed = gnssEvent.GpsSpeed.ToUnit(SpeedUnit.KilometerPerHour);
                break;
            case TripPausedEvent tripPausedEvent:
                Markers.Add(new PauseMarker(tripPausedEvent.Position));
                item.Status = LiveDataItemViewModel.TripStatus.Paused;
                item.CurrentLocation = tripPausedEvent.Position;
                SetCalculatedValues(item, tripPausedEvent);
                RemoveCurrentMarker();
                break;
            case TripResumedEvent tripResumedEvent:
                Markers.Add(new ResumeMarker(tripResumedEvent.Position));
                item.Status = LiveDataItemViewModel.TripStatus.Active;
                item.CurrentLocation = tripResumedEvent.Position;
                SetCalculatedValues(item, tripResumedEvent);
                break;
            case FuelStopEvent fuelStopEvent:
                Markers.Add(new FuelStopMarker(fuelStopEvent.Position));
                item.Status = LiveDataItemViewModel.TripStatus.FuelStop;
                item.CurrentLocation = fuelStopEvent.Position;
                SetCalculatedValues(item, fuelStopEvent);
                RemoveCurrentMarker();
                break;
            case TripStoppedEvent tripStopEvent:
                Markers.Add(new StopMarker(tripStopEvent.Position));
                item.Status = LiveDataItemViewModel.TripStatus.Finished;
                item.CurrentLocation = tripStopEvent.Position;
                SetCalculatedValues(item, tripStopEvent);
                item.EndTime = tripStopEvent.Timestamp;
                item.EndLocation = tripStopEvent.Position;
                RemoveCurrentMarker();
                break;
        }
        
    }

    private void SetMarker(Location currentLocation)
    {
        var marker = Markers.FirstOrDefault(m => m is CurrentPositionMarker);
        if (marker == null)
        {
            Markers.Add(new CurrentPositionMarker(currentLocation));
        }
        else
        {
            marker.Location = currentLocation;
        }
    }
    
    private void RemoveCurrentMarker()
    {
        var marker = Markers.FirstOrDefault(m => m is CurrentPositionMarker);
        if (marker != null)
        {
            Markers.Remove(marker);
        }
    }


    private static void SetCalculatedValues(LiveDataItemViewModel item, Event tripEvent)
    {
        item.Duration = tripEvent.Timestamp - item.StartTime;
        Distance dist = Distance.BetweenPositions(item.StartLocation, item.CurrentLocation);
        item.Distance = Length.From(dist.Kilometers, LengthUnit.Kilometer).ToUnit(LengthUnit.Kilometer);
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
    [ObservableProperty] private Length _distance;
    [ObservableProperty] private TimeSpan _duration;
    [ObservableProperty] private Speed _speed;
    
    
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

public partial class Marker : ObservableRecipient
{
    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private Location _location;
    [ObservableProperty]
    private Color _color;
    [ObservableProperty] 
    private string _label;
    [ObservableProperty] 
    private string _description;

    protected Marker(Location location, Color color, string label, string description)
    {
        Location = location;
        Color = color;
        Label = label;
        Description = description;
    }
}

public partial class StartMarker : Marker
{
    public StartMarker(Location location, string description = "") : base(location, Colors.Green, "Started", description)
    {
    }
}

public partial class CurrentPositionMarker : Marker
{
    public CurrentPositionMarker(Location location, string description = "") : base(location, Colors.Blue, "Current position", description)
    {
    }
}

public partial class PauseMarker : Marker
{
    public PauseMarker(Location location, string description = "") : base(location, Colors.Yellow, "Paused", description)
    {
    }
}

public partial class ResumeMarker : Marker
{
    public ResumeMarker(Location location, string description = "") : base(location, Colors.Olive, "Resumed", description)
    {
    }
}
public partial class FuelStopMarker : Marker
{
    public FuelStopMarker(Location location, string description =  "") : base(location, Colors.Purple, "Fuel stop", description)
    {
    }
}

public partial class StopMarker : Marker
{
    public StopMarker(Location location, string description = "") : base(location, Colors.Red, "Stopped at", description)
    {
    }
}