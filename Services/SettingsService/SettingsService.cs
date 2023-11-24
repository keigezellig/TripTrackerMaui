using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.SettingsService;

public class SettingsService : ISettingsService
{
    private readonly ILogger<SettingsService> _logger;

    private const string MessageQueueHostKey = "messageQueueHost";

    public string MessageQueueHost
    {
        get => Preferences.Get(MessageQueueHostKey, "localhost");
        set
        {
            _logger.LogInformation($"Saving {MessageQueueHostKey}");
            Preferences.Set(MessageQueueHostKey, value);
        }
    }

    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;
    }

}