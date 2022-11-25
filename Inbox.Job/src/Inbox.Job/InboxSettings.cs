namespace Inbox.Job.Infrastructure;
public class InboxSettings
{
    public string JobName { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public PubSub PubSub { get; set; }
    public string EventProcessingServiceName { get; set; }
}

public class ConnectionStrings
{
    public string Database { get; set; }
}

public class PubSub
{
    public string QueueName { get; set; }
    public List<string> Subscriptions { get; set; }
}
