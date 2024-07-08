namespace UserDataHub.Consumer.Core.Interfaces
{
    public interface IRabbitMQService: IDisposable
    {
        void ReadMessages();
    }
}
