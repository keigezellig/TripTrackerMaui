using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CoordinateSharp;
using MauiApp1.Controls.MarkerMap;
using MauiApp1.Models;
using MauiApp1.Models.TripEvents;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Maps;

namespace MauiApp1.ViewModels;

public partial class MapViewModel : ObservableObject
{
    private ILogger<MapViewModel> _logger;
    [ObservableProperty] private ObservableCollection<MarkerSet> _markerCollection;

    public MapViewModel(ILogger<MapViewModel> logger)
    {
        _logger = logger;
        MarkerCollection = new ObservableCollection<MarkerSet>();
    }

    [RelayCommand]
    private void AddSet()
    {
        _logger.LogInformation("Adding marker set to list");
        MarkerCollection.Add(new MarkerSet("A"));
    }

    [RelayCommand]
    private void AddMarkerToSet()
    {
        _logger.LogInformation("Adding marker to set");
        var markerSet = MarkerCollection.First();
        markerSet.Markers.Add(new Controls.MarkerMap.Marker(new Coordinate(),Colors.Blue, "hallo","test", true, false, markerSet));
    }

    [RelayCommand]
    private void RemoveMarkerFromSet()
    {
        _logger.LogInformation("Remove marker from set");
        var marker = MarkerCollection.First().Markers.First();
        MarkerCollection.First().Markers.Remove(marker);
    }
    
    [RelayCommand]
    private void ChangeMarker()
    {
        _logger.LogInformation("Change marker");
        var marker = MarkerCollection.First().Markers.First();
        marker.Label = "Poep";
    }
    
    [RelayCommand]
    private void RemoveSet()
    {
        // _logger.LogInformation("Remove set");
        // var markerSet = MarkerCollection.First();
        // MarkerCollection.Remove(markerSet);
        _logger.LogInformation("Toggle visibiluty of set");
        var markerSet = MarkerCollection.First();
        markerSet.IsVisible = true;
    }
}


