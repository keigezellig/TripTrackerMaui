using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class LiveDataPage : ContentPage
{

    public LiveDataPage(LiveDataViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();

    }
    
}