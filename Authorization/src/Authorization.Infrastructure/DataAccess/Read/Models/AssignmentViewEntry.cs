namespace Authorization.Infrastructure.DataAccess.Read.Models;

public class AssignmentViewEntry
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }

    public Guid RoleId { get; set; }
    public string RoleName { get; set; }

}
