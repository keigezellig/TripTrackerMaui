using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.Messaging;
using MauiApp1.Services.MessageProcessing.JsonMessages;
using Microsoft.Extensions.Logging;

namespace MauiApp1.Services.MessageProcessing;

public abstract class MessageProcessor<TMessage, TModel> : IExternalMessageProcessor where TMessage : class
                                                                                     where TModel: class 
{
    protected ILogger<MessageProcessor<TMessage, TModel>> _logger;

    protected MessageProcessor(ILogger<MessageProcessor<TMessage, TModel>> logger)
    {
        _logger = logger;
    }
    public void Process(string message)
    {
        var deserializedMessage = Deserialize(message);
        var modelData = ConvertToModel(deserializedMessage);
        SendModelData(modelData);
        
    }

    private TMessage Deserialize(string message)
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        
        return JsonSerializer.Deserialize<TMessage>(message, options);
    }

    protected abstract TModel ConvertToModel(TMessage deserializedMessage);

    private void SendModelData(TModel data)
    {
        _logger.LogInformation($"Sending {typeof(TModel).FullName} ");
        WeakReferenceMessenger.Default.Send(data);
    }
}