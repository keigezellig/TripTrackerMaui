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

    public LiveDataPage(LiveDataViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();

        LiveTripCollectionView.SelectionChanged += (sender, args) =>
        {
            var previous = args.PreviousSelection.FirstOrDefault() as LiveDataItemViewModel;
            var current = args.CurrentSelection.FirstOrDefault() as LiveDataItemViewModel;

            if (previous != null)
            {
                previous.MarkerSet.IsSelected = false;
            }

            if (current != null)
            {
                current.MarkerSet.IsSelected = true;
            }

        };
        
        
        

    }
    
    

    
}