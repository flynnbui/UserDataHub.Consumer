﻿using UserDataHub.Consumer.Core.Receiver;
using UserDataHub.Consumer.Core.Settings;
using UserDataHubConsumer.Service;
using UserDataHub.Consumer.Core.Settings;

namespace UserDataHub.Consumer.Core.Services
{
    public class RabbitMQHosted : RabbitMQService
    {
        public RabbitMQHosted(IServiceProvider services, IConfiguration configuration)
            : base(services, configuration, RabbitMQSettings.RouteKey, RabbitMQSettings.QueueName)
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
                // Simulate an async operation
                await Task.Run(() => Console.WriteLine("Hello" + message));
                // Add more logic to process message here
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


    }
}
