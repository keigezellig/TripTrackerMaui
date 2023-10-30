using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Maps;

namespace MauiApp1.ViewModels;

public partial class MapViewModel : ObservableRecipient
{
    [ObservableProperty] 
    private MapSpan _mapSpan;

    [ObservableProperty] 
    private ObservableCollection<PinInfo> _pinLocations;

    public MapViewModel()
    {
        PinLocations = new ObservableCollection<PinInfo>();
        IsActive = true;
    }
    protected override void OnActivated()
    {
        Messenger.Register<MapViewModel, GpsMessage>(this, (r, m) => MainThread.BeginInvokeOnMainThread( () => r.UpdateMapLocation(m)));
    }

    private void UpdateMapLocation(GpsMessage message)
    {
        if (PinLocations.Count == 0)
        {
            PinLocations.Add(new PinInfo(message.Location, $"{message.GpsSpeed:F1} km/h"));
            MapSpan = MapSpan.FromCenterAndRadius(message.Location, new Distance(500));
        }
        else
        {
           PinLocations.Clear();
           PinLocations.Add(new PinInfo(message.Location, $"{message.GpsSpeed:F1} km/h"));
        }
        
         
    }
}

public partial class PinInfo : ObservableObject
{
    [ObservableProperty] 
    private Location _location;
    
    [ObservableProperty] 
    private string _label;

    public PinInfo(Location location, string label)
    {
        Location = location;
        Label = label;
    }
}