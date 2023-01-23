extern alias API;
extern alias GRPC;
using Assignment.Infrastructure;
using Assignment.SDK.Features;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Refit;

namespace Assignment.Integration.Tests;
public class IntegrationTestBase : IDisposable, IClassFixture<CustomWebApplicationFactory<API.Program>>,
        IClassFixture<CustomWebApplicationFactory<GRPC.Program>>
{
    protected readonly AssignmentDbContext _dbContext;
    protected readonly IAssignmentApi _apiClient;
    protected readonly GrpcEventProcessingService.GrpcEventProcessingServiceClient _grpcClient;

    public IntegrationTestBase(CustomWebApplicationFactory<API.Program> apiFactory, CustomWebApplicationFactory<GRPC.Program> grpcFactory)
    {
        _dbContext = CreateDbContext();
        _apiClient = CreateApiClient(apiFactory);
        _grpcClient = CreateGrpcClient(grpcFactory);
    }

    private AssignmentDbContext CreateDbContext()
    {
        var dbContextOptions = new DbContextOptionsBuilder<AssignmentDbContext>()
            .UseSqlServer(Constants.ConnectionString)
            .Options;

        return new AssignmentDbContext(dbContextOptions);
    }

    private IAssignmentApi CreateApiClient(CustomWebApplicationFactory<API.Program> apiFactory)
    {
        var httpClient = apiFactory.CreateClient();

        var settings = new RefitSettings
        {
            ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
        };

        return RestService.For<IAssignmentApi>(httpClient, settings);
    }

    private GrpcEventProcessingService.GrpcEventProcessingServiceClient CreateGrpcClient(CustomWebApplicationFactory<GRPC.Program> grpcFactory)
    {
        var channel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            HttpClient = grpcFactory.CreateClient()
        });

        return new GrpcEventProcessingService.GrpcEventProcessingServiceClient(channel);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
