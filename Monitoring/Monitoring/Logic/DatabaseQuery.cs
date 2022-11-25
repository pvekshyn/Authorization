using Microsoft.Data.SqlClient;

namespace Monitoring.Logic
{
    public interface IDatabaseQuery
    {
        Task<int> GetAclCountAsync();
        Task<int> GetAclInboxCountAsync();
        Task<int> GetAssignmentOutboxCountAsync();
        Task<int> GetAssignmentsCountAsync();
    }

    public class DatabaseQuery : IDatabaseQuery
    {
        private static string _assignmentConnectionString = "Data Source=localhost\\SQLEXPRESS;Integrated Security=True;Initial Catalog=Assignment;TrustServerCertificate=True";
        private static string _authorizationConnectionString = "Data Source=localhost\\SQLEXPRESS;Integrated Security=True;Initial Catalog=Authorization;TrustServerCertificate=True";

        public async Task<int> GetAssignmentsCountAsync()
        {
            using SqlConnection connection = new SqlConnection(_assignmentConnectionString);
            await connection.OpenAsync();
            using SqlCommand commmand = new SqlCommand("SELECT COUNT(*) FROM Assignment", connection);
            return (int)commmand.ExecuteScalar();
        }

        public async Task<int> GetAssignmentOutboxCountAsync()
        {
            using SqlConnection connection = new SqlConnection(_assignmentConnectionString);
            await connection.OpenAsync();
            using SqlCommand commmand = new SqlCommand("SELECT COUNT(*) FROM OutboxMessage", connection);
            return (int)commmand.ExecuteScalar();
        }

        public async Task<int> GetAclInboxCountAsync()
        {
            using SqlConnection connection = new SqlConnection(_authorizationConnectionString);
            await connection.OpenAsync();
            using SqlCommand commmand = new SqlCommand("SELECT COUNT(*) FROM InboxMessage", connection);
            return (int)commmand.ExecuteScalar();
        }

        public async Task<int> GetAclCountAsync()
        {
            using SqlConnection connection = new SqlConnection(_authorizationConnectionString);
            await connection.OpenAsync();
            using SqlCommand commmand = new SqlCommand("SELECT COUNT(*) FROM AccessEntry", connection);
            return (int)commmand.ExecuteScalar();
        }
    }
}
