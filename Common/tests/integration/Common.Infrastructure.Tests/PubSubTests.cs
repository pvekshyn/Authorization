using Common.Infrastructure.PubSub;
using Common.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Common.Infrastructure.Tests
{
    public class RedirectOutput : TextWriter
    {
        private readonly ITestOutputHelper _output;

        public RedirectOutput(ITestOutputHelper output)
        {
            _output = output;
        }

        public override Encoding Encoding { get; } // set some if required

        public override void WriteLine(string? value)
        {
            _output.WriteLine(value);
        }
    }

    public class TestObject1
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class TestObject2
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class PubSubTests
    {
        public PubSubTests(ITestOutputHelper output)
        {
            Console.SetOut(new RedirectOutput(output));
        }

        [Fact]
        public async Task PubSubTest_TwoSubscribersToDifferentMessages()
        {
            var settingsDb = new CommonServiceSettings
            {
                DbConnectionString = "Data Source=localhost\\SQLEXPRESS;Integrated Security=True;Initial Catalog=IntegrationTest;TrustServerCertificate=True"
            };

            var settingsSub1 = new CommonServiceSettings
            {
                PubSubQueueName = "int_testA"
            };

            var settingsSub2 = new CommonServiceSettings
            {
                PubSubQueueName = "int_testB"
            };

            IOptions<CommonServiceSettings> optionsDb = Options.Create(settingsDb);
            IOptions<CommonServiceSettings> optionsSub1 = Options.Create(settingsSub1);
            IOptions<CommonServiceSettings> optionsSub2 = Options.Create(settingsSub2);

            var entityId = Guid.NewGuid();
            var obj = new TestObject1 { Id = entityId, Name = "test1" };
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var message = JsonConvert.SerializeObject(obj, jsonSerializerSettings);
            var loggerOutboxMock = new Mock<ILogger<OutboxRepository>>();
            var outboxRepository = new OutboxRepository(optionsDb, loggerOutboxMock.Object);

            outboxRepository.Insert(entityId, message);

            var publisher = new OutboxPublisher(outboxRepository);

            var loggerInboxMock = new Mock<ILogger<InboxRepository>>();
            var inboxRepository = new InboxRepository(optionsDb, loggerInboxMock.Object);
            var subscriber1 = new InboxSubscriber(inboxRepository, optionsSub1);
            subscriber1.AddSubscription<TestObject1>();
            await subscriber1.SubscribeAsync();
            var subscriber2 = new InboxSubscriber(inboxRepository, optionsSub2);
            subscriber2.AddSubscription<TestObject2>();
            await subscriber2.SubscribeAsync();

            await publisher.PublishAsync(entityId);

            var outboxMessages = outboxRepository.GetPubSubMessages(entityId);
            Assert.Empty(outboxMessages);

            var inboxMessage = inboxRepository.GetFirst();
            Assert.NotNull(inboxMessage);
            Assert.Equal(message, inboxMessage.Message);
            inboxRepository.Delete(inboxMessage.Id);

            inboxMessage = inboxRepository.GetFirst();
            Assert.Null(inboxMessage);

            var obj2 = new TestObject2 { Id = entityId, Name = "test2" };
            message = JsonConvert.SerializeObject(obj2, jsonSerializerSettings);
            outboxRepository.Insert(entityId, message);
            await publisher.PublishAsync(entityId);

            inboxMessage = inboxRepository.GetFirst();
            Assert.Equal(message, inboxMessage.Message);
            inboxRepository.Delete(inboxMessage.Id);

            inboxMessage = inboxRepository.GetFirst();
            Assert.Null(inboxMessage);
        }

        public class TestObject11
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class TestObject12
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        [Fact]
        public async Task PubSubTest_OnesubscriberToTwoMessages()
        {
            var settingsDb = new CommonServiceSettings
            {
                DbConnectionString = "Data Source=localhost\\SQLEXPRESS;Integrated Security=True;Initial Catalog=IntegrationTest;TrustServerCertificate=True"
            };

            var settingsSub1 = new CommonServiceSettings
            {
                PubSubQueueName = "int_testC"
            };

            IOptions<CommonServiceSettings> optionsDb = Options.Create(settingsDb);
            IOptions<CommonServiceSettings> optionsSub1 = Options.Create(settingsSub1);

            var entityId = Guid.NewGuid();
            var obj = new TestObject11 { Id = Guid.NewGuid(), Name = "test1" };
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var loggerOutboxMock = new Mock<ILogger<OutboxRepository>>();
            var outboxRepository = new OutboxRepository(optionsDb, loggerOutboxMock.Object);

            var message1 = JsonConvert.SerializeObject(obj, jsonSerializerSettings);
            outboxRepository.Insert(entityId, message1);

            var obj2 = new TestObject12 { Id = Guid.NewGuid(), Name = "test2" };
            var message2 = JsonConvert.SerializeObject(obj2, jsonSerializerSettings);
            outboxRepository.Insert(entityId, message2);

            var publisher = new OutboxPublisher(outboxRepository);

            var loggerInboxMock = new Mock<ILogger<InboxRepository>>();
            var inboxRepository = new InboxRepository(optionsDb, loggerInboxMock.Object);
            var subscriber1 = new InboxSubscriber(inboxRepository, optionsSub1);
            subscriber1.AddSubscription<TestObject11>();
            subscriber1.AddSubscription<TestObject12>();
            await subscriber1.SubscribeAsync();

            await publisher.PublishAsync(entityId);

            var outboxMessages = outboxRepository.GetPubSubMessages(entityId);
            Assert.Empty(outboxMessages);

            var inboxMessage = inboxRepository.GetFirst();
            Assert.Equal(message1, inboxMessage.Message);
            inboxRepository.Delete(inboxMessage.Id);

            inboxMessage = inboxRepository.GetFirst();
            Assert.NotNull(inboxMessage);
            Assert.Equal(message2, inboxMessage.Message);
            inboxRepository.Delete(inboxMessage.Id);

            inboxMessage = inboxRepository.GetFirst();
            Assert.Null(inboxMessage);
        }
    }
}