namespace Authorization.Infrastructure.DataAccess.Read.Models;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<Permission> Permissions { get; set; }
}
