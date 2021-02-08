using System;
namespace TinyServiceBusExplorer
{
    public class QueueChangedEventArgs : EventArgs
    {
        public QueueChangedEventArgs(string queueName)
        {
            QueueName = queueName;
        }

        public string QueueName { get; }
    }
}
