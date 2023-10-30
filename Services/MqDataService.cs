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

namespace MauiApp1.Services
{
    public class MessageQueueDataService: IDataService
    {
        private readonly IMessageQueueProvider _mqProvider;
        private bool _disposedValue;
        private ILogger<MessageQueueDataService> _logger;

        public MessageQueueDataService(IMessageQueueProvider mqProvider, ILogger<MessageQueueDataService> logger) 
        {
            _mqProvider = mqProvider;
            _mqProvider.MessageReceived += MqProvider_MessageReceived;            
            _logger = logger;
        }

        
        private void MqProvider_MessageReceived(object sender, MessageEventArgs e)
        {
            _logger.LogInformation($"Data received from topic {e.TopicName} -> {e.Message}");
            if (e.TopicName == "data/raw/gps")
            {
                SendGpsMessage(e.Message);                
            }

            if (e.TopicName == "data/raw/obd")
            {
                SendVehicleMessage(e.Message);
            }

        }

        private void SendVehicleMessage(string message)
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var deserializedMsg = JsonSerializer.Deserialize<VehicleJsonMessage>(message, options);
            var vehicleMessage = ParseVehicleMessage(deserializedMsg);
            WeakReferenceMessenger.Default.Send(vehicleMessage);
        }

        private VehicleMessage ParseVehicleMessage(VehicleJsonMessage deserializedMsg)
        {
            switch (deserializedMsg.q)
            {
                case "speed":
                    return new VehicleSpeedMessage(deserializedMsg.v);
                case "rpm":
                    return new VehicleRpmMessage(deserializedMsg.v);
                case "coolant_temp":
                    return new VehicleCoolantTempMessage(deserializedMsg.v);
                default:
                    _logger.LogWarning($"Unknown vehicle quantity {deserializedMsg.q}");
                    return new VehicleUnknownMessage(deserializedMsg.v);
            }
        }

        private void SendGpsMessage(string msg)
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var deserializedMsg = JsonSerializer.Deserialize<GpsJsonMessage>(msg, options);
            GpsMessage dataMessage = new GpsMessage(
                    location: new Location(deserializedMsg.P[0], deserializedMsg.P[1], deserializedMsg.T),
                    gpsSpeed: deserializedMsg.V != null ? (int)Math.Round(deserializedMsg.V.Value * 3.6) : -1,
                    fixQuality: (GpsMessage.FixQuality)deserializedMsg.Q);

            WeakReferenceMessenger.Default.Send(dataMessage);
        }

        public bool IsStarted => _mqProvider.IsConnected;

        public async Task Start()
        {
            //TODO: hardcoded
            await _mqProvider.Connect("192.168.2.71", 1883);
            await _mqProvider.Subscribe(new[] { "data/raw/gps", "data/raw/obd" });
            
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
