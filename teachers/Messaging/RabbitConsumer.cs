using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HU.Inno.Teachers.Messaging;

public class RabbitConsumer<T>
{
    private readonly IConnection _rabbitConnection;
    private readonly string _queue;
    private readonly Action<T> _consumer;

    public RabbitConsumer(IConnection rabbitConnection, string queue, Action<T> consumer)
    {
        _rabbitConnection = rabbitConnection;
        _queue = queue;
        _consumer = consumer;
    }

    public void Start()
    {
        IModel channel = _rabbitConnection.CreateModel();

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (ch, ea) =>
        {
            Console.WriteLine("Receiving");
            var body = ea.Body.ToArray();
            T message = JsonSerializer.Deserialize<T>(body)!;
            _consumer(message);
        };
        Console.WriteLine("Consuming");
        channel.BasicConsume(_queue, true, consumer);
        Console.WriteLine("???");
    }
}