namespace Authorization.Infrastructure.DataAccess.Read.Models;

public class Permission
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    [Obsolete]//just for EF many to many
    public ICollection<Role> Roles { get; set; }
}
