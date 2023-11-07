using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapsui.Extensions;
using Mapsui.Tiling;
using Mapsui.Widgets;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.Zoom;
using MauiApp1.ViewModels;
using HorizontalAlignment =Mapsui.Widgets.HorizontalAlignment;
using VerticalAlignment = Mapsui.Widgets.VerticalAlignment;

namespace MauiApp1.Views;

public partial class LiveDataPage : ContentPage
{
    public LiveDataPage(LiveDataViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
        InitMap();

    }
    
    

    private void InitMap()
    {
        MapView.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
        MapView.Map.Widgets.Add(new ScaleBarWidget(MapView.Map) { TextAlignment = Alignment.Center, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top });
        MapView.Map.Widgets.Add(new ZoomInOutWidget { MarginX = 20, MarginY = 40 });

        MapView.MyLocationLayer.UpdateMyLocation(new Mapsui.UI.Maui.Position(51.8,5.8));
    }
}