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

    public void ConfigureSubscribers(IServiceProvider appServices)
    {
        IConnection mainConnection = appServices.GetService<IConnection>();
        RabbitConsumer<TeacherCreated> createdListener = new RabbitConsumer<TeacherCreated>(mainConnection,
            TeacherCreatedQueue,
            tc =>
            {
                Console.WriteLine("Created: " + tc.Email);
            });
        Console.WriteLine("Starting MessageProcessors");
        createdListener.Start();
    }

    public void ConfigurePublishers(IServiceCollection builderServices)
    {
        EnsureConnected();
        builderServices.AddSingleton(_connection);
        builderServices.AddScoped<IModel>(s => { return s.GetService<IConnection>()!.CreateModel(); });
        builderServices.AddTransient<IPublisher<TeacherCreated>>(s =>
            new RabbitPublisher<TeacherCreated>(
                TeacherCreatedQueue, s.GetService<IModel>()!
            ));
    }
}