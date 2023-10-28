using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.ViewModels
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
            Messenger.Register<MainViewModel, StatusMessage>(this, (r, m) => MainThread.BeginInvokeOnMainThread( () => r.UpdateStatus(m)));
            
        }

        private void UpdateStatus(StatusMessage message)
        {
            
            (StartCommand as AsyncRelayCommand).NotifyCanExecuteChanged();
            (StopCommand as AsyncRelayCommand).NotifyCanExecuteChanged();

        }
    }
}
