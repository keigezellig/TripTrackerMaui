﻿using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Mapsui.Extensions;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using Mapsui.Utilities;
using Mapsui.Widgets;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.Zoom;

using HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment;
using Map = Mapsui.Map;
using VerticalAlignment = Mapsui.Widgets.VerticalAlignment;

namespace TripTracker.Controls.MarkerMap;

public partial class MarkerMapView : ContentView
{
    public static readonly BindableProperty MarkerCollectionSourceProperty = BindableProperty.Create(
        nameof(MarkerCollectionSource),
        typeof(ObservableCollection<MarkerSet>),
        typeof(MarkerMapView),
        defaultValue: null,
        defaultValueCreator: CreateList,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: UpdateCollection);


    private static void UpdateCollection(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MarkerMapView mapView && newValue is ObservableCollection<MarkerSet> newMarkerSet)
        {
            newMarkerSet.CollectionChanged += mapView.UpdateMarkerSetCollection;
        }
    }


    private static object CreateList(BindableObject bindable)
    {
        var collection = new ObservableCollection<MarkerSet>();
        var mapView = (MarkerMapView)bindable;
        collection.CollectionChanged += mapView.UpdateMarkerSetCollection;

        return collection;
    }

    private readonly Map _map;
    private readonly ObservableRangeCollection<Pin> _currentPins;

    public ObservableCollection<MarkerSet> MarkerCollectionSource
    {
        get => (ObservableCollection<MarkerSet>)GetValue(MarkerCollectionSourceProperty);
        set => SetValue(MarkerCollectionSourceProperty, value);
    }

    public MarkerMapView()
    {
        InitializeComponent();
        _map = MapView.Map;
        _currentPins = (ObservableRangeCollection<Pin>)MapView.Pins;
        InitializeMap();
    }

    private void InitializeMap()
    {
        _map.Layers.Add(OpenStreetMap.CreateTileLayer());
        _map.Widgets.Add(new ScaleBarWidget(MapView.Map) { TextAlignment = Alignment.Center, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top });
        _map.Widgets.Add(new ZoomInOutWidget { MarginX = 20, MarginY = 40 });
        MapView.MyLocationEnabled = false;
    }



    public void UpdateMarkerSetCollection(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:

                SetMarkerSetPropertyHandlers(e.NewItems);

                break;
            case NotifyCollectionChangedAction.Remove:
                break;
            case NotifyCollectionChangedAction.Replace:
                break;
            case NotifyCollectionChangedAction.Move:
                break;
            case NotifyCollectionChangedAction.Reset:
                SetMarkerSetPropertyHandlers(e.NewItems);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetMarkerSetPropertyHandlers(IList newItems)
    {
        if (newItems == null) return;

        foreach (MarkerSet markerSet in newItems)
        {
            markerSet.Markers.CollectionChanged += UpdateMarkerSet;
            markerSet.PropertyChanged += (sender, args) =>
            {
                var set = (MarkerSet)sender!;

                if (args.PropertyName == nameof(set.IsSelected))
                {
                    if (!set.Markers.Any())
                    {
                        return;
                    }

                    if (set.IsSelected && set.IsVisible)
                    {
                        var firstMarker = set.Markers.First();
                        _map.Navigator.CenterOnAndZoomTo(new Position(firstMarker.Position.Latitude.ToDouble(), firstMarker.Position.Longitude.ToDouble()).ToMapsui(), 13);
                    }
                }
            };
        }
    }

    private void UpdateMarkerSet(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                AddPins(e.NewItems);
                SetMarkerPropertyChangedHandlers(e.NewItems);
                break;
            case NotifyCollectionChangedAction.Remove:
                RemovePins(e.OldItems);
                break;
            case NotifyCollectionChangedAction.Replace:
                break;
            case NotifyCollectionChangedAction.Move:
                break;
            case NotifyCollectionChangedAction.Reset:
                _currentPins.Clear();
                AddPins(e.NewItems);
                SetMarkerPropertyChangedHandlers(e.NewItems);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetMarkerPropertyChangedHandlers(IEnumerable addedItems)
    {
        if (addedItems == null) return;

        foreach (Marker markerItem in addedItems)
        {
            markerItem.PropertyChanged += ((sender, args) =>
            {
                var marker = (Marker)sender!;
                var pin = _currentPins.SingleOrDefault(p => p.Tag != null && (int)p.Tag == marker.Id);

                if (pin == null) return;

                switch (args.PropertyName)
                {
                    case nameof(marker.Position):
                        pin.Position = new Position(marker.Position.Latitude.ToDouble(), marker.Position.Longitude.ToDouble());
                        if (marker.IsVisible && marker.IsFollowing && marker.MarkerSet.IsSelected)
                        {
                            _map.Navigator.CenterOnAndZoomTo(pin.Position.ToMapsui(), 13);
                        }
                        break;
                    case nameof(marker.IsVisible):
                        pin.IsVisible = marker.IsVisible;
                        break;
                    case nameof(marker.Color):
                        pin.Color = marker.Color;
                        break;
                    case nameof(marker.Label):
                        pin.Label = marker.Label;
                        break;
                    case nameof(marker.Description):
                        //TODO Something with descriptopn
                        break;
                }

            });

        }
    }

    private void RemovePins(IEnumerable items)
    {
        if (items == null) return;

        foreach (var item in items.Cast<Marker>())
        {
            var pin = _currentPins.SingleOrDefault(p => (int)p.Tag == item.Id);
            _currentPins.Remove(pin);

        }
    }

    private void AddPins(IEnumerable newItems)
    {
        if (newItems == null) return;

        foreach (Marker marker in newItems)
        {
            var newPin = new Pin()
            {
                Color = marker.Color,
                Position = new Position(marker.Position.Latitude.ToDouble(), marker.Position.Longitude.ToDouble()),
                Label = marker.Label,
                Scale = 0.7f,
                Tag = marker.Id
            };
            _currentPins.Add(newPin);
        }
    }
}