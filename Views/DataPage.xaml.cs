namespace MauiApp1.Views
{
    public partial class DataPage : ContentPage
    {        

        public DataPage(DataViewModel dataViewModel)
        {
            BindingContext = dataViewModel;
            InitializeComponent();
        }
        
    }
}