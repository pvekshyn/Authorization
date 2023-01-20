using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Refit;
using Role.Infrastructure;
using Role.SDK.Features;

namespace Role.Integration.Tests;
public class IntegrationTestBase : IDisposable, IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected readonly RoleDbContext _dbContext;
    protected readonly IPermissionApi _permissionApiClient;
    protected readonly IRoleApi _roleApiClient;

    public IntegrationTestBase(CustomWebApplicationFactory<Program> apiFactory)
    {
        var dbContextOptions = new DbContextOptionsBuilder<RoleDbContext>()
            .UseSqlServer(Constants.ConnectionString)
            .Options;

        _dbContext = new RoleDbContext(dbContextOptions);

        var httpClient = apiFactory.CreateClient();

        var settings = new RefitSettings
        {
            ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
        };

        _permissionApiClient = RestService.For<IPermissionApi>(httpClient, settings);

        _roleApiClient = RestService.For<IRoleApi>(httpClient, settings);
    }

    protected bool OutboxMessageExists(Guid entityId)
    {
        var sql = @"SELECT TOP 1 1 
                FROM OutboxMessage
                WHERE EntityId = @entityId";

        using (var connection = new SqlConnection(Constants.ConnectionString))
        {
            return connection.QueryFirstOrDefault<bool>(sql, new { entityId = entityId });
        }
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
