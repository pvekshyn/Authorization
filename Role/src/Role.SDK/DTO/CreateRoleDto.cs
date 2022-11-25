namespace Role.SDK.DTO;

public class CreateRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public IReadOnlyCollection<Guid> PermissionIds { get; set; }
}
