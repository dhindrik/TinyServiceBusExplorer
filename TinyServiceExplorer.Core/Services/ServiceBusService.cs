using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace TinyServiceBusExplorer.Core.Services
{
    public class ServiceBusService : IServiceBusService
    {
        private ServiceBusClient client;
        private ServiceBusAdministrationClient adminClient;

        public ServiceBusService()
        {
        }

        public async Task<List<string>> GetQueues()
        {
            if(adminClient == null)
            {
                throw new Exception("You must run init first");
            }

            var queues = adminClient.GetQueuesAsync();
            var enumerator = queues.GetAsyncEnumerator();

            var queueNames = new List<string>();

            while(true)
            {
                await enumerator.MoveNextAsync();

                if(enumerator.Current == null || queueNames.Contains(enumerator.Current.Name))
                {
                    break;
                }

                queueNames.Add(enumerator.Current.Name);
            }

            return queueNames;
        }

        public Task Init(string connectionString)
        {
            client = new ServiceBusClient(connectionString);
            adminClient = new ServiceBusAdministrationClient(connectionString);

            return Task.CompletedTask;
        }

        public async Task<List<ServiceBusReceivedMessage>> Peek(string queueName)
        {
            if (client == null)
            {
                throw new Exception("You must run init first");
            }

            var receiver = client.CreateReceiver(queueName);

            var messages = await receiver.PeekMessagesAsync(100);

            return messages.ToList();
        }

        public async Task<List<ServiceBusReceivedMessage>> PeekDeadLetter(string queueName)
        {
            if (client == null)
            {
                throw new Exception("You must run init first");
            }
            
            var receiver = client.CreateReceiver(queueName, new ServiceBusReceiverOptions()
            {
                SubQueue = SubQueue.DeadLetter
            });

            var messages = await receiver.PeekMessagesAsync(100);
            
            return messages.ToList();
        }

        public async Task AddToDeadLetter(string queueName, ServiceBusReceivedMessage message)
        {
            if (client == null)
            {
                throw new Exception("You must run init first");
            }

            var receiver = client.CreateReceiver(queueName, new ServiceBusReceiverOptions()
            {
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            });

            var messages = await receiver.ReceiveMessagesAsync(100);

            var messageToAdd = messages.Single(x => x.MessageId == message.MessageId && x.EnqueuedTime == message.EnqueuedTime);

            await receiver.DeadLetterMessageAsync(messageToAdd);

            foreach(var msg in messages)
            {
                if(msg == messageToAdd)
                {
                    continue;
                }

                await receiver.AbandonMessageAsync(msg);
            }

            await receiver.DisposeAsync();
        }

        public async Task Resend(string queueName, ServiceBusReceivedMessage message)
        {
            try
            {
                var receiver = client.CreateReceiver(queueName, new ServiceBusReceiverOptions()
                {
                    SubQueue = SubQueue.DeadLetter,
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                });

                var messages = await receiver.ReceiveMessagesAsync(100);

                var sender = client.CreateSender(queueName);

                var messageToAdd = new ServiceBusMessage()
                {
                    SessionId = message.SessionId,
                    Body = message.Body
                };

                await sender.SendMessageAsync(messageToAdd);

                var oldMessage = messages.Single(x => x.MessageId == message.MessageId && x.EnqueuedTime == message.EnqueuedTime);

                await receiver.CompleteMessageAsync(oldMessage);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
 