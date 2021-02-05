using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using TinyServiceBusExplorer.Core.Services;
using Xunit;

namespace TinyServiceBusExplorer.Core.Tests
{
    public class ServiceBusExplorerTests
    {
        [Fact]
        public async Task GetQueueName()
        {
            var settings = new Settings();
            settings.InitSettings();

            var service = new ServiceBusService();
            await service.Init(settings.ConnectionString);
            var queues = await service.GetQueues();

            Assert.True(queues.Count == 2);
        }

        [Fact]
        public async Task GetQueueCount()
        {
            var settings = new Settings();
            settings.InitSettings();

            var client = new ServiceBusClient(settings.ConnectionString);
            var sender = client.CreateSender("produkter");

            await sender.SendMessageAsync(new ServiceBusMessage("hello"));
           
            await Task.Delay(1000);

            var service = new ServiceBusService();
            await service.Init(settings.ConnectionString);

            var countResult = await service.MessageCount("produkter");

            var receiver = client.CreateReceiver("produkter");
            var messages = receiver.ReceiveMessagesAsync();
            var enumerator = messages.GetAsyncEnumerator();
            int count = 0;

            var ids = new List<string>();

            while(true)
            {
                await enumerator.MoveNextAsync();

                if(ids.Contains(enumerator.Current.MessageId))
                {
                    break;
                }

                ids.Add(enumerator.Current.MessageId);
                count++;
            }

            Assert.True(countResult == count);

        }
    }
}
