using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace TinyServiceBusExplorer.Core.Services
{
    public interface IServiceBusService
    {
        Task Init(string connectionString);
        Task<List<string>> GetQueues();
        Task<List<ServiceBusReceivedMessage>> Peek(string queueName);
    }
}
