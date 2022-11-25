namespace Role.Infrastructure;
public class RoleSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
}

public class ConnectionStrings
{
    public string Database { get; set; }
}
