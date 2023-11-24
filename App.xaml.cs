using Microsoft.Maui.Controls.Handlers.Items;

namespace TripTracker
{
    public partial class App : Application
    {

        public App(IServiceProvider provider)
        {
            InitializeComponent();
            CollectionViewHandler.Mapper.AppendToMapping("HeaderAndFooterFix", (_, collectionView) =>
            {
                collectionView.AddLogicalChild(collectionView.Header as Element);
                collectionView.AddLogicalChild(collectionView.Footer as Element);
            });
            MainPage = new AppShell();
        }
    }
}