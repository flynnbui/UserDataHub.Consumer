using UserDataHub.Consumer.Core.Receiver;
using UserDataHubConsumer.Service;

namespace UserDataHub.Consumer.Core.Services
{
    public class RabbitMQHosted : RabbitMQService
    {
        public RabbitMQHosted(IServiceProvider services, IConfiguration configuration)
            : base(services, configuration, UserDataReceiver.RouteKey, UserDataReceiver.QueueName)
        { }

        protected override async Task<bool> ProcessAsync(string message)
        {
            var userDataTransfer = GetMessageInfo<UserDataReceiver>(message);
            if (userDataTransfer == null)
            {
                return false;
            }
            var userDataList = userDataTransfer.UserDataList;
            try
            {
                await Task.Run(() => Console.WriteLine(message)); // Simulate an async operation
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


    }
}
