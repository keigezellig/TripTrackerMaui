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
        

    }
    
    

    
}