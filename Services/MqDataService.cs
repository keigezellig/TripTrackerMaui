using CommunityToolkit.Mvvm.Messaging;
using MauiApp1.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MauiApp1.Services.MessageProcessing;

namespace MauiApp1.Services
{
    public class MessageQueueDataService: IDataService
    {
        private readonly IMessageQueueProvider _mqProvider;
        private bool _disposedValue;
        private ILogger<MessageQueueDataService> _logger;
        private readonly ExternalMessageProcessorFactory _mpFactory;

        public MessageQueueDataService(IMessageQueueProvider mqProvider, ILogger<MessageQueueDataService> logger, ExternalMessageProcessorFactory mpFactory ) 
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
