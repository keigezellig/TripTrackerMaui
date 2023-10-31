using CommunityToolkit.Mvvm.Messaging;
using MauiApp1.Models;
using MauiApp1.Services.MessageProcessing;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.DataService
{
    public class MessageQueueDataService: IDataService
    {
        private readonly IMessageQueueProvider _mqProvider;
        private bool _disposedValue;
        private ILogger<MessageQueueDataService> _logger;
        private readonly MessageProcessorFactory _mpFactory;

        public MessageQueueDataService(IMessageQueueProvider mqProvider, ILogger<MessageQueueDataService> logger, MessageProcessorFactory mpFactory ) 
        {
            _mqProvider = mqProvider;
            _mqProvider.MessageReceived += MqProvider_MessageReceived;            
            _logger = logger;
            _mpFactory = mpFactory;
        }


        private void MqProvider_MessageReceived(object sender, MessageEventArgs e)
        {
            _logger.LogDebug($"Data received from topic {e.TopicName} -> {e.Message}");
            var processor = _mpFactory.GetMessageProcessor(e.Message);
            processor.Process(e.Message);
        }

        public bool IsStarted => _mqProvider.IsConnected;

        public async Task Start()
        {
            //TODO: hardcoded
            await _mqProvider.Connect("192.168.2.71", 1883);
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
