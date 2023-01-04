using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Globalization;
using System.Text;

namespace Inbox.Job.Infrastructure
{
    public interface IInboxSubscriber
    {
        Task SubscribeAsync();
    }

    public class InboxSubscriber : IInboxSubscriber
    {
        private readonly IInboxRepository _inboxRepository;
        private string _queueName;
        private List<string> _subscriptions;
        public InboxSubscriber(IInboxRepository inboxRepository, IOptions<InboxSettings> settings)
        {
            _inboxRepository = inboxRepository;
            _queueName = settings.Value.PubSub.QueueName;
            _subscriptions = settings.Value.PubSub.Subscriptions;
        }

        public async Task SubscribeAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var exchange = "box_exchange";
            var queue = _queueName;

            channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);
            channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

            foreach (var routingKey in _subscriptions)
            {
                channel.QueueBind(queue, exchange, routingKey, null);
            }

            var props = channel.CreateBasicProperties();
            props.Persistent = true;

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var createdHeader = ea.BasicProperties.Headers.Single(x => x.Key == "Created").Value;
                    var createdString = Encoding.UTF8.GetString((byte[])createdHeader);
                    var created = DateTime.Parse(createdString, null, DateTimeStyles.RoundtripKind);

                    var message = Encoding.UTF8.GetString(body);

                    _inboxRepository.Insert(message, created);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception)
                {
                    channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                }
            };
            channel.BasicConsume(queue: queue,
                                 autoAck: false,
                                 consumer: consumer);
        }
    }
}
