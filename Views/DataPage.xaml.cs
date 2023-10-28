namespace MauiApp1
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