namespace Inbox.Job.Infrastructure.Models
{
    public class InboxMessage
    {
        public Guid Id { get; init; }
        public string Message { get; init; }
        public DateTime Created { get; init; }
    }
}
