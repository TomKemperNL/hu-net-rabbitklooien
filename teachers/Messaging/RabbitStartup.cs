using RabbitMQ.Client;
using Teachers.Inno.HU.Domain;

namespace HU.Inno.Teachers.Messaging;

public class RabbitStartup
{
    private ConnectionFactory _connectionFactory = new ConnectionFactory();
    private IConnection? _connection;

    public const string TeacherCreatedQueue = "TeacherCreated";

    private void EnsureConnected()
    {
        _connection ??= this._connectionFactory.CreateConnection();
    }

    public void ConfigureQueues()
    {
        EnsureConnected();
        using (IModel channel = _connection.CreateModel())
        {
            channel.QueueDeclare(TeacherCreatedQueue, true, false, false);
        }
    }


    private List<IDisposable> _consumers = new List<IDisposable>();
    
    public void ConfigureSubscribers(IServiceProvider appServices)
    {
        IConnection mainConnection = appServices.GetService<IConnection>();
        
        RabbitConsumer<TeacherCreated> createdListener = new RabbitConsumer<TeacherCreated>(mainConnection,
            appServices,
            TeacherCreatedQueue,
            (services, tc) =>
            {
                Console.WriteLine("Created: " + tc.Email);
            });
        
        _consumers.Add(createdListener);
        Console.WriteLine("Starting MessageProcessors");
        createdListener.Start();
    }

    public void ConfigurePublishers(IServiceCollection builderServices)
    {
        EnsureConnected();
        // https://www.rabbitmq.com/client-libraries/dotnet-api-guide#connection-and-channel-lifespan
        // Een connection mag over de applicatie geshared worden
        builderServices.AddSingleton(_connection);
        
        // Maar een channel mag niet door meerdere publishers of consumers tegelijk gebruikt worden.
        // AddScoped zorgt voor 1 dedicated channel per request. En een request wordt gewoonlijk serieel afgehandeld.
        builderServices.AddScoped<IModel>(s => { return s.GetService<IConnection>()!.CreateModel(); });
        
        // De Publisher zelf is nu transient (dus twee losse services in hetzelfde request krijgen eigen instances), 
        // dat is misschien niet optimaal: AddScoped kan hier vast ook. Maar ik default naar 'zo min mogelijk delen',
        // en deze kleine classjes zijn geen dure resources (in tegenstelling tot de channel of connection).
        builderServices.AddTransient<IPublisher<TeacherCreated>>(s =>
            new RabbitPublisher<TeacherCreated>(
                TeacherCreatedQueue, s.GetService<IModel>()!
            ));
    }
}