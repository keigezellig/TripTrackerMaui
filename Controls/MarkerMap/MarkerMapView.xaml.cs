using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CoordinateSharp;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using Mapsui.Utilities;
using Mapsui.Widgets;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.Zoom;
using Microsoft.Extensions.Logging;
using Distance = CoordinateSharp.Distance;
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

                    if (set.IsSelected)
                    {
                        var firstMarker = set.Markers.First();
                        var lastMarker = set.Markers.Last();
                        if (firstMarker == lastMarker)
                        {
                            _map.Navigator.CenterOnAndZoomTo(new Position(firstMarker.Position.Latitude.ToDouble(), firstMarker.Position.Longitude.ToDouble()).ToMapsui(), 13);
                        }
                        else
                        {
                            MRect box = CalculateBoundingBox(firstMarker.Position, lastMarker.Position);
                            
                            _map.Navigator.ZoomToBox(box);
                        }
                    }
                } 
            };
        }
    }

    private MRect CalculateBoundingBox(Coordinate firstMarkerPosition, Coordinate lastMarkerPosition)
    {
        
        Coordinate c = new Coordinate(firstMarkerPosition.Latitude.ToDouble(), firstMarkerPosition.Longitude.ToDouble());
        Distance distanceAB = new Distance(firstMarkerPosition, lastMarkerPosition, Shape.Ellipsoid);
        GeoFence.Drawer gd = new GeoFence.Drawer(c, Shape.Ellipsoid, distanceAB.Bearing);
        gd.Draw(new Distance(0.5 * distanceAB.Kilometers), -90);
        gd.Draw(new Distance(distanceAB.Kilometers), 90);
        gd.Draw(new Distance(distanceAB.Kilometers), -90);
        gd.Draw(new Distance(distanceAB.Kilometers), -90);
        gd.Draw(new Distance(0.5 * distanceAB.Kilometers), -90);
        
        
        //See: https://coordinatesharp.com/DeveloperGuide#moving-coordinates
        
        
        
        //Set coordinates
        var upperLeft = new Coordinate(firstMarkerPosition.Latitude.ToDouble(), firstMarkerPosition.Longitude.ToDouble());
        var lowerRight = new Coordinate(lastMarkerPosition.Latitude.ToDouble(), lastMarkerPosition.Longitude.ToDouble());

        //Get distance from start to end
        // Distance distanceAB = new Distance(firstMarkerPosition, lastMarkerPosition, Shape.Ellipsoid);
        //
        // double distance = 0.5 * distanceAB.Meters;
        //
        // upperLeft.Move(dis); 
     
        // //Set width of box as distance between A and B
        // double width = distanceAB.Meters; 
        //
        // //Set height of box at 500m
        // double height = 100000;
        //
        // //Turn -90 degrees
        // double baseBearing = distanceAB.Bearing-90;
		      //
        // //Calculate Point C from A
        // var crdInitialPointC = new Coordinate(crdInitialPointA.Latitude.ToDouble(), crdInitialPointA.Longitude.ToDouble());
        // crdInitialPointC.Move(height, distanceAB.Bearing-90, Shape.Ellipsoid);
        //
        // //Calculate Point D from C
        // //Get new "initial" bearing by reverse calculating the distance from C to A
        // Distance distanceCA = new Distance(crdInitialPointC, crdInitialPointA, Shape.Ellipsoid);
        // var crdInitialPointD = new Coordinate(crdInitialPointC.Latitude.ToDouble(), crdInitialPointC.Longitude.ToDouble());
        // crdInitialPointD.Move(width, distanceCA.Bearing-90, Shape.Ellipsoid);
        //

        var posses = gd.Points.Select(p => new Position(p.Latitude.ToDouble(), p.Longitude.ToDouble())).ToList();
        // var posA = new Position(gd.Points, centerPoint.Longitude.ToDouble() );//.ToMapsui();
        // var posB = new Position(crdInitialPointB.Latitude.ToDouble(), crdInitialPointB.Longitude.ToDouble() );//.ToMapsui();
        // var posC = new Position(crdInitialPointC.Latitude.ToDouble(), crdInitialPointC.Longitude.ToDouble() );//.ToMapsui();
        // var posD = new Position(crdInitialPointD.Latitude.ToDouble(), crdInitialPointD.Longitude.ToDouble() );//.ToMapsui();
        _currentPins.AddRange(posses.Select(ps => new Pin() {Position = ps, Color = Colors.Fuchsia, Tag = -1, Label = ps.ToString() }));
        foreach (var pin in _currentPins)
        {
            pin.ShowCallout();   
        }
        var pos1Rect = posses[0].ToMapsui();
        var pos2Rect = posses[4].ToMapsui();
        return new MRect(pos1Rect.X, pos1Rect.Y, pos2Rect.X, pos2Rect.Y);
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