namespace Inbox.Job.Infrastructure
{
    public interface IInboxSubscriber
    {
        Task SubscribeAsync();
    }
}
