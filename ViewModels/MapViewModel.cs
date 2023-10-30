using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MauiApp1.Models;
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
        Messenger.Register<MapViewModel, GpsModel>(this, (r, m) => MainThread.BeginInvokeOnMainThread( () => r.UpdateMapLocation(m)));
    }

    private void UpdateMapLocation(GpsModel model)
    {
        if (PinLocations.Count == 0)
        {
            PinLocations.Add(new PinInfo(model.Location, model.VehicleId, $"{model.GpsSpeed * 3.6:F1} km/h"));
            MapSpan = MapSpan.FromCenterAndRadius(model.Location, new Distance(500));
        }
        else
        {
           PinLocations.Clear();
           PinLocations.Add(new PinInfo(model.Location, model.VehicleId,$"{model.GpsSpeed * 3.6:F1} km/h"));
        }
        
         
    }
}

public partial class PinInfo : ObservableObject
{
    [ObservableProperty] 
    private Location _location;
    
    [ObservableProperty] 
    private string _label;
    
    [ObservableProperty] 
    private string _description;

    public PinInfo(Location location, string label, string description)
    {
        Location = location;
        Label = label;
        Description = description;
    }
}