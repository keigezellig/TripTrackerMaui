using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Controls.MarkerMap;

public partial class MarkerMapView : ContentView
{
    
    public static readonly BindableProperty MarkerCollectionProperty = BindableProperty.Create(nameof(MarkerCollection), typeof(ObservableCollection<MarkerSet>), typeof(MarkerMapView), null);

    public ObservableCollection<MarkerSet> MarkerCollection
    {
        get => (ObservableCollection<MarkerSet>)GetValue(MarkerMapView.MarkerCollectionProperty);
        set => SetValue(MarkerCollectionProperty, value);
    }
    
    public MarkerMapView()
    {
        InitializeComponent();
    }
}