using CommunityToolkit.Mvvm.Messaging;

using CoordinateSharp.Formatters;

using Mapsui;
using Mapsui.Extensions;
using Mapsui.Fetcher;
using Mapsui.Layers;
using Mapsui.Layers.AnimatedLayers;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Styles.Thematics;
using Mapsui.Tiling;
using Mapsui.Widgets;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.Zoom;

using MauiApp1.Models.TripEvents;
using MauiApp1.ViewModels;

using UnitsNet;

using Color = Mapsui.Styles.Color;
using HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment;
using VerticalAlignment = Mapsui.Widgets.VerticalAlignment;

namespace MauiApp1.Views;

public partial class MapPage : ContentPage
{
    private Mapsui.Map _map;

    public MapPage(MapViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();

        _map = MapView.Map;
        InitializeMap();

    }

    void OnToggleVisiblity(object sender, EventArgs args)
    {
        _map.Layers[1].Enabled = !_map.Layers[1].Enabled;
    }

    public void InitializeMap()
    {
        _map.Layers.Add(OpenStreetMap.CreateTileLayer());
        _map.Widgets.Add(new ScaleBarWidget(MapView.Map) { TextAlignment = Alignment.Center, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top });
        _map.Widgets.Add(new ZoomInOutWidget { MarginX = 20, MarginY = 40 });

        var animatedPointLayer = new AnimatedPointLayer(new PointProvider())
        {
            Name = "Current positions",
            Style = CreatePointStyle()
        };
        _map.Layers.Add(animatedPointLayer);
        _map.Layers[1].Enabled = true;
        _map.Home = n => n.ZoomToBox(animatedPointLayer.Extent);
    }

    private IStyle CreatePointStyle()
    {
        return new ThemeStyle(f =>
        {
            return new SymbolStyle() { Fill = new Mapsui.Styles.Brush(Color.Blue), SymbolScale = 1, SymbolType = SymbolType.Triangle, SymbolRotation = (double)f["rotation"] };
        });
    }
}

internal class PointProvider : MemoryProvider, IDynamic
{
    public event DataChangedEventHandler DataChanged;

    private List<PointFeature> _currentPositions;

    private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

    public PointProvider()
    {
        WeakReferenceMessenger.Default.Register<PointProvider, GnssEvent>(this, (r, m) => r.UpdateData(m));
        WeakReferenceMessenger.Default.Register<PointProvider, TripStoppedEvent>(this, (r, m) => r.RemovePoint(m));

        _currentPositions = new List<PointFeature>();
    }


    private void RemovePoint(TripStoppedEvent m)
    {
        var point = Find(m.TripId, "ID");
        if (point != null)
        {

            Clear();
        }

        OnDataChanged();
    }

    private double AngleOf(MPoint point1, MPoint point2)
    {
        if (point2 == null) return 0;
        var angleInRadians = Math.Atan2(point1.Y - point2.Y, point2.X - point1.X);
        return Angle.FromRadians(angleInRadians).Degrees.NormalizeDegrees360();

    }

    private void UpdateData(GnssEvent m)
    {
        var point = Find(m.TripId, "ID") ?? new PointFeature(SphericalMercator.FromLonLat(m.Location.Longitude.ToDouble(), m.Location.Latitude.ToDouble()).ToMPoint());

        var positionFeature = _currentPositions.SingleOrDefault(f => (string)f["ID"] == m.TripId) ?? new PointFeature(SphericalMercator.FromLonLat(m.Location.Longitude.ToDouble(), m.Location.Latitude.ToDouble()).ToMPoint());
        if (!positionFeature.Fields.Contains("ID"))
        {
            positionFeature["ID"] = m.TripId;
            positionFeature["rotation"] = 0.0;
            _currentPositions.Add(positionFeature);
        }
        else
        {
            var newCoords = SphericalMercator.FromLonLat(m.Location.Longitude.ToDouble(), m.Location.Latitude.ToDouble()).ToMPoint();
            positionFeature["rotation"] = AngleOf(newCoords, positionFeature.Point) - 90;

            positionFeature.Point.X = newCoords.X;
            positionFeature.Point.Y = newCoords.Y;

        }

        OnDataChanged();
    }

    public void DataHasChanged()
    {
        OnDataChanged();
    }

    private void OnDataChanged()
    {
        DataChanged?.Invoke(this, new DataChangedEventArgs(null, false, null));
    }


    public override Task<IEnumerable<IFeature>> GetFeaturesAsync(FetchInfo fetchInfo)
    {

        return Task.FromResult(_currentPositions.AsEnumerable<IFeature>());
    }


}