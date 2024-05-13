using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HU.Inno.Teachers.Messaging;

public class RabbitConsumer<T> : IDisposable
{
    private readonly IConnection _rabbitConnection;
    private readonly IServiceProvider _appServices;
    private readonly string _queue;
    private readonly Action<IServiceProvider, T> _consumer;

    public RabbitConsumer(IConnection rabbitConnection, IServiceProvider appServices, string queue, Action<IServiceProvider, T> consumer)
    {
        _rabbitConnection = rabbitConnection;
        _appServices = appServices;
        _queue = queue;
        _consumer = consumer;
    }

    private IModel? _channel;
    private EventingBasicConsumer? _eventingBasicConsumer;
    
    public void Start()
    {
        _channel = _rabbitConnection.CreateModel();
        _eventingBasicConsumer = new EventingBasicConsumer(_channel);
        _eventingBasicConsumer.Received += (ch, ea) =>
        {
            Console.WriteLine("Receiving");
            var body = ea.Body.ToArray();
            T message = JsonSerializer.Deserialize<T>(body)!;

            using (var scope = _appServices.CreateScope())
            {
                _consumer(scope.ServiceProvider, message);    
            }
        };
        Console.WriteLine("Consuming");
        _channel.BasicConsume(_queue, true, _eventingBasicConsumer);
        Console.WriteLine("???");
    }

    public void Dispose()
    {
        _rabbitConnection.Dispose();
        _channel.Dispose();
    }
}