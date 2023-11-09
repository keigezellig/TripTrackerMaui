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
		
		
		
		

	}
}