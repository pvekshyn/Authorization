extern alias GRPC;
using Grpc.Core;
using Grpc.Net.Client;
using NUnit.Framework;

namespace Assignment.Integration.Tests
{
    [TestFixture]
    public class AssigmentServiceTests
    {
        private HttpClient _grpcClient;

        [OneTimeSetUp]
        public void Init()
        {
            var factory = new CustomWebApplicationFactory<GRPC.Program>();
            _grpcClient = factory.CreateClient();
        }

        [Test]
        public async Task GetAssigments_ReturnAssignments()
        {
            // Arrange
            var channel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
            {
                HttpClient = _grpcClient
            });
            var client = new GrpcAssignmentService.GrpcAssignmentServiceClient(channel);

            var cts = new CancellationTokenSource();
            var hasMessages = false;
            var callCancelled = false;

            // Act
            using var call = client.GetAssignments(new GrpcAssignmentsRequest(), cancellationToken: cts.Token);
            try
            {
                await foreach (var message in call.ResponseStream.ReadAllAsync())
                {
                    hasMessages = true;
                    cts.Cancel();
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                callCancelled = true;
            }

            // Assert
            Assert.True(hasMessages);
            Assert.True(callCancelled);
        }
    }
}