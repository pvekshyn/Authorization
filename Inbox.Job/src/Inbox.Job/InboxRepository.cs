using Dapper;
using Inbox.Job.Infrastructure.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Inbox.Job.Infrastructure
{
    public interface IInboxRepository
    {
        InboxMessage? GetFirst();
        void Insert(string message, DateTime created);
        void Delete(Guid id);
        void InsertError(Guid id, Exception e);
        void InsertError(Guid inboxMessageId, string message, string stackTrace);
    }

    internal class InboxRepository : IInboxRepository
    {
        private string _connectionString;
        private readonly ILogger<InboxRepository> _logger;
        public InboxRepository(IConfiguration configuration, ILogger<InboxRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("Database");
            _logger = logger;
        }

        public InboxMessage? GetFirst()
        {
            var sql = "SELECT TOP 1 Id, Message, Created FROM InboxMessage ORDER BY Created";
            _logger.LogDebug(sql);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<InboxMessage>(sql);
            }
        }

        public void Insert(string message, DateTime created)
        {
            var sql = "INSERT INTO InboxMessage (Message, Created) VALUES (@message, @created)";
            _logger.LogDebug(sql);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { message, created });
            }
        }

        public void Delete(Guid id)
        {
            var sql = "DELETE FROM InboxMessage WHERE Id = @id";
            _logger.LogDebug(sql);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { id });
            }
        }

        public void InsertError(Guid inboxMessageId, Exception e)
        {
            InsertError(inboxMessageId, e.Message, e.StackTrace);
        }

        public void InsertError(Guid inboxMessageId, string message, string stackTrace)
        {
            var sql = "INSERT INTO InboxMessageError (InboxMessageId, Message, StackTrace, Created) VALUES (@inboxMessageId, @message, @stackTrace,  GETUTCDATE())";
            _logger.LogDebug(sql);

            var param = new
            {
                inboxMessageId,
                message,
                stackTrace
            };
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, param);
            }
        }
    }
}
