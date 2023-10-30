using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Exceptions;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class MqttProvider : IMessageQueueProvider
    {
        private MqttFactory _mqttFactory;
        private IMqttClient _mqttClient;
        private ILogger _logger;
        private bool disposedValue;

        public bool IsConnected => _mqttClient.IsConnected;

        public event EventHandler<MessageEventArgs> MessageReceived;

        public MqttProvider(ILogger<MqttProvider> logger)
        {
            _mqttFactory = new MqttFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();
            _mqttClient.ApplicationMessageReceivedAsync += _mqttClient_ApplicationMessageReceivedAsync;
            _logger = logger;
        }
        

        public async Task Connect(string host, int port)
        {
            _logger.LogInformation("Connecting..");
            
            var mqttClientOptions = new MqttClientOptionsBuilder()
                                    .WithTcpServer(host)
                                    .Build();

                if (_mqttClient.IsConnected)
                {
                    _logger.LogInformation("Already connected.");
                    return;
                }

            try
            {
                using (var timeoutToken = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    var bla = await _mqttClient.ConnectAsync(mqttClientOptions, timeoutToken.Token);
                    _logger.LogInformation("Connection result: {0}", bla.ResultCode);
                    
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogError("Timeout while connecting.");
            }

            catch (Exception ex)
            {
                _logger.LogError("Other error while connecting: {0}",ex);
            }


        }

        private Task _mqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {

            var message = Encoding.UTF8.GetString(arg.ApplicationMessage.PayloadSegment);
            var topic = arg.ApplicationMessage.Topic;            
            
            OnMessageReceived(topic, message);
            return Task.CompletedTask;
        }

        public async Task Disconnect()
        {
            if (!_mqttClient.IsConnected)
            {
                _logger.LogError("Not connected!");
                return;
            }

            var options = new MqttClientDisconnectOptionsBuilder()
                .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                .Build();
            
            await _mqttClient.DisconnectAsync(options);
            _logger.LogInformation("Disconnected");
        }

        public async Task Subscribe(string[] topicNames)
        {
            if (!_mqttClient.IsConnected)
            {
                _logger.LogError("Not connected!");
                return;
            }
            var mqttSubscribeOptionsBuilder = _mqttFactory.CreateSubscribeOptionsBuilder();
            foreach (var topic in topicNames)
            {
                mqttSubscribeOptionsBuilder = mqttSubscribeOptionsBuilder.WithTopicFilter(f => f.WithTopic(topic));
            }

            var result = await _mqttClient.SubscribeAsync(mqttSubscribeOptionsBuilder.Build(), CancellationToken.None);
            var subscribedTo = String.Join(",", result.Items.Select(i => i.TopicFilter.Topic));
            _logger.LogInformation($"Subscribed to {subscribedTo}");
            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _mqttClient.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnMessageReceived(string topic, string message)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<MessageEventArgs> eventHandler = MessageReceived;

            // Event will be null if there are no subscribers
            if (eventHandler != null)
            {
                // Format the string to send inside the CustomEventArgs parameter
                var eventData = new MessageEventArgs(topic, message);

                // Call to raise the event.
                eventHandler(this, eventData);
            }
        }


    }
}
