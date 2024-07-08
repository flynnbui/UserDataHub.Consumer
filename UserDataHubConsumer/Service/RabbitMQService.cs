using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using UserDataHub.Consumer.Core.Interfaces;

namespace UserDataHubConsumer.Service;

public class RabbitMQService : IRabbitMQService
{
    private readonly IModel _model;
    private readonly IConnection _connection;
    private readonly ConnectionFactory _factory;
    private readonly string _queueName;
    public RabbitMQService(ConnectionFactory factory, string queueName)
    {
        _factory = factory;
        _queueName = queueName;
        _connection = _factory.CreateConnection();
        _model = _connection.CreateModel();
        _model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
        _model.QueueBind(_queueName, "register", string.Empty);
    }
    public void ReadMessages()
    {
        Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        var consumer = new EventingBasicConsumer(_model);
        consumer.Received += (model, message) =>
        {
            var body = message.Body.ToArray();
            var message1 = Encoding.UTF8.GetString(body);
            var messageBody = JsonSerializer.Serialize(message1);
            Console.WriteLine(messageBody);
            HandleMessage(messageBody);
            _model.BasicAck(message.DeliveryTag, false);
        };
        _model.BasicConsume(_queueName, false, consumer);
    }

    private static void HandleMessage (string message)
    {
        // Xử lý thông điệp
        Console.WriteLine("Processing message: " + message);
        // Thêm logic xử lý của bạn ở đây
    }

    public void Dispose()
    {
        if (_model.IsOpen)
            _model.Close();
        if (_connection.IsOpen)
            _connection.Close();
    }


}
