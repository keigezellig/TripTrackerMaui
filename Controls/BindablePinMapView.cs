using Mapsui.UI.Maui;
using Mapsui.Utilities;

namespace MauiApp1.Controls;

public class BindablePinMapView : MapView
{
    public static readonly BindableProperty MarkersProperty = 
        BindableProperty.Create(
            nameof(Markers), 
            typeof(IEnumerable<Position>), 
            typeof(BindablePinMapView), null, 
            BindingMode.TwoWay, propertyChanged: (bindableObject, _, newValue) =>
     {
         if (bindableObject is BindablePinMapView map && newValue is IEnumerable<Position> markerList)
         {
             UpdateMarkerList(map, markerList);
         }
     });

    private static void UpdateMarkerList(BindablePinMapView map, IEnumerable<Position> markerList)
    {
       var pins = map.Pins as ObservableRangeCollection<Pin>;
        pins.ReplaceRange(markerList.Select(m => new Pin() {Position = m, Color = Colors.Blue}));
    }


    public IEnumerable<Position> Markers
    {
        get => (IEnumerable<Position>)this.GetValue(MarkersProperty);
        set => this.SetValue(MarkersProperty, value);
    }

   
}