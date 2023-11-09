using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiApp1.Controls.MarkerMap;
using MauiApp1.Models;
using MauiApp1.Models.TripEvents;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Maps;

namespace MauiApp1.ViewModels;

public partial class MapViewModel : ObservableObject
{
    private ILogger<MapViewModel> _logger;
    [ObservableProperty] private ObservableCollection<MarkerSet> _markers;

    public MapViewModel(ILogger<MapViewModel> logger)
    {
        _logger = logger;
        Markers = new ObservableCollection<MarkerSet>();
    }

    [RelayCommand]
    private void AddMarkerSet()
    {
        _logger.LogInformation("Adding marker set to list");
        Markers.Add(new MarkerSet("A"));
    }
}


