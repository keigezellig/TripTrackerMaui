using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using TripTracker.Models;
using TripTracker.Services.DataService;

namespace TripTracker.ViewModels
{
    public class MainViewModel : ObservableRecipient
    {
        private readonly IDataService _dataService;

        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            IsActive = true;


            StartCommand = new AsyncRelayCommand(() =>
            {

                return dataService.Start();

            }, () => !dataService.IsStarted);
            StopCommand = new AsyncRelayCommand(() =>
            {
                return dataService.Stop();

            }, () => dataService.IsStarted);
        }

        public IAsyncRelayCommand StartCommand { get; }

        public IAsyncRelayCommand StopCommand { get; }

        protected override void OnActivated()
        {
            Messenger.Register<MainViewModel, StatusMessage>(this, (r, m) => MainThread.BeginInvokeOnMainThread(() => r.UpdateStatus(m)));

        }

        private void UpdateStatus(StatusMessage message)
        {

            (StartCommand as AsyncRelayCommand).NotifyCanExecuteChanged();
            (StopCommand as AsyncRelayCommand).NotifyCanExecuteChanged();

        }
    }
}
