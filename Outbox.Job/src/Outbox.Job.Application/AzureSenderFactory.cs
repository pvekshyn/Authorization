using Azure.Messaging.ServiceBus;

namespace Outbox.Job.Infrastructure
{
    internal class AzureSenderFactory
    {
        private Dictionary<string, ServiceBusSender> _senders = new Dictionary<string, ServiceBusSender>();
        private readonly ServiceBusClient _client;

        public AzureSenderFactory(ServiceBusClient client)
        {
            _client = client;
        }

        public ServiceBusSender GetSender(string eventName)
        {
            if (_senders.TryGetValue(eventName, out var sender))
                return sender;

            sender = _client.CreateSender(eventName);
            _senders.Add(eventName, sender);
            return sender;
        }
    }
}
