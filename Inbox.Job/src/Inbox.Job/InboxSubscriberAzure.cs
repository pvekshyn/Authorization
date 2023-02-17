using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Inbox.Job.Infrastructure
{
    public class InboxSubscriberAzure : IInboxSubscriber
    {
        private readonly IInboxRepository _inboxRepository;
        private readonly ILogger<InboxSubscriberAzure> _logger;
        private string _subscription;
        private List<string> _topics;
        private readonly ServiceBusAdministrationClient _adminClient;

        public InboxSubscriberAzure(
            IInboxRepository inboxRepository,
            IOptions<InboxSettings> settings,
            ILogger<InboxSubscriberAzure> logger)
        {
            _inboxRepository = inboxRepository;
            _logger = logger;
            _subscription = settings.Value.PubSub.EventProcessingServiceName;
            _topics = settings.Value.PubSub.Events;
            _adminClient = new ServiceBusAdministrationClient(
                "pv-authorization.servicebus.windows.net",
                new DefaultAzureCredential());
        }

        public async Task SubscribeAsync()
        {
            var client = new ServiceBusClient(
                "pv-authorization.servicebus.windows.net",
                new DefaultAzureCredential());

            foreach (var topic in _topics)
            {
                _logger.LogInformation($"Subscribing to {topic}");

                if (!await _adminClient.TopicExistsAsync(topic))
                    await _adminClient.CreateTopicAsync(topic);

                if (!await _adminClient.SubscriptionExistsAsync(topic, _subscription))
                    await _adminClient.CreateSubscriptionAsync(topic, _subscription);

                var options = new ServiceBusProcessorOptions
                {
                    AutoCompleteMessages = false,
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                };

                var processor = client.CreateProcessor(topic, _subscription, options);

                try
                {
                    processor.ProcessMessageAsync += MessageHandler;

                    processor.ProcessErrorAsync += ErrorHandler;

                    await processor.StartProcessingAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string message = args.Message.Body.ToString();

            //ToDo pass create date
            _inboxRepository.Insert(message, DateTime.UtcNow);

            await args.CompleteMessageAsync(args.Message);
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception.ToString());
            return Task.CompletedTask;
        }

    }
}
