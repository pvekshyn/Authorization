using Assignment.Domain.ValueObjects;
using Common.Domain;

namespace Assignment.Domain;

public class Assignment : IAggregateRoot
{
    public AssignmentId Id { get; private set; }
    public UserId UserId { get; private set; }
    public RoleId RoleId { get; private set; }

    // just for EF
    private Assignment()
    {
    }

    public Assignment(AssignmentId id, UserId userId, RoleId roleId)
    {
        Id = new AssignmentId(id);
        UserId = new UserId(userId);
        RoleId = new RoleId(roleId);
    }
}
