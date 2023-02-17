using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Outbox.Job.Infrastructure.Models;

namespace Outbox.Job.Infrastructure;
internal class OutboxPublisherAzure : IOutboxPublisher
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly ServiceBusClient client;
    private readonly ServiceBusSender sender;

    public OutboxPublisherAzure(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;

        client = new ServiceBusClient(
            "pv-authorization.servicebus.windows.net",
            new DefaultAzureCredential());

        sender = client.CreateSender("role.sdk.events.rolecreatedevent");
    }

    public async Task PublishAsync(OutboxMessage outboxMessage)
    {
        try
        {
            //properties.Headers.Add("Created", outboxMessage.Created.ToString("o"));

            var message = new ServiceBusMessage(outboxMessage.Message);

            await sender.SendMessageAsync(message);

            _outboxRepository.Delete(outboxMessage.Id);
        }
        catch (Exception e)
        {
            _outboxRepository.InsertError(outboxMessage.Id, e);
        }
    }
}
