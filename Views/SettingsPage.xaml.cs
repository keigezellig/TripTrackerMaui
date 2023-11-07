using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
        // viewModel.ErrorsChanged += async (sender, args) =>
        // {
        //     if (args.PropertyName == nameof(viewModel.MessageQueueHost))
        //     {
        //         await DisplayAlert("Error", "Enter a host name", "OK");
        //     }
        // };
    }
}