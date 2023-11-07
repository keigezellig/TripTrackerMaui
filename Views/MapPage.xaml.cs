using Mapsui.Extensions;
using Mapsui.Tiling;
using Mapsui.UI.Objects;
using Mapsui.Widgets;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.Zoom;
using MauiApp1.ViewModels;
using HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment;
using VerticalAlignment =Mapsui.Widgets.VerticalAlignment;

namespace MauiApp1.Views;

public partial class MapPage : ContentPage
{
	public MapPage(MapViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
		
		var test = Mapje ?? throw new InvalidOperationException();
		var test1 = info ?? throw new InvalidOperationException();

		Mapje!.RotationLock = false;
		Mapje.UnSnapRotationDegrees = 20;
		Mapje.ReSnapRotationDegrees = 5;
		Mapje.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
		Mapje.Map.Widgets.Add(new ScaleBarWidget(Mapje.Map) { TextAlignment = Alignment.Center, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top });
		Mapje.Map.Widgets.Add(new ZoomInOutWidget { MarginX = 20, MarginY = 40 });

		Mapje.MyLocationLayer.UpdateMyLocation(new Mapsui.UI.Maui.Position(51.8,5.8));
		
		

	}
}