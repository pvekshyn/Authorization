using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Authorization.Infrastructure.DataAccess.Read
{
    public interface IAccessRepository
    {
        bool CheckAccess(Guid userId, Guid permissionId);
    }

    internal class AccessRepository : IAccessRepository
    {
        private string _connectionString { get; init; }

        public AccessRepository(IOptions<AuthorizationSettings> settings)
        {
            _connectionString = settings.Value.ConnectionStrings.Database;
        }

        public bool CheckAccess(Guid userId, Guid permissionId)
        {
            var sql = $"SELECT 1 FROM [AccessEntry] AS [a] WHERE([a].[UserId] = @userId) AND([a].[PermissionId] = @permissionId)";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<bool>(sql, new { userId, permissionId });
            }
        }
    }
}
