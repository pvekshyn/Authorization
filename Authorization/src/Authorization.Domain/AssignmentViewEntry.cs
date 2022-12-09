namespace Authorization.Domain;

public class AssignmentViewEntry
{
    public Guid UserId { get; init; }
    public string UserName { get; init; }

    public Guid RoleId { get; init; }
    public string RoleName { get; init; }

}
