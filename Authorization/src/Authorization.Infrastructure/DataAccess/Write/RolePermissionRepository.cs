using Authorization.Application.Dependencies;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Role.SDK.DTO;
using System.Data;

namespace Authorization.Infrastructure.DataAccess.Write
{
    internal class RolePermissionRepository : IRolePermissionRepository
    {
        private string _connectionString { get; init; }

        public RolePermissionRepository(IOptions<AuthorizationSettings> settings)
        {
            _connectionString = settings.Value.ConnectionStrings.Database;
        }

        public void AddRole(CreateRoleDto role)
        {
            var sql = "INSERT INTO Role (Id, Name) VALUES (@id, @name)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, role);
            }

            var rolePermissions = role.PermissionIds
                .Select<Guid, (Guid roleId, Guid permissionId)>(x => new(role.Id, x))
                .ToList();

            BulkInsertRolePermissions(rolePermissions);
        }

        public void RenameRole(RenameRoleDto role)
        {
            var sql = "UPDATE Role SET Name = @name WHERE Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, role);
            }
        }

        public void UpdateRolePermissions(UpdateRolePermissionsDto role)
        {
            var sql = "DELETE FROM RolePermission WHERE RoleId = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, role);
            }

            var rolePermissions = role.PermissionIds
                .Select<Guid, (Guid roleId, Guid permissionId)>(x => new(role.Id, x))
                .ToList();

            BulkInsertRolePermissions(rolePermissions);
        }

        public void DeleteRole(Guid id)
        {
            var sql = "DELETE FROM Role WHERE Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { id = id });
            }
        }

        public void DeleteRoles()
        {
            var sql = $"DELETE FROM Role";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql);
            }
        }

        public void DeletePermissions()
        {
            var sql = $"DELETE FROM Permission";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql);
            }
        }

        public void BulkInsertRoles(IReadOnlyCollection<(Guid Id, string Name)> roles)
        {
            using (DataTable dt = new DataTable())
            {
                dt.Columns.Add("Id", typeof(Guid));
                dt.Columns.Add("Name", typeof(string));

                foreach (var role in roles)
                {
                    dt.Rows.Add(role.Id, role.Name);
                }

                using var sqlBulk = new SqlBulkCopy(_connectionString);
                sqlBulk.DestinationTableName = "Role";
                sqlBulk.WriteToServer(dt);
            }
        }

        public void BulkInsertRolePermissions(IReadOnlyCollection<(Guid roleId, Guid permissionId)> rolePermissions)
        {
            using (DataTable dt = new DataTable())
            {
                dt.Columns.Add("Id", typeof(Guid));
                dt.Columns.Add("RoleId", typeof(Guid));
                dt.Columns.Add("PermissionId", typeof(Guid));

                foreach (var rolePermission in rolePermissions)
                {
                    dt.Rows.Add(Guid.NewGuid(), rolePermission.roleId, rolePermission.permissionId);
                }

                using var sqlBulk = new SqlBulkCopy(_connectionString);
                sqlBulk.DestinationTableName = "RolePermission";
                sqlBulk.WriteToServer(dt);
            }
        }

        public void BulkInsertPermissions(IReadOnlyCollection<(Guid Id, string Name)> permissions)
        {
            using (DataTable dt = new DataTable())
            {
                dt.Columns.Add("Id", typeof(Guid));
                dt.Columns.Add("Name", typeof(string));

                foreach (var permission in permissions)
                {
                    dt.Rows.Add(permission.Id, permission.Name);
                }

                using var sqlBulk = new SqlBulkCopy(_connectionString);
                sqlBulk.DestinationTableName = "Permission";
                sqlBulk.WriteToServer(dt);
            }
        }
    }
}
