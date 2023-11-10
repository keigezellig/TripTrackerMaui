using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Mapsui.Extensions;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using Mapsui.Utilities;
using Mapsui.Widgets;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.Zoom;
using Microsoft.Extensions.Logging;
using HorizontalAlignment =Mapsui.Widgets.HorizontalAlignment;
using VerticalAlignment = Mapsui.Widgets.VerticalAlignment;
using Map = Mapsui.Map;

namespace MauiApp1.Controls.MarkerMap;

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
                if (e.NewItems != null)
                    foreach (MarkerSet newItem in e.NewItems)
                    {
                        newItem.Markers.CollectionChanged += UpdateMarkerSet;
                    }

                break;
            case NotifyCollectionChangedAction.Remove:
                break;
            case NotifyCollectionChangedAction.Replace:
                break;
            case NotifyCollectionChangedAction.Move:
                break;
            case NotifyCollectionChangedAction.Reset:
                if (e.NewItems != null)
                    foreach (MarkerSet newItem in e.NewItems)
                    {
                        newItem.Markers.CollectionChanged += UpdateMarkerSet;
                    }
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateMarkerSet(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                AddPins(e.NewItems);
                SetPropertyChangedHandlers(e.NewItems);
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
                SetPropertyChangedHandlers(e.NewItems);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetPropertyChangedHandlers(IList addedItems)
    {
        if (addedItems == null) return;
        
        var markerItems = addedItems.Cast<Marker>();

        foreach (var markerItem in markerItems)
        {
            //TODO
            if (markerItem.PropertyChanged == null)
            {
                markerItem.PropertyChanged += ((sender, args) =>
                {
                    var pin = _currentPins.SingleOrDefault(p => (int)p.Tag == markerItem.Id);

                    if (pin == null) return;
                    if (args.PropertyName == nameof(markerItem.Location))
                    {
                        pin.Position = new Position(markerItem.Location.Latitude, markerItem.Location.Longitude);
                    }
                    else if (args.PropertyName == nameof(markerItem.IsVisible))
                    {
                        pin.IsVisible = markerItem.IsVisible;
                    }

                });
            }
        }
    }

    private void RemovePins(IEnumerable items)
    {
        //TODO:
        
        // if (items == null) return;
        // var markerItems = items.Cast<Marker>();
        // foreach (var item in markerItems)
        // {
        //     
        // }
        //
        // _currentPins.RemoveRange(markerItems.Where());
        
    }

    private void AddPins(IEnumerable newItems)
    {
        if (newItems == null) return;
        
        var markerItems = newItems.Cast<Marker>();
        foreach (var markerItem in markerItems)
        {
            var newPin = new Pin()
            {
                Color = markerItem.Color,
                Position = new Position(markerItem.Location.Latitude, markerItem.Location.Longitude),
                Label = markerItem.Label,
                Scale = 0.7f,
                Tag = markerItem.Id
            };
            _currentPins.Add(newPin);
        }
    }
}