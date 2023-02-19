using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace Outbox.Job.Infrastructure
{
    internal class AzureSenderFactory
    {
        private Dictionary<string, ServiceBusSender> _senders = new Dictionary<string, ServiceBusSender>();
        private readonly ServiceBusClient _client;
        private readonly ServiceBusAdministrationClient _adminClient;

        public AzureSenderFactory(ServiceBusClient client, DefaultAzureCredentialOptions options)
        {
            _client = client;

            _adminClient = new ServiceBusAdministrationClient(
                "pv-authorization.servicebus.windows.net",
                new DefaultAzureCredential(options));
        }

        public async Task<ServiceBusSender> GetSenderAsync(string eventName)
        {
            if (_senders.TryGetValue(eventName, out var sender))
                return sender;

            if (!await _adminClient.TopicExistsAsync(eventName))
                await _adminClient.CreateTopicAsync(eventName);

            sender = _client.CreateSender(eventName);
            _senders.Add(eventName, sender);
            return sender;
        }
    }
}
