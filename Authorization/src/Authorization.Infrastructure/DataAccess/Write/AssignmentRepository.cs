using Assignment.SDK.DTO;
using Authorization.Application.Dependencies;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace Authorization.Infrastructure.DataAccess.Write
{
    internal class AssignmentRepository : IAssignmentRepository
    {
        private string _connectionString;
        public AssignmentRepository(IOptions<AuthorizationSettings> settings)
        {
            _connectionString = settings.Value.ConnectionStrings.Database;
        }

        public void AddAssignment(AssignmentDto assignment)
        {
            var sql = $"INSERT INTO Assignment (Id, UserId, RoleId) VALUES (@id, @userId, @roleId)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, assignment);
            }
        }

        public void DeleteAssignment(Guid id)
        {
            var sql = $"DELETE FROM Assignment WHERE Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { id = id });
            }
        }

        public void DeleteAssignments()
        {
            var sql = $"TRUNCATE TABLE Assignment";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql);
            }
        }

        public void BulkInsertAssignments(IReadOnlyCollection<AssignmentDto> assignments)
        {
            using (DataTable dt = new DataTable())
            {
                dt.Columns.Add("Id", typeof(Guid));
                dt.Columns.Add("UserId", typeof(Guid));
                dt.Columns.Add("RoleId", typeof(Guid));

                foreach (var assignment in assignments)
                {
                    dt.Rows.Add(assignment.Id, assignment.UserId, assignment.RoleId);
                }

                using var sqlBulk = new SqlBulkCopy(_connectionString);
                sqlBulk.DestinationTableName = "Assignment";
                sqlBulk.WriteToServer(dt);
            }
        }
    }
}
