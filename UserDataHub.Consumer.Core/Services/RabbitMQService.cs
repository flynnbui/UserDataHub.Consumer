using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using UserDataHub.Consumer.Core.Helper;
using UserDataHub.Consumer.Core.Interfaces;
using UserDataHub.Consumer.Core.Settings;

namespace UserDataHubConsumer.Service;

public abstract class RabbitMQService : IRabbitMQService, IDisposable
{
    protected readonly IServiceProvider _services;

    private readonly IConnection _connection;
    private readonly IModel _channel;

    private readonly string _routeKey;
    private string _queue;
    private readonly string eventExchange;

    public RabbitMQService(IServiceProvider services,
            IConfiguration configuration,
            string routeKey,
            string queueName)
    {
        _services = services;
        _routeKey = routeKey;
        _queue = queueName;
        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = ConfigurationHelper.GetConfiguration(configuration, "RabbitMQSettings:HostName"),
                UserName = ConfigurationHelper.GetConfiguration(configuration, "RabbitMQSettings:UserName"),
                Password = ConfigurationHelper.GetConfiguration(configuration, "RabbitMQSettings:Password"),
                Port = int.Parse(ConfigurationHelper.GetConfiguration(configuration, "RabbitMQSettings:Port"))
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            eventExchange = ConfigurationHelper.GetConfiguration(configuration, "RabbitMQSettings:EventExchange");

            _channel.QueueDeclare(_queue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(_queue, eventExchange, _routeKey);

            

        }
        catch (Exception ex)
        {
            throw new Exception($"RabbitMQConsumer init failed: {ex.Message}", ex);
        }
    }

    public void ReadMessages()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, message) =>
        {
            var body = message.Body.ToArray();
            var messageContent = Encoding.UTF8.GetString(body);
            var processResult = await ProcessAsync(messageContent);
            if (processResult)
            {
                _channel.BasicAck(message.DeliveryTag, false);
            }
            else
            {
                // Implement a retry mechanism or log the error
            }
        };

        _channel.BasicConsume(_queue, false, consumer);

    }

    public void Dispose()
    {
        if (_channel != null && _channel.IsOpen)
            _channel.Close();
        if (_connection != null && _connection.IsOpen)
            _connection.Close();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        ReadMessages();
        return Task.CompletedTask;
    }

    protected abstract Task<bool> ProcessAsync(string message);

    protected T? GetMessageInfo<T>(string message) where T : class
    {
        if (string.IsNullOrEmpty(message))
            return null;

        try
        {
            return JsonSerializer.Deserialize<T>(message);
        }
        catch (JsonException ex)
        {
            return null;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();
        return Task.CompletedTask;
    }
}
