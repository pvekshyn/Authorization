using Azure.Messaging.ServiceBus;

namespace Outbox.Job.Infrastructure
{
    internal class AzureSenderFactory
    {
        private Dictionary<string, ServiceBusSender> _senders = new Dictionary<string, ServiceBusSender>();

        public ServiceBusSender GetSender(string eventName, ServiceBusClient client)
        {
            if (_senders.TryGetValue(eventName, out var sender))
                return sender;

            sender = client.CreateSender(eventName);
            _senders.Add(eventName, sender);
            return sender;
        }
    }
}
