using MauiApp1.ViewModels;

namespace MauiApp1.Views
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }

    }
}