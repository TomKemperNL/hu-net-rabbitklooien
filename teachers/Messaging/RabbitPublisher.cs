using System.Text.Json;
using RabbitMQ.Client;

namespace HU.Inno.Teachers.Messaging;

public class RabbitPublisher<T> : IPublisher<T>
{

    public const string DefaultExchange = "";

    private readonly string _queue;
    private readonly IModel _channel;
    
    public RabbitPublisher(String queue, IModel channel)
    {
        _queue = queue;
        this._channel = channel;
    }
    
    public void Publish(T message)
    {
        byte[] messageBodyBytes = JsonSerializer.SerializeToUtf8Bytes(message);
        IBasicProperties props = _channel.CreateBasicProperties();
        props.ContentType = "application/json";
        _channel.BasicPublish(DefaultExchange, _queue, true, props, messageBodyBytes);
    }
}