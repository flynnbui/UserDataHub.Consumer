using UserDataHub.Consumer.Core.Interfaces;
namespace UserDataHub.Consumer.Core.Services
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly IRabbitMQService _rabbitMQService;
        private List<string> _messageStore = new List<string>();

        public ConsumerHostedService(IRabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            // _rabbitMQService.ReadMessages();

        }
        public List<string> getAllMessage()
        {
            return _messageStore;
        }
    }
}
