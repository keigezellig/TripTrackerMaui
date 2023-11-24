using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;

using TripTracker.Models;
using TripTracker.Services.MessageProcessing;
using TripTracker.Services.MessageQueue;
using TripTracker.Services.SettingsService;

namespace TripTracker.Services.DataService
{
    public class MessageQueueDataService : IDataService
    {
        private readonly IMessageQueueProvider _mqProvider;
        private bool _disposedValue;
        private ILogger<MessageQueueDataService> _logger;
        private readonly MessageProcessorFactory _mpFactory;
        private readonly ISettingsService _settingsService;

        public MessageQueueDataService(IMessageQueueProvider mqProvider,
                                       ILogger<MessageQueueDataService> logger,
                                       MessageProcessorFactory mpFactory,
                                       ISettingsService settingsService)
        {
            _mqProvider = mqProvider;
            _mqProvider.MessageReceived += MqProvider_MessageReceived;
            _logger = logger;
            _mpFactory = mpFactory;
            _settingsService = settingsService;
        }


        private void MqProvider_MessageReceived(object sender, MessageEventArgs e)
        {
            _logger.LogTrace($"Data received from topic {e.TopicName} -> {e.Message}");
            if (e.TopicName == "live/trip")
            {
                var processor = _mpFactory.GetMessageProcessor(e.Message);
                processor.Process(e.Message);
            }
        }

        public bool IsStarted => _mqProvider.IsConnected;

        public async Task Start()
        {
            await _mqProvider.Connect(_settingsService.MessageQueueHost, -1);
            await _mqProvider.Subscribe(new[] { "live/trip" });


            OnStarted();
        }

        public async Task Stop()
        {
            await _mqProvider.Disconnect();
            OnStopped();
        }

        protected virtual void OnStarted()
        {

            WeakReferenceMessenger.Default.Send(new StatusMessage(true));
        }

        protected virtual void OnStopped()
        {
            WeakReferenceMessenger.Default.Send(new StatusMessage(false));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _mqProvider.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
