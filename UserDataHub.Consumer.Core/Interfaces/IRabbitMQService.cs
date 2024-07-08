namespace UserDataHub.Consumer.Core.Interfaces
{
    public interface IRabbitMQService : IHostedService
    {
        void ReadMessages();
    }
}
