namespace Role.SDK.DTO;

public class UpdateRolePermissionsDto
{
    public Guid Id { get; set; }

    public IReadOnlyCollection<Guid> PermissionIds { get; set; }
}
