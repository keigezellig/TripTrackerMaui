using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public interface IMessageQueueProvider : IDisposable
    {
        bool IsConnected { get;}

        event EventHandler<MessageEventArgs> MessageReceived;
        Task Connect(string host, int port);
        Task Disconnect();

        Task Subscribe(string topicName);

    }

    public class MessageEventArgs : EventArgs 
    { 
        public string TopicName { get;}
        public string Message { get;}

        public MessageEventArgs(string topicName, string message)
        {
            TopicName = topicName;
            Message = message;
        }
    }
}
