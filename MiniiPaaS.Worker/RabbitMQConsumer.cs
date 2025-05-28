using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Mini-iPaaS.Worker
{
    public class RabbitMQConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQConsumer()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost", // RabbitMQ sunucu adresi
            Port = 5672,            // RabbitMQ portu
            UserName = "guest",     // Varsayılan kullanıcı adı
            Password = "guest"      // Varsayılan şifre
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Kuyruğu tanımla
        _channel.QueueDeclare(
            queue: "miniipaaS",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");
        };

        _channel.BasicConsume(
            queue: "miniipaaS",
            autoAck: true,
            consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}
}