using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Mapsui.Extensions;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using Mapsui.Utilities;
using Mapsui.Widgets;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.Zoom;
using MauiApp1.ViewModels;
using Microsoft.Extensions.Logging;
using HorizontalAlignment =Mapsui.Widgets.HorizontalAlignment;
using VerticalAlignment = Mapsui.Widgets.VerticalAlignment;

namespace MauiApp1.Views;

public partial class LiveDataPage : ContentPage
{
    private readonly LiveDataViewModel _viewModel;
    private readonly ILogger<LiveDataPage> _logger;

    public LiveDataPage(LiveDataViewModel viewModel, ILogger<LiveDataPage> logger)
    {
        BindingContext = viewModel;
        this._viewModel = viewModel;
        _logger = logger;
        InitializeComponent();
        InitMap();

    }
    
    

    private void InitMap()
    {
        var osmLayer = OpenStreetMap.CreateTileLayer();
        
        MapView.Map.Layers.Add(osmLayer);
        MapView.Map.Widgets.Add(new ScaleBarWidget(MapView.Map) { TextAlignment = Alignment.Center, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top });
        MapView.Map.Widgets.Add(new ZoomInOutWidget { MarginX = 20, MarginY = 40 });
        
        MapView.MyLocationLayer.UpdateMyLocation(new Mapsui.UI.Maui.Position(51.8,5.8));
        MapView.MyLocationLayer.Scale = 0.7f;
        var xy =Mapsui.Projections.SphericalMercator.FromLonLat(MapView.MyLocationLayer.MyLocation.Longitude, MapView.MyLocationLayer.MyLocation.Latitude ).ToMPoint();
        MapView.Map.Navigator.CenterOnAndZoomTo(xy, 13);
        
        _viewModel.Markers.CollectionChanged += (sender, args) =>
        {
            UpdateMarkerList(args.Action, args.OldItems, args.OldStartingIndex, args.NewItems, args.NewStartingIndex);
        };
        
        WeakReferenceMessenger.Default.Register<LiveDataPage, PropertyChangedMessage<Location>>(
            this, 
            (r, m) => MainThread.BeginInvokeOnMainThread( () => r.UpdateMarker(m)));
    }

    private void UpdateMarker(PropertyChangedMessage<Location> message)
    {
        
        if (message.Sender is CurrentPositionMarker currentPositionMarker)
        {
            
            var pin = MapView.Pins.FirstOrDefault(p => p.Label == currentPositionMarker.Label);
            if (pin != null)
            {
                _logger.LogInformation($"Updating marker: {message.Sender} {message.PropertyName} {message.NewValue}");
                pin.Position = new Position(new Position(currentPositionMarker.Location.Latitude,
                    currentPositionMarker.Location.Longitude));
                
            }
        }
    }

    private void UpdateMarkerList(NotifyCollectionChangedAction action, IList oldItems, int oldStartingIndex, IList newItems, int newStartingIndex)
    {
        _logger.LogInformation($"UpdateMarkerList: {action}, oldStartingIndex: {oldStartingIndex}, newStartingIndex: {newStartingIndex}, oldItems: {oldItems?.Count ?? -1}, newItems: {newItems?.Count ?? -1} ");
        _logger.LogInformation($"Pin list count before: {MapView.Pins.Count}");
        var pins = (ObservableRangeCollection<Pin>)MapView.Pins;
        switch (action)
        {
            case NotifyCollectionChangedAction.Add:
                if (newItems != null)
                {
                    if (newItems[0] is StartMarker sm)
                    {
                        var xy =Mapsui.Projections.SphericalMercator.FromLonLat(sm.Location.Longitude, sm.Location.Latitude).ToMPoint();
                        MapView.Map.Navigator.CenterOnAndZoomTo(xy, 13);
                    }

                    _logger.LogInformation($"Adding {newItems[0]}");
                    if (newItems[0] is Marker newMarker)
                    {
                        var newPin = new Pin()
                        {
                            Color = newMarker.Color,
                            Position = new Position(newMarker.Location.Latitude, newMarker.Location.Longitude),
                            Label = newMarker.Label,
                            Scale = 0.7f
                        };
                        
                        pins.Add(newPin);
                        var bla = newPin.Callout;
                        newPin.ShowCallout();
                    }
                }   
                break;
            case NotifyCollectionChangedAction.Remove:
                pins.RemoveAt(oldStartingIndex);
                break;
            case NotifyCollectionChangedAction.Replace:
                break;
            case NotifyCollectionChangedAction.Move:
                break;
            case NotifyCollectionChangedAction.Reset:
                pins.Clear();
                if (newItems != null)
                {
                    pins.ReplaceRange(newItems.Cast<Marker>().Select(item => new Pin()
                    {
                        Color = item.Color, Position = new Position(item.Location.Latitude, item.Location.Longitude),
                        Label = item.Label,
                        Scale = 0.7f
                    }));
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
            
                
        }
        _logger.LogInformation($"Pinlist count after: {MapView.Pins.Count}");
    }

    private void UpdateMarkerList(NotifyCollectionChangedAction action, IList newItems, int newItemIndex)
    {
        
    }
}