using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services.SettingsService;
using Microsoft.Extensions.Logging;


namespace MauiApp1.ViewModels;

public partial class SettingsViewModel : ObservableValidator
{
    [ObservableProperty] 
    [NotifyDataErrorInfo]
    [Required]
    private string _messageQueueHost;

    private readonly ISettingsService _settingsService;
    private readonly ILogger<SettingsViewModel> _logger;


    public SettingsViewModel(ISettingsService settingsService, ILogger<SettingsViewModel> logger)
    {
        _settingsService = settingsService;
        _logger = logger;
        MessageQueueHost = _settingsService.MessageQueueHost;
        
        

    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void SaveSettings()
    {
        _logger.LogDebug("Saving settings");
        _settingsService.MessageQueueHost = MessageQueueHost;
    }

    private bool CanSave()
    {
        return !HasErrors;
    }
}