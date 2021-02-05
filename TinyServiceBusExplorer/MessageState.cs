using System;
using System.Collections.Generic;
using Azure.Messaging.ServiceBus;

namespace TinyServiceBusExplorer
{
    public static class MessageState
    {
        public static List<ServiceBusReceivedMessage> Messages { get; set; } = new List<ServiceBusReceivedMessage>();
    }
}
