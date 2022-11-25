using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Outbox.Job.Infrastructure.Models;

namespace Outbox.Job.Infrastructure
{
    public interface IOutboxRepository
    {
        OutboxMessage GetFirst();
        void Delete(Guid id);
        void InsertError(Guid id, Exception e);
    }

    internal class OutboxRepository : IOutboxRepository
    {
        private string _connectionString;
        private readonly ILogger<OutboxRepository> _logger;
        public OutboxRepository(IConfiguration configuration, ILogger<OutboxRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("Database");
            _logger = logger;
        }

        public OutboxMessage GetFirst()
        {
            var sql = @"SELECT TOP 1 Id, Message 
                FROM OutboxMessage
                ORDER BY Created";
            _logger.LogDebug(sql);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<OutboxMessage>(sql);
            }
        }

        public void Delete(Guid id)
        {
            var sql = "DELETE FROM OutboxMessage WHERE Id = @id";
            _logger.LogDebug(sql);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { id, });
            }
        }

        public void InsertError(Guid outboxMessageId, Exception e)
        {
            var sql = "INSERT INTO OutboxMessageError (OutboxMessageId, Message, StackTrace, Created) VALUES (@outboxMessageId, @message, @stackTrace, GETUTCDATE())";
            _logger.LogDebug(sql);

            var param = new
            {
                outboxMessageId,
                message = e.Message,
                stackTrace = e.StackTrace
            };
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, param);
            }
        }
    }
}
