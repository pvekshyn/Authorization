using System.Text;
using EasyNetQ;
using EasyNetQ.Topology;
using Newtonsoft.Json.Linq;
using Outbox.Job.Infrastructure.Models;

namespace Outbox.Job.Infrastructure;
internal class OutboxPublisherRabbit : IOutboxPublisher
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly IAdvancedBus _bus;
    private readonly Exchange _exchange;

    public OutboxPublisherRabbit(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;

        _bus = RabbitHutch.CreateBus("host=localhost;publisherConfirms=true;timeout=1").Advanced;

        _exchange = _bus.ExchangeDeclare("box_exchange", ExchangeType.Topic);
    }

    public async Task PublishAsync(OutboxMessage outboxMessage)
    {
        try
        {
            var properties = new MessageProperties();
            properties.Headers.Add("Created", outboxMessage.Created.ToString("o"));

            var body = Encoding.UTF8.GetBytes(outboxMessage.Message);

            JObject json = JObject.Parse(outboxMessage.Message);
            var type = json["$type"].ToString();
            var routingKey = type.Split(",")[0];

            _bus.Publish(_exchange, routingKey, false, properties, body);

            _outboxRepository.Delete(outboxMessage.Id);
        }
        catch (Exception e)
        {
            _outboxRepository.InsertError(outboxMessage.Id, e);
        }
    }
}
