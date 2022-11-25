using Assignment.Domain.ValueObjects;
using AutoFixture;

namespace Assignment.Application.Tests;

public static class TestExtensions
{
    public static Domain.Assignment CreateAssignment(this IFixture fixture, Guid userId, Guid roleId)
    {
        return new Domain.Assignment(
            new AssignmentId(Guid.NewGuid()),
            new UserId(userId),
            new RoleId(roleId));
    }
}
