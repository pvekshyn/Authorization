namespace Authorization.Domain;

public class Role
{
    public Guid Id { get; init; }
    public string Name { get; init; }

    public ICollection<Permission> Permissions { get; init; }
}
