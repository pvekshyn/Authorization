using Azure.Identity;
using Azure.Messaging.ServiceBus;
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

        public InboxSubscriberAzure(
            IInboxRepository inboxRepository,
            IOptions<InboxSettings> settings,
            ILogger<InboxSubscriberAzure> logger)
        {
            _inboxRepository = inboxRepository;
            _logger = logger;
            _subscription = settings.Value.PubSub.EventProcessingServiceName;
            _topics = settings.Value.PubSub.Events;
        }

        public async Task SubscribeAsync()
        {
            var client = new ServiceBusClient(
                "pv-authorization.servicebus.windows.net",
                new DefaultAzureCredential());

            var options = new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            };

            var processor = client.CreateProcessor(_topics.First(), _subscription, options);

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
            finally
            {
                await processor.DisposeAsync();
                await client.DisposeAsync();
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
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

    }
}
