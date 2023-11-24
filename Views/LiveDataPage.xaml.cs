using TripTracker.ViewModels;

namespace TripTracker.Views;

public partial class LiveDataPage : ContentPage
{

    public LiveDataPage(LiveDataViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();

    }

}