using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UserDataHub.Consumer.Core.Interfaces;

namespace UserDataHubConsumer.Service;

public abstract class RabbitMQService : IRabbitMQService, IDisposable
{
    protected readonly IServiceProvider _services;
    protected readonly IConfiguration _configuration;

    private readonly IConnection _connection;
    private readonly IModel _channel;

    private readonly string RouteKey;
    private readonly string QueueName;
    private readonly string eventExchange;

    public RabbitMQService(IServiceProvider services,
            IConfiguration configuration,
            string routeKey,
            string queueName)
    {
        _services = services;
        _configuration = configuration;
        RouteKey = routeKey;
        QueueName = queueName;

        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration.GetSection("RabbitMQSettings:HostName").Value,
                UserName = _configuration.GetSection("RabbitMQSettings:UserName").Value,
                Password = _configuration.GetSection("RabbitMQSettings:Password").Value,
                Port = int.Parse(_configuration.GetSection("RabbitMQSettings:Port").Value)
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            eventExchange = _configuration.GetSection("RabbitMQSettings:EventExchange").Value;

            _channel.ExchangeDeclare(eventExchange, ExchangeType.Fanout, durable: true, autoDelete: false);
            _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queueName, eventExchange, string.Empty);
        }
        catch (Exception ex)
        {
            throw new Exception($"RabbitMQConsumer init failed: {ex.Message}", ex);
        }
    }

    public void ReadMessages()
    {
        var queueNameFormat = _configuration.GetSection("RabbitMQSettings:QueueName").Value;
        var queueName = string.Format(queueNameFormat, QueueName);

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
        _channel.BasicConsume(queueName, false, consumer);
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
