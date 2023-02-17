using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json.Linq;
using Outbox.Job.Infrastructure.Models;

namespace Outbox.Job.Infrastructure;
internal class OutboxPublisherAzure : IOutboxPublisher
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly ServiceBusClient _client;
    private readonly AzureSenderFactory _senderFactory;

    public OutboxPublisherAzure(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;

        _client = new ServiceBusClient(
            "pv-authorization.servicebus.windows.net",
            new DefaultAzureCredential());

        _senderFactory = new AzureSenderFactory(_client);
    }

    public async Task PublishAsync(OutboxMessage outboxMessage)
    {
        try
        {
            //properties.Headers.Add("Created", outboxMessage.Created.ToString("o"));

            var message = new ServiceBusMessage(outboxMessage.Message);

            JObject json = JObject.Parse(outboxMessage.Message);
            var type = json["$type"].ToString();
            var eventName = type.Split(",")[0];

            await _senderFactory.GetSender(eventName).SendMessageAsync(message);

            _outboxRepository.Delete(outboxMessage.Id);
        }
        catch (Exception e)
        {
            _outboxRepository.InsertError(outboxMessage.Id, e);
        }
    }
}
