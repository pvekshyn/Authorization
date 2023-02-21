using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Outbox.Job.Infrastructure.Models;

namespace Outbox.Job.Infrastructure;
internal class OutboxPublisherAzure : IOutboxPublisher
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly ServiceBusClient _client;
    private readonly AzureSenderFactory _senderFactory;

    public OutboxPublisherAzure(
        IOutboxRepository outboxRepository,
        IConfiguration configuration)
    {
        _outboxRepository = outboxRepository;

        var managedIdentityClientId = configuration.GetSection("ManagedIdentityClientId")?.Value;
        var options = new DefaultAzureCredentialOptions { ManagedIdentityClientId = managedIdentityClientId };

        _client = new ServiceBusClient(
            "pv-authorization.servicebus.windows.net",
            new DefaultAzureCredential(options));

        _senderFactory = new AzureSenderFactory(_client, options);
    }

    public async Task PublishAsync(OutboxMessage outboxMessage)
    {
        try
        {
            var message = new ServiceBusMessage(outboxMessage.Message);
            message.ApplicationProperties.Add("Created", outboxMessage.Created);

            JObject json = JObject.Parse(outboxMessage.Message);
            var type = json["$type"].ToString();
            var eventName = type.Split(",")[0];

            var sender = await _senderFactory.GetSenderAsync(eventName);
            await sender.SendMessageAsync(message);

            _outboxRepository.Delete(outboxMessage.Id);
        }
        catch (Exception e)
        {
            if (_outboxRepository.ErrorsAny(outboxMessage.Id))
            {
                return;
            }

            _outboxRepository.InsertError(outboxMessage.Id, e);
        }
    }
}
