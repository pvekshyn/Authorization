extern alias API;
extern alias GRPC;
using Assignment.SDK.DTO;
using Common.SDK;
using Grpc.Net.Client;
using Newtonsoft.Json;
using Role.SDK.DTO;
using Role.SDK.Events;
using System.Net;
using System.Text;
using static Assignment.Application.Validation.Errors;

namespace Assignment.Integration.Tests
{
    public class AssignTests :
        IClassFixture<CustomWebApplicationFactory<API.Program>>,
        IClassFixture<CustomWebApplicationFactory<GRPC.Program>>
    {
        private readonly HttpClient _apiClient;
        private readonly HttpClient _grpcClient;

        public AssignTests(CustomWebApplicationFactory<API.Program> apiFactory, CustomWebApplicationFactory<GRPC.Program> grpcFactory)
        {
            Environment.SetEnvironmentVariable("SERVICE__role-grpc__HOST", "http://localhost");
            Environment.SetEnvironmentVariable("SERVICE__role-grpc__PORT", "5001");

            _apiClient = apiFactory.CreateClient();
            _grpcClient = grpcFactory.CreateClient();
        }

        [Fact]
        public async Task Assign_NotExistingRole_RoleNotFound()
        {
            // Arrange
            var request = new AssignmentDto()
            {
                Id = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _apiClient.PostAsync("/assign", jsonContent);

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result>(stringResponse);

            Assert.Equal(NOT_FOUND, result.Errors.Single().Code);
            Assert.EndsWith(nameof(request.RoleId), result.Errors.Single().Field);
        }

        [Fact]
        public async Task Assign_AfterRoleCreated_Success()
        {
            // Arrange
            var channel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
            {
                HttpClient = _grpcClient
            });
            var client = new GrpcEventProcessingService.GrpcEventProcessingServiceClient(channel);

            var roleCreatedEvent = new RoleCreatedEvent
            {
                Role = new CreateRoleDto { Id = Guid.NewGuid() }
            };

            var roleCreatedEventString = JsonConvert.SerializeObject(roleCreatedEvent, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            // Act
            var response = client.ProcessEvent(new GrpcEventRequest() { Event = roleCreatedEventString });


            var roleDeletedEvent = new RoleDeletedEvent
            {
                Id = roleCreatedEvent.Role.Id
            };
            var roleDeletedEventString = JsonConvert.SerializeObject(roleDeletedEvent, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            var response2 = client.ProcessEvent(new GrpcEventRequest() { Event = roleDeletedEventString });


        }
    }
}